using System;
using AppKit;
using Foundation;

namespace RicePaper.MacOS
{
    public partial class AboutController : NSViewController
    {
        #region Constants
        private const string _githubUrl = "https://github.com/felipemullen/RicePaper-public";
        #endregion

        #region Initialization
        public AboutController(IntPtr handle) : base(handle) { }

        public override void ViewWillLayout()
        {
            SetVersionString();

            base.ViewWillLayout();
        }
        #endregion

        #region Actions
        partial void OnActionGithubLink(NSButton _)
        {
            var url = new NSUrl(_githubUrl);
            NSWorkspace.SharedWorkspace.OpenUrl(url);
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
