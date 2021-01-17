using System;
using AppKit;
using Foundation;
using RicePaper.Lib;

namespace RicePaper.MacOS
{
    public partial class AboutController : NSViewController
    {
        #region Constants
        private const string _githubUrl = "https://github.com/felipemullen/RicePaper-public";
        private const string _donateUrl = "https://www.paypal.com/paypalme/felipemullen";
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
            Util.OpenUrl(_githubUrl);
        }

        partial void OnActionDonateLink(NSButton sender)
        {
            Util.OpenUrl(_donateUrl);
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
