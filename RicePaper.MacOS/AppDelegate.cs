using System;
using AppKit;
using CoreGraphics;
using Foundation;
using RicePaper.Lib;
using RicePaper.Lib.Dictionary;
using RicePaper.Lib.Model;

namespace RicePaper.MacOS
{
    [Register("AppDelegate")]
    public class AppDelegate : NSApplicationDelegate
    {
        #region Static Constants
        private const float ICON_SIZE = 18.0f;
        #endregion

        #region Private Fields
        private readonly RiceDictionary riceDict;
        private readonly WallpaperList imageList;
        private readonly RiceScheduler riceScheduler;
        private readonly AppSettings settings;
        #endregion

        #region UI Related Fields
        private readonly NSStatusItem statusItem;
        private readonly NSPopover popoverView;
        private NSStoryboard mainStoryboard;
        private ViewController viewController;
        #endregion

        #region Constructor
        public AppDelegate()
        {
            settings = AppSettings.Load();

            riceDict = new RiceDictionary();
            imageList = new WallpaperList();
            riceScheduler = new RiceScheduler(settings, riceDict, imageList);

            riceDict.Load(settings.WordListPath, settings.WordIndex);
            imageList.Load(settings.ImagePath, settings.ImageIndex);

            popoverView = new NSPopover();

            statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Square);
            statusItem.Button.Target = this;
            statusItem.Button.Image = NSImage.ImageNamed("TrayIcon");
            statusItem.Button.Image.Size = new CGSize(ICON_SIZE, ICON_SIZE);
            statusItem.Button.Action = new ObjCRuntime.Selector("StatusBarClicked:");

            RegisterAppKitNotifications();
        }

        private void RegisterAppKitNotifications()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(NSApplication.DidChangeScreenParametersNotification, (n) =>
            {
                riceScheduler.ForcedUpdate(false, false);
            });
        }
        #endregion

        #region NS Lifecycle
        public override void DidFinishLaunching(NSNotification notification)
        {
            mainStoryboard = NSStoryboard.FromName("Main", null);

            viewController = mainStoryboard.InstantiateControllerWithIdentifier("ViewController") as ViewController;
            viewController.AppSettings = this.settings;
            viewController.RiceDictionary = this.riceDict;
            viewController.ImageList = this.imageList;
            viewController.RiceScheduler = riceScheduler;

            popoverView.ContentViewController = viewController;
            popoverView.Behavior = NSPopoverBehavior.Semitransient;

            riceScheduler.BeginScheduling();
        }

        public override void WillTerminate(NSNotification notification)
        {
            AppSettings.Save(settings);
        }

        [Export("applicationWillResignActive:")]
        public override void WillResignActive(NSNotification notification)
        {
            CloseMainWindow(notification);
        }
        #endregion

        #region Button Actions
        [Action("StatusBarClicked:")]
        public void StatusBarClicked(NSObject sender)
        {
            OpenMainWindow();
        }
        #endregion

        #region Private Helpers
        private void OpenMainWindow()
        {
            popoverView.Show(statusItem.Button.Bounds, statusItem.Button, NSRectEdge.MaxYEdge);
            NSRunningApplication.CurrentApplication.Activate(NSApplicationActivationOptions.ActivateIgnoringOtherWindows);
        }

        private void CloseMainWindow(NSNotification notification)
        {
            popoverView.PerformClose(notification);
        }
        #endregion
    }
}
