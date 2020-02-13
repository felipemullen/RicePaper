using System;
using AppKit;
using Foundation;

namespace RicePaper.MacOS
{
    public partial class AboutController : NSViewController
    {
        #region Initialization
        public AboutController(IntPtr handle) : base(handle) { }

        public override void ViewWillLayout()
        {
            SetVersionString();

            base.ViewWillLayout();
        }
        #endregion

        #region Private Helpers
        private void SetVersionString()
        {
            var version = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"];
            FieldRefVersion.StringValue = (version != null)
                ? $"version {version}"
                : "";
        }
        #endregion
    }
}
