using System;
using AppKit;
using RicePaper.Lib.Model;
using RicePaper.MacOS.ViewControllers;

namespace RicePaper.MacOS
{
    public partial class PreferencesController : TabController
    {
        #region Initialization
        public PreferencesController(IntPtr handle) : base(handle) { }

        public override void ViewWillLayout()
        {
            UpdateUI();

            base.ViewWillLayout();
        }
        #endregion

        #region Action Handlers
        partial void ActionOnBtnShowInDock(NSButton sender)
        {
            Settings.UserPreferences = GetUIModel();
            Settings.ApplyShowInDockSetting();
            SaveSettings();
        }

        partial void ActionOnBtnStartOnBoot(NSButton sender)
        {
            Settings.UserPreferences = GetUIModel();
            Settings.ApplyStartOnBootSetting();
            SaveSettings();
        }
        #endregion

        #region Private Helpers
        /// <summary>
        /// Reads the state of the UI as seen by the user
        /// </summary>
        /// <returns></returns>
        private UserPreferences GetUIModel()
        {
            return new UserPreferences()
            {
                ShowInDock = GetCheckboxValue(ButtonRefShowInDock),
                StartOnBoot = GetCheckboxValue(ButtonRefStartOnBoot)
            };
        }

        private void UpdateUI()
        {
            SetCheckboxValue(ButtonRefShowInDock, Settings.UserPreferences.ShowInDock);
            SetCheckboxValue(ButtonRefStartOnBoot, Settings.UserPreferences.StartOnBoot);
        }
        #endregion
    }
}
