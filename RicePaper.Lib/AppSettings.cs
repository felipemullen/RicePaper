﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using AppKit;
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
                    LastImageChange = DateTime.MinValue,
                    LastWordChange = DateTime.MinValue,
                    Dictionary = DictionarySelection.JapanDict,
                    DrawPosition = DrawPosition.LeftTop,
                    ImageCycle = CycleInfo.Default,
                    ImageOption = ImageOptionType.Unchanged,
                    UserImagePath = string.Empty,
                    ImageIndex = 0,
                    WordIndex = 0,
                    TextOptions = TextOptions.Default,
                    UserPreferences = UserPreferences.Default,
                    WordCycle = CycleInfo.Default,
                    WordList = WordListSelection.MostFrequent1000,
                    UserWordListPath = string.Empty,
                    WordSelection = SelectionMode.InOrder,
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

            if (settings.UserPreferences == null)
                settings.UserPreferences = UserPreferences.Default;
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
                    if (Directory.Exists("System/Library/Desktop Pictures"))
                        return "System/Library/Desktop Pictures";
                    else if (Directory.Exists("/Library/Desktop Pictures"))
                        return "/Library/Desktop Pictures";
                    else
                        return Path.Combine(Util.AppRoot, "Resources/Content/Images", "macos_notfound");
                default:
                    return Path.Combine(Util.AppRoot, "Resources/Content/Images");
                    // return Path.Combine(Util.AppRoot, "Resources/Content/Images", type.ToString());
            }
        }

        public static string GetFilePath(WordListSelection list)
        {
            return Path.Combine(Util.AppRoot, "Resources/Content/WordLists", $"{list}.list");
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

        public UserPreferences UserPreferences { get; set; }

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

        public SelectionMode WordSelection { get; set; }

        public DictionarySelection Dictionary { get; set; }

        public CycleInfo ImageCycle { get; set; }

        public CycleInfo WordCycle { get; set; }

        public int ImageIndex { get; set; }

        public int WordIndex { get; set; }

        public float PrimaryTextScale { get; set; }

        public float SecondaryTextScale { get; set; }

        public DateTime LastImageChange { get; set; }

        public DateTime LastWordChange { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Uses current value found in `this.UserPreferences.ShowInDock`
        /// to determine wether or not the application will be shown on
        /// the dock
        /// </summary>
        public void ApplyShowInDockSetting()
        {
            if (this.UserPreferences.ShowInDock)
            {
                NSApplication.SharedApplication.ActivationPolicy = NSApplicationActivationPolicy.Regular;
            }
            else
            {
                NSApplication.SharedApplication.ActivationPolicy = NSApplicationActivationPolicy.Accessory;
            }
        }

        public void ApplyStartOnBootSetting()
        {
            try
            {
                if (this.UserPreferences.StartOnBoot)
                {
                    RemoveStartupItem().ContinueWith(result =>
                    {
                        AddStartupItem();
                    });
                }
                else
                {
                    RemoveStartupItem();
                }
            }
            catch
            {
                string title = "Cannot add Startup item!";
                string message = @"""System Events"" permission is required in order to add startup item";
                Util.Alert(title, message);

                this.UserPreferences.StartOnBoot = false;
            }
        }
        #endregion

        #region Private Helpers
        public Task AddStartupItem()
        {
            return Task.Run(() =>
            {
                var args = String.Format(@"-e 'tell application ""System Events"" to make login item at end with properties {{path:""{0}"", hidden:false}}'", @"/Applications/Rice Paper.app");
                Util.RunOSAScript(args);
            });
        }

        public Task RemoveStartupItem()
        {
            return Task.Run(() =>
            {
                var args = String.Format(@"-e 'tell application ""System Events"" to delete login item ""{0}""'", @"Rice Paper");
                Util.RunOSAScript(args);
            });
        }
        #endregion

        /// <summary>
        /// Marked private to avoid creating an instance
        /// without loading data
        /// </summary>
        private AppSettings() { }
    }
}
