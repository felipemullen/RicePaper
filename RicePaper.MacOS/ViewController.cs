using System;
using AppKit;
using Foundation;
using RicePaper.Lib;
using RicePaper.Lib.Model;

namespace RicePaper.MacOS
{
    public partial class ViewController : NSViewController
    {
        #region Properties
        public override NSObject RepresentedObject
        {
            get { return base.RepresentedObject; }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        public AppSettings AppSettings { get; set; }
        public RiceScheduler RiceScheduler { get; internal set; }
        #endregion

        #region Constructor
        public ViewController(IntPtr handle) : base(handle) { }
        #endregion

        #region NS Lifecycle
        public override void ViewWillLayout()
        {
            base.ViewWillLayout();
            SetLabels();
        }
        #endregion

        #region Action Handlers
        partial void FileDialogButton(NSButton sender)
        {
            try
            {
                var dialog = NSOpenPanel.OpenPanel;
                dialog.CanChooseFiles = false;
                dialog.CanChooseDirectories = true;
                dialog.Title = "Select directory";
                dialog.ShowsResizeIndicator = true;
                dialog.ShowsHiddenFiles = false;
                dialog.AllowsMultipleSelection = false;

                var outcome = dialog.RunModal();
                if (outcome == 1)
                {
                    var result = dialog.Url;

                    if (result != null)
                    {
                        var path = result.Path;
                        LabelImagePath.StringValue = path;
                    }
                }
                else
                {
                    // was canceled
                    return;
                }
            }
            catch (Exception) { }
        }

        partial void ButtonNextWord(NSObject sender)
        {
            RiceScheduler.Update(changeImage: false, changeWord: true);
            //AppSettings.State.WordIndex.Next();

        }
        #endregion

        #region Private Helpers
        private void UpdateSettings()
        {
        }

        private void Redraw()
        {

        }

        private void SetLabels()
        {
            LabelImagePath.StringValue = AppSettings.ImagePathLabel;
            LabelWordListPath.StringValue = AppSettings.WordListPathLabel;
        }
        #endregion
    }
}
