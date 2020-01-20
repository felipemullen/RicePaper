using System;
using System.IO;
using Foundation;
using Newtonsoft.Json;

namespace RicePaper.Lib.Model
{
    public class AppState
    {
        public int ImageIndex;
        public int WordIndex;
        public static AppState Default
        {
            get
            {
                return new AppState()
                {
                    ImageIndex = 0,
                    WordIndex = 0
                };
            }
        }
    }

    public class AppSettings
    {
        #region Instance Fields
        private ImageOptionType _imageOption;
        private string _imagePath;
        private TextOptions _textOptions;
        private DrawPosition _drawPosition;
        private WordListSelection _wordList;
        private string _wordListPath;
        private WordSelection _wordSelection;
        private DictionarySelection _dictionary;
        private CycleInfo _imageCycle;
        private CycleInfo _wordCycle;
        private AppState _state;
        #endregion

        #region Static Instance
        private static AppSettings _instance;
        private static NSString DATA_KEY = new NSString("ricepaperserialized");
        private static NSUserDefaults valueStore = NSUserDefaults.StandardUserDefaults;

        public static AppSettings Default
        {
            get
            {
                return new AppSettings()
                {
                    _dictionary = DictionarySelection.JapanDict,
                    _drawPosition = DrawPosition.LeftTop,
                    _imageCycle = CycleInfo.Default,
                    _imageOption = ImageOptionType.Japan,
                    _imagePath = GetFolderPath(ImageOptionType.Japan),
                    _state = new AppState(),
                    _textOptions = TextOptions.Default,
                    _wordCycle = CycleInfo.Default,
                    _wordList = WordListSelection.MostFrequent1000,
                    _wordListPath = GetFolderPath(WordListSelection.MostFrequent1000),
                    _wordSelection = WordSelection.InOrder
                };
            }
        }

        public static void Load()
        {
            try
            {
                NSString data = valueStore.ValueForKey(DATA_KEY) as NSString;
                AppSettings appSettings = JsonConvert.DeserializeObject<AppSettings>(data);
                _instance = appSettings;
            }
            catch
            {
                _instance = AppSettings.Default;
            }
        }

        public static void Save()
        {
            string jsonString = JsonConvert.SerializeObject(_instance);
            NSString data = new NSString(jsonString);
            valueStore.SetValueForKey(data, DATA_KEY);
        }
        #endregion

        #region Static Properties
        public static ImageOptionType ImageOption
        {
            get { return _instance._imageOption; }
            set { _instance._imageOption = value; }
        }

        public static string ImagePath
        {
            get
            {
                return (ImageOption == ImageOptionType.Custom)
                    ? _instance._imagePath
                    : GetFolderPath(ImageOption);
            }
            set
            {
                if (ImageOption == ImageOptionType.Custom)
                    _instance._imagePath = value;
            }
        }

        public static TextOptions TextOptions
        {
            get { return _instance._textOptions; }
            set { _instance._textOptions = value; }
        }

        public static DrawPosition drawPosition
        {
            get { return _instance._drawPosition; }
            set { _instance._drawPosition = value; }
        }

        public static WordListSelection WordList
        {
            get { return _instance._wordList; }
            set { _instance._wordList = value; }
        }

        public static string WordListPath
        {
            get
            {
                if (WordList == WordListSelection.Custom)
                    return _instance._wordListPath;

                return GetFolderPath(WordList);
            }
            set
            {
                if (WordList == WordListSelection.Custom)
                    _instance._wordListPath = value;
            }
        }

        public static WordSelection WordSelection
        {
            get { return _instance._wordSelection; }
            set { _instance._wordSelection = value; }
        }

        public static DictionarySelection Dictionary
        {
            get { return _instance._dictionary; }
            set { _instance._dictionary = value; }
        }

        public static CycleInfo ImageCycle
        {
            get { return _instance._imageCycle; }
            set { _instance._imageCycle = value; }
        }

        public static CycleInfo WordCycle
        {
            get { return _instance._wordCycle; }
            set { _instance._wordCycle = value; }
        }

        public static AppState State
        {
            get { return _instance._state; }
            set { _instance._state = value; }
        }
        #endregion

        #region Static Helpers
        private static string GetFolderPath(ImageOptionType type)
        {
            return Path.Join(AppContext.BaseDirectory, "images", type.ToString());
        }

        private static string GetFolderPath(WordListSelection list)
        {
            return Path.Join(AppContext.BaseDirectory, "wordList", list.ToString());
        }
        #endregion
    }
}
