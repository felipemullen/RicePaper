using System;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using RicePaper.Lib.Dictionary;
using RicePaper.Lib.Model;

namespace RicePaper.Lib
{
    public class RiceScheduler : NSObject
    {
        #region Constants
        private const int CYCLE_INTERVAL_SEC = 60;
        private const int CYCLE_THRESHOLD_MS = 1500;
        #endregion

        #region Private Fields
        private readonly AppSettings settings;
        private readonly WallpaperMaker wallpaperUtility;
        private readonly RiceDictionary riceDict;
        private readonly WallpaperList imageList;

        private Timer _cycleTimer;
        #endregion

        #region Constructor
        public RiceScheduler(AppSettings settings, RiceDictionary riceDict, WallpaperList imageList)
        {
            this.settings = settings;
            wallpaperUtility = new WallpaperMaker();
            this.riceDict = riceDict;
            this.imageList = imageList;
        }
        #endregion

        #region Public Methods
        public void BeginScheduling()
        {
            var callback = new TimerCallback(TimerTick);
            _cycleTimer = new Timer(TimerTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(CYCLE_INTERVAL_SEC));
        }
        #endregion

        #region Private Helpers
        private bool CyclePeriodHasElapsed(DateTime lastUpdate, TimeSpan period)
        {
            var now = DateTime.Now;
            var threshold = TimeSpan.FromMilliseconds(CYCLE_THRESHOLD_MS);

            var timeDelta = (now - lastUpdate);
            return timeDelta >= period.Subtract(threshold);
        }

        private void TimerTick(Object o)
        {
            bool shouldUpdateImage = CyclePeriodHasElapsed(settings.LastImageChange, settings.ImageCycle.Period) && settings.ImageOption != ImageOptionType.Unchanged;
            bool shouldUpdateWord = CyclePeriodHasElapsed(settings.LastWordChange, settings.WordCycle.Period);

            Console.WriteLine("tick");

            using (var pool = new NSAutoreleasePool())
            {
                pool.InvokeOnMainThread(() =>
                {
                    if (shouldUpdateImage || shouldUpdateWord)
                    {
                        Update(shouldUpdateImage, shouldUpdateWord);
                    }
                    GC.Collect();
                });
            }
        }

        private void Update(bool changeImage, bool changeWord)
        {
            var now = DateTime.Now;

            imageList.Reload();

            if (changeImage && settings.ImageOption != ImageOptionType.Unchanged)
            {
                settings.ImageIndex = imageList.Increment(SelectionMode.InOrder);
                settings.LastImageChange = now;
                Console.WriteLine("updating lastImageChange");
            }

            if (changeWord)
            {
                settings.WordIndex = riceDict.Increment(settings.WordSelection);
                settings.LastWordChange = now;
                Console.WriteLine("updating lastWordChange");
            }

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

            wallpaperUtility.SetWallpaper(imageList.CurrentItem, settings.ImageOption, parameters);
        }

        /// <summary>
        /// Similar to Update() except it will reset timers to make sure there
        /// are no double updates taking place
        /// </summary>
        public Task ForcedUpdate(bool changeImage, bool changeWord)
        {
            return Task.Run(() =>
            {
                Update(changeImage, changeWord);
            });
        }
        #endregion

    }
}
