using System.Collections.Generic;
using System.IO;
using AppKit;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;

namespace RicePaper.Lib.Model
{
    public class AppSettings
    {
        #region Constants
        private static float DEFAULT_TEXT_SCALE = 0.75f;
        private static float MIN_TEXT_SCALE = 0.5f;
        private static float MAX_TEXT_SCALE = 1.0f;
        #endregion

        #region Static Instance
        private static NSString DATA_KEY = new NSString("RP_DATA");
        private static NSUserDefaults valueStore = NSUserDefaults.StandardUserDefaults;

        public static AppSettings Default
        {
            get
            {
                return new AppSettings()
                {
                    SavedDesktop = StoreCurrentDesktop(),
                    Dictionary = DictionarySelection.JapanDict,
                    DrawPosition = DrawPosition.LeftTop,
                    ImageCycle = CycleInfo.Default,
                    ImageOption = ImageOptionType.Unchanged,
                    UserImagePath = string.Empty,
                    ImageIndex = 0,
                    WordIndex = 0,
                    TextOptions = TextOptions.Default,
                    WordCycle = CycleInfo.Default,
                    WordList = WordListSelection.MostFrequent1000,
                    UserWordListPath = string.Empty,
                    WordSelection = WordSelectionMode.InOrder,
                    PrimaryTextScale = DEFAULT_TEXT_SCALE,
                    SecondaryTextScale = DEFAULT_TEXT_SCALE
                };
            }
        }

        public static AppSettings Load()
        {
            try
            {
                NSString data = valueStore.ValueForKey(DATA_KEY) as NSString;

                if (string.IsNullOrWhiteSpace(data) || data == "{}")
                    return AppSettings.Default;

                AppSettings appSettings = JsonConvert.DeserializeObject<AppSettings>(data);
                IntegrityCheck(appSettings);

                return appSettings;
            }
            catch
            {
                return AppSettings.Default;
            }
        }

        public static void IntegrityCheck(AppSettings settings)
        {
            if (settings.ImageCycle == null)
                settings.ImageCycle = CycleInfo.Default;

            if (settings.ImageOption == ImageOptionType.Custom && string.IsNullOrWhiteSpace(settings.UserImagePath))
                settings.ImageOption = ImageOptionType.Unchanged;

            if (settings.TextOptions == null)
                settings.TextOptions = TextOptions.Default;

            if (settings.WordCycle == null)
                settings.WordCycle = CycleInfo.Default;

            if (settings.WordList == WordListSelection.Custom && string.IsNullOrWhiteSpace(settings.WordListPath))
                settings.WordList = WordListSelection.MostFrequent1000;

            if (settings.PrimaryTextScale < MIN_TEXT_SCALE || settings.PrimaryTextScale > MAX_TEXT_SCALE)
                settings.PrimaryTextScale = DEFAULT_TEXT_SCALE;

            if (settings.SecondaryTextScale < MIN_TEXT_SCALE || settings.SecondaryTextScale > MAX_TEXT_SCALE)
                settings.SecondaryTextScale = DEFAULT_TEXT_SCALE;

            if (settings.SavedDesktop == null || settings.SavedDesktop.Count == 0)
                settings.SavedDesktop = StoreCurrentDesktop();
        }

        public static void Save(AppSettings settings)
        {
            string jsonString = JsonConvert.SerializeObject(settings);
            NSString data = new NSString(jsonString);
            valueStore.SetValueForKey(data, DATA_KEY);
            valueStore.Synchronize();
        }

        public static string GetFolderPath(ImageOptionType type)
        {
            switch (type)
            {
                case ImageOptionType.MacDefault:
                    return "/Library/Desktop Pictures";
                default:
                    return Path.Combine(Util.AppRoot, "Resources/Content/Images", type.ToString());
            }
        }

        public static string GetFilePath(WordListSelection list)
        {
            return Path.Combine(Util.AppRoot, "Resources/Content/WordLists", $"{list}.list");
        }

        private static Dictionary<string, string> StoreCurrentDesktop()
        {
            var paths = new Dictionary<string, string>();
            foreach (var screen in NSScreen.Screens)
            {
                try
                {
                    var id = Util.ScreenId(screen);
                    NSUrl filepath = NSWorkspace.SharedWorkspace.DesktopImageUrl(screen);
                    paths.Add(id.ToString(), filepath.ToString());
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            }

            return paths;
        }

        public static void RestoreSavedDesktop(AppSettings settings)
        {
            foreach (var item in settings.SavedDesktop)
            {
                foreach (var screen in NSScreen.Screens)
                {
                    var id = Util.ScreenId(screen);
                    if (id == item.Key)
                    {
                        NSError errorContainer = new NSError();

                        var workspace = NSWorkspace.SharedWorkspace;
                        var options = workspace.DesktopImageOptions(screen);
                        var url = new NSUrl(item.Value);
                        NSWorkspace.SharedWorkspace.SetDesktopImageUrl(url, screen, options, errorContainer);
                    }
                }
            }
        }
        #endregion

        #region Properties
        public ImageOptionType ImageOption { get; set; }

        public string ImagePath
        {
            get
            {
                return (ImageOption == ImageOptionType.Custom)
                    ? UserImagePath
                    : GetFolderPath(ImageOption);
            }
        }

        public string UserImagePath { get; set; }

        public TextOptions TextOptions { get; set; }

        public DrawPosition DrawPosition { get; set; }

        public WordListSelection WordList { get; set; }

        public string WordListPath
        {
            get
            {
                return (WordList == WordListSelection.Custom)
                    ? UserWordListPath
                    : GetFilePath(WordList);
            }
        }

        public string UserWordListPath { get; set; }

        public WordSelectionMode WordSelection { get; set; }

        public DictionarySelection Dictionary { get; set; }

        public CycleInfo ImageCycle { get; set; }

        public CycleInfo WordCycle { get; set; }

        public int ImageIndex { get; set; }

        public int WordIndex { get; set; }

        public float PrimaryTextScale { get; set; }

        public float SecondaryTextScale { get; set; }

        public Dictionary<string, string> SavedDesktop { get; private set; }
        #endregion

        /// <summary>
        /// Marked private to avoid creating an instance
        /// without loading data
        /// </summary>
        private AppSettings() { }
    }
}
