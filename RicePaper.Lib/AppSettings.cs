using System;
using System.IO;
using Foundation;
using Newtonsoft.Json;

namespace RicePaper.Lib.Model
{
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
        private Cycle _imageCycle;
        private Cycle _wordCycle;
        #endregion

        #region Static Instance
        private static AppSettings _instance;
        private static NSString DATA_KEY = new NSString("ricepaperserialized");
        private static NSUserDefaults valueStore = NSUserDefaults.StandardUserDefaults;

        public static void Load()
        {
            NSString data = valueStore.ValueForKey(DATA_KEY) as NSString;
            AppSettings appSettings = JsonConvert.DeserializeObject<AppSettings>(data);
            _instance = appSettings;
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
            get { return (_instance != null) ? _instance._imageOption : ImageOptionType.Japan; }
            set
            {
                if (_instance != null)
                    _instance._imageOption = value;
            }
        }

        public static string ImagePath
        {
            get
            {
                if (_instance != null && ImageOption == ImageOptionType.Custom)
                    return _instance._imagePath;

                return GetFolderPath(ImageOption);
            }
            set
            {
                if (_instance != null && ImageOption == ImageOptionType.Custom)
                    _instance._imagePath = value;
            }
        }

        public static TextOptions TextOptions
        {
            get { return (_instance != null && _instance._textOptions != null) ? _instance._textOptions : TextOptions.Default; }
            set
            {
                if (_instance != null)
                    _instance._textOptions = value;
            }
        }

        public static DrawPosition drawPosition
        {
            get { return (_instance != null) ? _instance._drawPosition : DrawPosition.LeftTop; }
            set
            {
                if (_instance != null)
                    _instance._drawPosition = value;
            }
        }

        public static WordListSelection WordList
        {
            get { return (_instance != null) ? _instance._wordList : WordListSelection.MostFrequent1000; }
            set
            {
                if (_instance != null)
                    _instance._wordList = value;
            }
        }

        public static string WordListPath
        {
            get
            {
                if (_instance != null && WordList == WordListSelection.Custom)
                    return _instance._wordListPath;

                return GetFolderPath(WordList);
            }
            set
            {
                if (_instance != null && WordList == WordListSelection.Custom)
                    _instance._wordListPath = value;
            }
        }

        public static WordSelection WordSelection
        {
            get { return (_instance != null) ? _instance._wordSelection : WordSelection.InOrder; }
            set
            {
                if (_instance != null)
                    _instance._wordSelection = value;
            }
        }

        public static DictionarySelection Dictionary
        {
            get { return (_instance != null) ? _instance._dictionary : DictionarySelection.Jisho; }
            set
            {
                if (_instance != null)
                    _instance._dictionary = value;
            }
        }

        public static Cycle ImageCycle
        {
            get { return (_instance != null) ? _instance._imageCycle : Cycle.Default; }
            set
            {
                if (_instance != null)
                    _instance._imageCycle = value;
            }
        }

        public static Cycle WordCycle
        {
            get { return (_instance != null) ? _instance._wordCycle : Cycle.Default; }
            set
            {
                if (_instance != null)
                    _instance._wordCycle = value;
            }
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
