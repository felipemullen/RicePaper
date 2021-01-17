using System;
using System.Threading.Tasks;
using AppKit;
using Foundation;
using RicePaper.Lib;
using RicePaper.Lib.Dictionary;
using RicePaper.Lib.Model;

namespace RicePaper.MacOS.ViewControllers
{
    public class TabController : NSViewController
    {
        #region Properties
        public static AppDelegate App
        {
            get { return (AppDelegate)NSApplication.SharedApplication.Delegate; }
        }
        #endregion

        #region Private Fields
        public readonly AppSettings Settings;
        public readonly RiceScheduler Scheduler;
        public readonly WallpaperList ImageList;
        public readonly RiceDictionary RiceDictionary;
        #endregion

        #region Initialization
        public TabController(IntPtr handle) : base(handle)
        {
            Settings = App.Settings;
            Scheduler = App.Scheduler;
            ImageList = App.ImageList;
            RiceDictionary = App.RiceDict;
        }
        #endregion

        #region Inherited Methods
        protected int GetUnitValue(NSObject sender)
        {
            var field = sender as NSTextField;
            int numberValue = int.Parse(field.StringValue);

            return numberValue;
        }

        /// <summary>
        /// This method relies on the "Accessibility Identifier" being
        /// set in XCode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <returns></returns>
        protected T GetPopupButtonValue<T>(NSObject sender) where T : struct
        {
            var dropdown = sender as NSPopUpButton;
            var choiceString = dropdown.SelectedItem.AccessibilityIdentifier;
            T choice = Enum.Parse<T>(choiceString);

            return choice;
        }

        protected bool GetCheckboxValue(NSObject sender)
        {
            var checkbox = sender as NSButton;
            return (checkbox.StringValue == "0") ? false : true;
        }

        protected void SetCheckboxValue(NSObject sender, bool value)
        {
            var checkbox = sender as NSButton;
            checkbox.State = (value) ? NSCellStateValue.On : NSCellStateValue.Off;
        }

        protected void UpdateImage(bool changeImage = false, bool changeWord = false)
        {
            Scheduler.ForcedUpdate(changeImage, changeWord);
            SaveSettings();
        }

        protected void SaveSettings()
        {
            try
            {
                AppSettings.Save(Settings);
            }
            catch (Exception) { }
        }
        #endregion
    }
}
