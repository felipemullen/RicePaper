using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Threading;
using Foundation;
using RicePaper.Lib.Model;

namespace RicePaper.Lib
{
    public class RiceScheduler
    {
        #region Private Fields
        private readonly WallpaperMaker wallpaperUtility;

        private DateTime lastImageChange;
        private DateTime lastWordChange;
        private readonly List<Timer> timers;

        private List<string> wallpaperPaths;
        private List<string> wordList;
        #endregion

        #region Constructor
        public RiceScheduler()
        {
            wallpaperUtility = new WallpaperMaker();
            timers = new List<Timer>();
            lastImageChange = DateTime.MinValue;
            lastWordChange = DateTime.MinValue;

            wallpaperPaths = GetWallpapers();
            wordList = new List<string>() { "felipo" };
        }

        private List<string> GetWallpapers()
        {
            // TODO: load from directory

            return new List<string>()
            {
                { "/Users/fmullen/Desktop/backgrounds/bigger_bigger.png" },
                { "/Users/fmullen/Desktop/backgrounds/long_bigger.png" },
                { "/Users/fmullen/Desktop/backgrounds/long_smaller.png" },
                { "/Users/fmullen/Desktop/backgrounds/tall_bigger.png" },
                { "/Users/fmullen/Desktop/backgrounds/tall_smaller.png" }
            };
        }
        #endregion

        #region Public Methods
        public void BeginScheduling()
        {
            CancelAllTimers();

            if (AppSettings.ImageCycle.Period == AppSettings.WordCycle.Period)
            {
                ScheduleTask(AppSettings.ImageCycle, () => { Update(true, true); });
            }
            else
            {
                ScheduleTask(AppSettings.ImageCycle, () => { Update(true, false); });
                ScheduleTask(AppSettings.WordCycle, () => { Update(false, true); });
            }
        }
        #endregion

        #region Private Helpers
        private void CancelAllTimers()
        {
            foreach (var timer in timers)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        private void ScheduleTask(CycleInfo cycle, Action task)
        {
            TimeSpan timeLeft = TimeSpan.FromSeconds(2);

            var timer = new Timer(x =>
            {
                using (var pool = new NSAutoreleasePool())
                {
                    pool.InvokeOnMainThread(task);
                }
            }, null, timeLeft, cycle.Period);

            timers.Add(timer);
        }

        private void Update(bool changeImage, bool changeWord)
        {
            // TODO: Word resolution
            int wordIndex = AppSettings.State.WordIndex;
            var currentWord = "";

            var parameters = new DrawParameters
            {
                Position = AppSettings.drawPosition,
                ChangeWord = changeWord,
                ChangeWallpaper = changeImage,
                Text = GetDetailsFromWord(currentWord)
            };

            int imageIndex = AppSettings.State.ImageIndex;
            var imagePath = wallpaperPaths[imageIndex];

            wallpaperUtility.SetWallpaper(imagePath, parameters);

            if (changeImage)
                AppSettings.State.ImageIndex = (imageIndex + 1) % wallpaperPaths.Count;
            if (changeWord)
                AppSettings.State.WordIndex = (wordIndex + 1) % wordList.Count;

            lastWordChange = DateTime.Now;
            lastImageChange = DateTime.Now;
        }

        public TextDetails GetDetailsFromWord(string word)
        {
            // TODO: Dictionary lookup
            return new TextDetails
            {
                Kanji = "音楽",
                Furigana = "おんがく",
                Romaji = "ongaku",
                Definition = "song/music",
                JapaneseSentence = "僕は音楽が大好きですね",
                EnglishSentence = "I love music ya kno"
            };
        }
        #endregion

    }
}
