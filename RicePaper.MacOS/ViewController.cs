using System;
using System.Drawing;
using System.Security.Policy;
using AppKit;
using Foundation;

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
        #endregion

        #region Constructor
        public ViewController(IntPtr handle) : base(handle) { }
        #endregion

        #region NS Lifecycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //FilePathLabel.StringValue = "TO BE DETERMINED";
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
                        FilePathLabel.StringValue = path;
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
        #endregion
    }
}
