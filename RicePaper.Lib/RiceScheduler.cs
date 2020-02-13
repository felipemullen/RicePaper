using System;
using System.Collections.Generic;
using System.Threading;
using Foundation;
using RicePaper.Lib.Dictionary;
using RicePaper.Lib.Model;

namespace RicePaper.Lib
{
    public class RiceScheduler : NSObject
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
            wallpaperUtility = new WallpaperMaker(settings);
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
            if (changeImage && settings.ImageOption != ImageOptionType.Unchanged)
                settings.ImageIndex = imageList.Increment(settings.WordSelection);

            if (changeWord)
                settings.WordIndex = riceDict.Increment(settings.WordSelection);

            TextDetails currentWord = riceDict.CurrentDefinition(settings);

            var parameters = new DrawParameters
            {
                Position = settings.DrawPosition,
                ChangeWord = changeWord,
                ChangeWallpaper = changeImage,
                PrimaryTextScale = settings.PrimaryTextScale,
                SecondaryTextScale = settings.SecondaryTextScale,
                Text = currentWord
            };

            string imagePath = (settings.ImageOption == ImageOptionType.Unchanged)
                ? string.Empty
                : imageList.CurrentItem;
            wallpaperUtility.SetWallpaper(imagePath, parameters);
        }

        /// <summary>
        /// Similar to Update() except it will reset timers to make sure there
        /// are no double updates taking place
        /// </summary>
        public void ForcedUpdate(bool changeImage, bool changeWord)
        {
            this.BeginInvokeOnMainThread(() =>
            {
                BeginScheduling(startImmediate: false);
                Update(changeImage, changeWord);
            });
        }
        #endregion

    }
}
