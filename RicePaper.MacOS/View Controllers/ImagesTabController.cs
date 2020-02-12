using System;
using Foundation;
using AppKit;
using RicePaper.Lib;
using RicePaper.Lib.Model;
using System.Linq;
using RicePaper.MacOS.ViewControllers;

namespace RicePaper.MacOS
{
    public partial class ImagesTabController : TabController
    {
        #region Private Fields
        private DrawPosition _drawPosition;
        private ImageOptionType _imageOptionType;
        private string _imagePath;
        #endregion

        #region Initialization
        public ImagesTabController(IntPtr handle) : base(handle) { }

        public override void ViewWillLayout()
        {
            LoadVolatileValues();
            UpdateEntireUI();

            base.ViewWillLayout();
        }

        /// <summary>
        /// I couldn't think of a more appropriate name for this method after
        /// a good 10 seconds of pondering. The responsibility of this method
        /// is to set the values for variables used in this controller that
        /// come straight from AppSettings but need to be loaded before
        /// the UI is rendered
        /// </summary>
        private void LoadVolatileValues()
        {
            _imagePath = Settings.UserImagePath;
            _imageOptionType = Settings.ImageOption;
        }
        #endregion

        #region Action Handlers
        partial void ActionImageListDropdown(NSObject sender)
        {
            var choice = GetPopupButtonValue<ImageOptionType>(sender);

            if (_imageOptionType != choice)
            {
                _imageOptionType = choice;

                ButtonRefImagePicker.Enabled = (choice == ImageOptionType.Custom);

                UpdateLabels();
                SetDirty();
            }
        }

        partial void ActionSliderPrimary(NSObject sender) => SetDirty();
        partial void ActionSliderSecondary(NSObject sender) => SetDirty();

        partial void ActionPositionCB(NSObject sender) => UpdatePosition(sender, DrawPosition.CenterBottom);
        partial void ActionPositionCM(NSObject sender) => UpdatePosition(sender, DrawPosition.CenterMid);
        partial void ActionPositionCT(NSObject sender) => UpdatePosition(sender, DrawPosition.CenterTop);
        partial void ActionPositionLB(NSObject sender) => UpdatePosition(sender, DrawPosition.LeftBottom);
        partial void ActionPositionLM(NSObject sender) => UpdatePosition(sender, DrawPosition.LeftMid);
        partial void ActionPositionLT(NSObject sender) => UpdatePosition(sender, DrawPosition.LeftTop);
        partial void ActionPositionRB(NSObject sender) => UpdatePosition(sender, DrawPosition.RightBottom);
        partial void ActionPositionRM(NSObject sender) => UpdatePosition(sender, DrawPosition.RightMid);
        partial void ActionPositionRT(NSObject sender) => UpdatePosition(sender, DrawPosition.RightTop);

        partial void ActionImageDialog(NSButton sender)
        {
            var dialog = NSOpenPanel.OpenPanel;
            dialog.CanChooseFiles = false;
            dialog.CanChooseDirectories = true;
            dialog.Title = "Select Directory";
            dialog.ShowsResizeIndicator = true;
            dialog.ShowsHiddenFiles = false;
            dialog.AllowsMultipleSelection = false;

            dialog.BeginSheet(this.View.Window, (nint outcome) =>
            {
                if (outcome == 1)
                {
                    var result = dialog.Url;

                    if (result != null)
                    {
                        _imagePath = result.Path;
                        UpdateLabels();
                        SetDirty();
                    }
                }
            });
        }

        partial void ActionButtonApply(NSObject sender)
        {
            if (!ValidateSettings())
                return;

            bool imageFolderWasChanged = false;
            bool wordListWasChanged = false;

            if (Settings.ImageOption != _imageOptionType)
            {
                if (_imageOptionType == ImageOptionType.Custom)
                {
                    ImageList.Load(_imagePath);
                    Settings.UserImagePath = _imagePath;
                }
                else
                {
                    ImageList.Load(_imageOptionType);
                }

                Settings.ImageOption = GetPopupButtonValue<ImageOptionType>(DropdownRefImageList);
                imageFolderWasChanged = true;
            }

            else if (Settings.ImageOption == ImageOptionType.Custom && _imageOptionType == ImageOptionType.Custom && Settings.UserImagePath != _imagePath)
            {
                ImageList.Load(_imagePath);
                Settings.UserImagePath = _imagePath;
                imageFolderWasChanged = true;
            }

            Settings.DrawPosition = _drawPosition;
            Settings.PrimaryTextScale = SliderRefPrimaryText.FloatValue;
            Settings.SecondaryTextScale = SliderRefSecondaryText.FloatValue;

            SetClean();

            Scheduler.ForcedUpdate(changeImage: imageFolderWasChanged, changeWord: wordListWasChanged);

            try
            {
                AppSettings.Save(Settings);
            }
            catch (Exception) { }
        }
        #endregion

