using System;
using System.IO;
using Foundation;
using Newtonsoft.Json;

namespace RicePaper.Lib.Model
{
    public class AppSettings
    {
        #region Static Instance
        private static NSString DATA_KEY = new NSString("RP_DATA");
        private static NSUserDefaults valueStore = NSUserDefaults.StandardUserDefaults;

        public static AppSettings Default
        {
            get
            {
                return new AppSettings()
                {
                    Dictionary = DictionarySelection.JapanDict,
                    DrawPosition = DrawPosition.LeftTop,
                    ImageCycle = CycleInfo.Default,
                    ImageOption = ImageOptionType.Japan,
                    ImagePath = GetFolderPath(ImageOptionType.Japan),
                    State = new AppState(),
                    TextOptions = TextOptions.Default,
                    WordCycle = CycleInfo.Default,
                    WordList = WordListSelection.MostFrequent1000,
                    WordListPath = GetFilePath(WordListSelection.MostFrequent1000),
                    WordSelection = WordSelection.InOrder
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

            if (settings.ImageOption == ImageOptionType.Custom && settings.ImagePath == null)
                settings.ImageOption = ImageOptionType.Japan;

            if (settings.State == null)
                settings.State = new AppState();

            if (settings.TextOptions == null)
                settings.TextOptions = TextOptions.Default;

            if (settings.WordCycle == null)
                settings.WordCycle = CycleInfo.Default;

            if (settings.WordList == WordListSelection.Custom && settings.WordListPath == null)
                settings.WordList = WordListSelection.MostFrequent1000;
        }

        public static void Save(AppSettings settings)
        {
            string jsonString = JsonConvert.SerializeObject(settings);
            NSString data = new NSString(jsonString);
            valueStore.SetValueForKey(data, DATA_KEY);
        }

        private static string GetFolderPath(ImageOptionType type)
        {

            return Path.Combine(AppRoot, "Resources/Content/Images", type.ToString());
        }

        private static string GetFilePath(WordListSelection list)
        {
            return Path.Combine(AppRoot, "Resources/Content/WordLists", $"{list}.list");
        }

        // TODO: Move to util class
        private static string AppRoot => Directory.GetParent(AppContext.BaseDirectory.TrimEnd('/')).FullName;
        #endregion

        #region Properties
        public ImageOptionType ImageOption { get; set; }

        public string ImagePath
        {
            get
            {
                return (ImageOption == ImageOptionType.Custom)
                    ? ImagePath
                    : GetFolderPath(ImageOption);
            }
            set
            {
                if (ImageOption == ImageOptionType.Custom)
                    ImagePath = value;
            }
        }

        public TextOptions TextOptions { get; set; }

        public DrawPosition DrawPosition { get; set; }

        public WordListSelection WordList { get; set; }

        public string WordListPath
        {
            get
            {
                if (WordList == WordListSelection.Custom)
                    return WordListPath;

                return GetFilePath(WordList);
            }
            set
            {
                if (WordList == WordListSelection.Custom)
                    WordListPath = value;
            }
        }

        public WordSelection WordSelection { get; set; }

        public DictionarySelection Dictionary { get; set; }

        public CycleInfo ImageCycle { get; set; }

        public CycleInfo WordCycle { get; set; }

        public AppState State { get; set; }

        public string ImagePathLabel => (ImageOption == ImageOptionType.Custom) ? ImagePath : $"Image Set: {ImageOption.ToString()}";
        public string WordListPathLabel => (WordList == WordListSelection.Custom) ? WordListPath : $"Word List: {WordList.ToString()}";
        #endregion

        /// <summary>
        /// Marked private to avoid creating an instance
        /// without loading data
        /// </summary>
        private AppSettings() { }
    }
}
