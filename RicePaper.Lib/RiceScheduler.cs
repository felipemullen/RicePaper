using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Foundation;
using RicePaper.Lib.Dictionary;
using RicePaper.Lib.Model;

namespace RicePaper.Lib
{
    public class RiceScheduler
    {
        #region Private Fields
        private readonly AppSettings settings;
        private readonly WallpaperMaker wallpaperUtility;
        private readonly RiceDictionary riceDict;
        private readonly WallpaperList imageList;

        private readonly List<Timer> timers;
        #endregion

        #region Constructor
        public RiceScheduler(AppSettings settings, RiceDictionary riceDict, WallpaperList imageList)
        {
            this.settings = settings;
            wallpaperUtility = new WallpaperMaker();
            this.riceDict = riceDict;
            this.imageList = imageList;

            timers = new List<Timer>();
        }
        #endregion

        #region Public Methods
        public void BeginScheduling(bool startImmediate = true)
        {
            CancelAllTimers();

            if (startImmediate == true)
            {
                settings.ImageCycle.DueTime = null;
                settings.WordCycle.DueTime = null;
            }
            else
            {
                settings.ImageCycle.DueTime = DateTime.Now.Add(settings.ImageCycle.Period) - DateTime.Now;
                settings.WordCycle.DueTime = DateTime.Now.Add(settings.WordCycle.Period) - DateTime.Now;
            }

            if (settings.ImageCycle.Period == settings.WordCycle.Period)
            {
                ScheduleTask(settings.ImageCycle, () => { Update(true, true); });
            }
            else
            {
                ScheduleTask(settings.ImageCycle, () => { Update(true, false); });
                ScheduleTask(settings.WordCycle, () => { Update(false, true); });
            }
        }
        #endregion

        #region Private Helpers
        private void CancelAllTimers()
        {
            for (int i = timers.Count - 1; i >= 0; i--)
            {
                var timer = timers[i];
                timer.Change(Timeout.Infinite, Timeout.Infinite);

                timers.RemoveAt(i);
            }
        }

        private void ScheduleTask(CycleInfo cycle, Action task)
        {
            TimeSpan timeLeft = (cycle.DueTime != null)
                ? (TimeSpan)cycle.DueTime
                : TimeSpan.Zero;

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
            TextDetails currentWord = riceDict.CurrentDefinition();

            var parameters = new DrawParameters
            {
                Position = settings.DrawPosition,
                ChangeWord = changeWord,
                ChangeWallpaper = changeImage,
                Text = currentWord
            };

            var imagePath = imageList.CurrentItem;
            wallpaperUtility.SetWallpaper(imagePath, parameters);

            if (changeImage)
                settings.ImageIndex = imageList.Increment();

            if (changeWord)
                settings.WordIndex = riceDict.Increment();
        }

        /// <summary>
        /// Similar to Update() except it will reset timers to make sure there
        /// are no double updates taking place
        /// </summary>
        public void ForcedUpdate(bool changeImage, bool changeWord)
        {
            BeginScheduling(startImmediate: false);
            Update(changeImage, changeWord);
        }
        #endregion

    }
}
