using AppKit;
using Foundation;

namespace RicePaper.MacOS
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        #region Properties
        private readonly NSStatusItem statusItem;
        #endregion

        #region Constructor
        public AppDelegate()
        {
            statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Variable);
        }
        #endregion

        #region NS Lifecycle
        public override void DidFinishLaunching(NSNotification notification)
        {
            statusItem.Title = "Ω";
            statusItem.Button.Target = this;
            //statusItem.Button.Image = NSImage.ImageNamed("");
            statusItem.Button.Action = new ObjCRuntime.Selector("StatusBarClicked:");
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
        #endregion

        #region Actions
        [Action("StatusBarClicked:")]
        public void StatusBarClicked(NSObject sender)
        {
            try
            {
                var storyboard = NSStoryboard.FromName("Main", null);
                var controller = storyboard.InstantiateControllerWithIdentifier("ViewController") as ViewController;

                var popoverView = new NSPopover();
                popoverView.ContentViewController = controller;
                popoverView.Behavior = NSPopoverBehavior.Transient;
                popoverView.Show(statusItem.Button.Bounds, statusItem.Button, NSRectEdge.MaxYEdge);

            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }
        #endregion
    }
}
