using System;
using Foundation;
using AppKit;
using RicePaper.Lib;
using System.Linq;
using RicePaper.MacOS.ViewControllers;

namespace RicePaper.MacOS
{
    public partial class ImagesTabController : TabController
    {
        #region Initialization
        public ImagesTabController(IntPtr handle) : base(handle) { }

        public override void ViewWillLayout()
        {
            UpdateUI();

            base.ViewWillLayout();
        }
        #endregion

        #region Action Handlers
        partial void ActionImageListDropdown(NSObject sender)
        {
            var choice = GetPopupButtonValue<ImageOptionType>(sender);

            if (Settings.ImageOption != choice)
            {
                if (choice == ImageOptionType.Custom)
                {
                    // Don't update unless there is already a custom path stored
                    if (string.IsNullOrWhiteSpace(Settings.UserImagePath) == false)
                    {
                        ImageList.Load(Settings.UserImagePath);
                        UpdateImage();
                    }

                    ButtonRefImagePicker.Enabled = true;
                }
                else if (choice == ImageOptionType.Unchanged)
                {
                    ButtonRefImagePicker.Enabled = false;
                    UpdateImage();
                }
                else
                {
                    ButtonRefImagePicker.Enabled = false;
                    ImageList.Load(choice);
                    UpdateImage();
                }

                Settings.ImageOption = choice;
                UpdateLabels();
                App.RefreshImageMenuText();
            }
        }

        partial void ActionSliderPrimary(NSObject sender) => ChangeScale();
        partial void ActionSliderSecondary(NSObject sender) => ChangeScale();

        partial void ActionPositionCB(NSObject sender) => ChangePosition(sender, DrawPosition.CenterBottom);
        partial void ActionPositionCM(NSObject sender) => ChangePosition(sender, DrawPosition.CenterMid);
        partial void ActionPositionCT(NSObject sender) => ChangePosition(sender, DrawPosition.CenterTop);
        partial void ActionPositionLB(NSObject sender) => ChangePosition(sender, DrawPosition.LeftBottom);
        partial void ActionPositionLM(NSObject sender) => ChangePosition(sender, DrawPosition.LeftMid);
        partial void ActionPositionLT(NSObject sender) => ChangePosition(sender, DrawPosition.LeftTop);
        partial void ActionPositionRB(NSObject sender) => ChangePosition(sender, DrawPosition.RightBottom);
        partial void ActionPositionRM(NSObject sender) => ChangePosition(sender, DrawPosition.RightMid);
        partial void ActionPositionRT(NSObject sender) => ChangePosition(sender, DrawPosition.RightTop);

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
                        Settings.UserImagePath = result.Path;
                        ImageList.Load(result.Path);

                        UpdateLabels();
                        UpdateImage();
                    }
                }
            });
        }
        #endregion

        #region Private Helpers
        private void UpdateUI()
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

        private void ChangePosition(NSObject sender, DrawPosition newPosition)
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

            Settings.DrawPosition = newPosition;

            UpdateImage();
        }

        private void ChangeScale()
        {
            Settings.PrimaryTextScale = SliderRefPrimaryText.FloatValue;
            Settings.SecondaryTextScale = SliderRefSecondaryText.FloatValue;
            UpdateImage();
            SaveSettings();
        }

        private void UpdateLabels()
        {
            if (Settings.ImageOption == ImageOptionType.Custom && string.IsNullOrWhiteSpace(Settings.UserImagePath) != true)
                LabelImagePath.StringValue = Settings.UserImagePath;
            else
                LabelImagePath.StringValue = $"Image Set: {DropdownRefImageList.Title}";
        }
        #endregion
    }
}
