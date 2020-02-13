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
        public readonly RiceDictionary RiceDict;
        public readonly WallpaperList ImageList;
        public readonly RiceScheduler Scheduler;
        public readonly AppSettings Settings;
        #endregion

        #region UI Related Fields
        private readonly NSStatusItem statusItem;
        private NSStoryboard mainStoryboard;

        private NSWindowController aboutWindow;
        private NSWindowController settingsWindow;
        #endregion

        #region Constructor
        public AppDelegate()
        {
            NSApplication.SharedApplication.ActivateIgnoringOtherApps(true);

            Settings = AppSettings.Load();

            RiceDict = new RiceDictionary();
            ImageList = new WallpaperList();
            Scheduler = new RiceScheduler(Settings, RiceDict, ImageList);

            RiceDict.Load(Settings.WordListPath, Settings.WordIndex);
            ImageList.Load(Settings.ImagePath, Settings.ImageIndex);

            statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Square);
            statusItem.Button.Target = this;
            statusItem.Button.Image = NSImage.ImageNamed("TrayIcon");
            statusItem.Button.Image.Size = new CGSize(ICON_SIZE, ICON_SIZE);

            statusItem.Menu = CreateStatusBarMenu();

            RegisterAppKitNotifications();
        }

        private NSMenu CreateStatusBarMenu()
        {
            var menu = new NSMenu();

            menu.AddItem("Next Image", new ObjCRuntime.Selector("MenuNextImage:"), "");
            menu.AddItem("Next Word", new ObjCRuntime.Selector("MenuNextWord:"), "");
            menu.AddItem(NSMenuItem.SeparatorItem);
            menu.AddItem("Settings", new ObjCRuntime.Selector("MenuSettings:"), "");
            menu.AddItem(NSMenuItem.SeparatorItem);
            menu.AddItem("About RicePaper", new ObjCRuntime.Selector("MenuAbout:"), "");
            menu.AddItem("Quit", new ObjCRuntime.Selector("MenuQuit:"), "");

            return menu;
        }

        private void RegisterAppKitNotifications()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(NSApplication.DidChangeScreenParametersNotification, (n) =>
            {
                Scheduler.ForcedUpdate(false, false);
            });
        }
        #endregion

        #region NS Lifecycle
        public override void DidFinishLaunching(NSNotification notification)
        {
            mainStoryboard = NSStoryboard.FromName("Main", null);

            aboutWindow = mainStoryboard.InstantiateControllerWithIdentifier("AboutWindow") as NSWindowController;
            settingsWindow = mainStoryboard.InstantiateControllerWithIdentifier("SettingsWindow") as NSWindowController;

            Scheduler.BeginScheduling();
        }

        public override void WillTerminate(NSNotification notification)
        {
            AppSettings.Save(Settings);
            // TODO: Restore original user wallpaper
        }
        #endregion

        #region Button Actions
        [Action("MenuNextImage:")]
        public void NextImage(NSObject sender)
        {
            Scheduler.ForcedUpdate(changeImage: true, changeWord: false);
        }

        [Action("MenuNextWord:")]
        public void NextWord(NSObject sender)
        {
            Scheduler.ForcedUpdate(changeImage: false, changeWord: true);
        }

        [Action("MenuQuit:")]
        public void Quit(NSObject sender)
        {
            System.Environment.Exit(0);
        }

        [Action("MenuSettings:")]
        public void OpenSettings(NSObject sender)
        {
            FocusWindow(settingsWindow);
        }

        [Action("MenuAbout:")]
        public void OpenAbout(NSObject sender)
        {
            FocusWindow(aboutWindow);
        }
        #endregion

        #region Private Helpers
        private void FocusWindow(NSWindowController controller)
        {
            if (controller.Window.IsVisible == false)
            {
                controller.ShowWindow(this);
            }

            controller.Window.Center();
            controller.Window.MakeKeyAndOrderFront(this);
        }
        #endregion
    }
}
