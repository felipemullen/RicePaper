using System;
using System.Collections.Generic;
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
        public RiceScheduler(AppSettings settings)
        {
            this.settings = settings;
            wallpaperUtility = new WallpaperMaker();
            riceDict = new RiceDictionary(settings);
            imageList = new WallpaperList(settings);

            timers = new List<Timer>();
        }
        #endregion

        #region Public Methods
        public void BeginScheduling()
        {
            CancelAllTimers();

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
            var currentWord = riceDict.CurrentDefinition();

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
                imageList.Increment();
            if (changeWord)
                riceDict.Increment();

            try
            {
                AppSettings.Save(settings);
            }
            catch (Exception) { }
        }
        #endregion

    }
}