        #region Private Helpers
        private bool ValidateSettings()
        {
            if (_imageOptionType == ImageOptionType.Custom && string.IsNullOrWhiteSpace(_imagePath))
            {
                Util.Alert(
                    title: "Invalid Settings",
                    message: "Please specify a folder path for custom images",
                    window: this.View.Window
                );

                return false;
            }

            return true;
        }

        private void UpdateEntireUI()
        {
            ButtonRefImagePicker.Enabled = (Settings.ImageOption == ImageOptionType.Custom);

            ButtonRefPosCB.AlphaValue = 0.6f;
            ButtonRefPosCM.AlphaValue = 0.6f;
            ButtonRefPosCT.AlphaValue = 0.6f;
            ButtonRefPosLB.AlphaValue = 0.6f;
            ButtonRefPosLM.AlphaValue = 0.6f;
            ButtonRefPosLT.AlphaValue = 0.6f;
            ButtonRefPosRB.AlphaValue = 0.6f;
            ButtonRefPosRM.AlphaValue = 0.6f;
            ButtonRefPosRT.AlphaValue = 0.6f;

            // This is a dirty switch statement but oh well
            switch (Settings.DrawPosition)
            {
                case DrawPosition.LeftTop: ButtonRefPosLT.AlphaValue = 1; break;
                case DrawPosition.LeftMid: ButtonRefPosLM.AlphaValue = 1; break;
                case DrawPosition.LeftBottom: ButtonRefPosLB.AlphaValue = 1; break;
                case DrawPosition.CenterTop: ButtonRefPosCT.AlphaValue = 1; break;
                case DrawPosition.CenterMid: ButtonRefPosCM.AlphaValue = 1; break;
                case DrawPosition.CenterBottom: ButtonRefPosCB.AlphaValue = 1; break;
                case DrawPosition.RightTop: ButtonRefPosRT.AlphaValue = 1; break;
                case DrawPosition.RightMid: ButtonRefPosRM.AlphaValue = 1; break;
                case DrawPosition.RightBottom: ButtonRefPosRB.AlphaValue = 1; break;
            }

            DropdownRefImageList.SelectItem(DropdownRefImageList.Items()
                .Where(x => x.AccessibilityIdentifier == Settings.ImageOption.ToString()).FirstOrDefault()
            );

            SliderRefPrimaryText.FloatValue = Settings.PrimaryTextScale;
            SliderRefSecondaryText.FloatValue = Settings.SecondaryTextScale;

            UpdateLabels();
        }

        private void UpdatePosition(NSObject sender, DrawPosition newPosition)
        {
            ButtonRefPosLT.AlphaValue = 0.6f;
            ButtonRefPosLM.AlphaValue = 0.6f;
            ButtonRefPosLB.AlphaValue = 0.6f;
            ButtonRefPosCT.AlphaValue = 0.6f;
            ButtonRefPosCM.AlphaValue = 0.6f;
            ButtonRefPosCB.AlphaValue = 0.6f;
            ButtonRefPosRT.AlphaValue = 0.6f;
            ButtonRefPosRM.AlphaValue = 0.6f;
            ButtonRefPosRB.AlphaValue = 0.6f;

            var button = sender as NSButton;
            button.AlphaValue = 1.0f;

            _drawPosition = newPosition;
            SetDirty();
        }

        private void UpdateLabels()
        {
            if (_imageOptionType == ImageOptionType.Custom && string.IsNullOrWhiteSpace(_imagePath) != true)
                LabelImagePath.StringValue = _imagePath;
            else
                LabelImagePath.StringValue = $"Image Set: {DropdownRefImageList.Title}";
        }

        private void SetDirty() => ButtonRefApply.Enabled = true;
        private void SetClean() => ButtonRefApply.Enabled = false;
        #endregion
    }
}
