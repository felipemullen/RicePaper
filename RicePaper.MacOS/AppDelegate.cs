using System;
using System.Threading.Tasks;
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
        private const string HELP_URL = "https://github.com/felipemullen/RicePaper-public/blob/master/help.md";
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
        private NSMenuItem imageMenuItem;
        private NSMenuItem wordMenuItem;
        #endregion

        #region Constructor
        public AppDelegate()
        {
            Settings = AppSettings.Load();
            DesktopBackup.BackupDesktops();

            RiceDict = new RiceDictionary();
            ImageList = new WallpaperList();
            Scheduler = new RiceScheduler(Settings, RiceDict, ImageList);

            RiceDict.Load(Settings.WordListPath, Settings.WordIndex);

            if (Settings.ImageOption != ImageOptionType.Unchanged)
                ImageList.Load(Settings.ImagePath, Settings.ImageIndex);

            statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Square);
            statusItem.Button.Target = this;
            statusItem.Button.Image = NSImage.ImageNamed("TrayIcon");
            statusItem.Button.Image.Size = new CGSize(ICON_SIZE, ICON_SIZE);

            statusItem.Menu = CreateStatusBarMenu();

            RegisterAppKitNotifications();

            //NSUserDefaults.StandardUserDefaults.SetValueForKey("NSApplicationCrashOnExceptions", "true");
        }

        private NSMenu CreateStatusBarMenu()
        {
            var menu = new NSMenu();

            imageMenuItem = new NSMenuItem("", new ObjCRuntime.Selector("MenuNextImage:"), "");
            RefreshImageMenuText();
            menu.AddItem(imageMenuItem);

            wordMenuItem = new NSMenuItem("Next Word", new ObjCRuntime.Selector("MenuNextWord:"), "");
            menu.AddItem(wordMenuItem);

            menu.AddItem(NSMenuItem.SeparatorItem);
            menu.AddItem("Add word to blacklist", new ObjCRuntime.Selector("MenuAddToBlacklist:"), "");
            menu.AddItem("View blacklist", new ObjCRuntime.Selector("MenuViewBlacklist:"), "");

            menu.AddItem(NSMenuItem.SeparatorItem);
            menu.AddItem("Settings", new ObjCRuntime.Selector("MenuSettings:"), "");
            menu.AddItem(NSMenuItem.SeparatorItem);
            menu.AddItem("About RicePaper", new ObjCRuntime.Selector("MenuAbout:"), "");
            menu.AddItem("Help", new ObjCRuntime.Selector("MenuHelp:"), "");
            menu.AddItem("Quit", new ObjCRuntime.Selector("MenuQuit:"), "");

            return menu;
        }

        private void RegisterAppKitNotifications()
        {
            NSNotificationCenter.DefaultCenter.AddObserver(NSApplication.DidChangeScreenParametersNotification, (n) =>
            {
                DesktopBackup.BackupDesktops();
                Scheduler.ForcedUpdate(false, false);
            });

            NSWorkspace.SharedWorkspace.NotificationCenter.AddObserver(NSWorkspace.ActiveSpaceDidChangeNotification, (n) =>
            {
                Scheduler.ForcedUpdate(false, false);
            });
        }

        public void RefreshImageMenuText()
        {
            string imageText = (Settings.ImageOption == ImageOptionType.Unchanged)
                ? "Refresh Image"
                : "Next Image";

            imageMenuItem.Title = imageText;
        }
        #endregion

        #region NS Lifecycle
        public override void DidFinishLaunching(NSNotification notification)
        {
            mainStoryboard = NSStoryboard.FromName("Main", null);

            aboutWindow = mainStoryboard.InstantiateControllerWithIdentifier("AboutWindow") as NSWindowController;
            settingsWindow = mainStoryboard.InstantiateControllerWithIdentifier("SettingsWindow") as NSWindowController;

            Settings.ApplyShowInDockSetting();
            Settings.ApplyStartOnBootSetting();
            Task.Run(Scheduler.BeginScheduling);
        }

        public override void WillTerminate(NSNotification notification)
        {
            AppSettings.Save(Settings);
            DesktopBackup.RestoreDesktops();
        }
        #endregion

        #region Button Actions
        [Action("MenuNextImage:")]
        public void NextImage(NSObject sender)
        {
            string originalText = imageMenuItem.Title;
            imageMenuItem.Title = string.Concat(originalText, " (...)");

            Scheduler.ForcedUpdate(changeImage: true, changeWord: false).ContinueWith(result =>
            {
                imageMenuItem.Title = originalText;
            });
        }

        [Action("MenuNextWord:")]
        public void NextWord(NSObject sender)
        {
            string originalText = wordMenuItem.Title;
            wordMenuItem.Title = string.Concat(originalText, " (...)");

            Scheduler.ForcedUpdate(changeImage: false, changeWord: true).ContinueWith(result =>
            {
                wordMenuItem.Title = originalText;
            });
        }

        [Action("MenuAddToBlacklist:")]
        public void AddToBlacklist(NSObject sender)
        {
            RiceDict.AddToBlacklist(RiceDict.CurrentItem);
            Scheduler.ForcedUpdate(false, true);
        }

        [Action("MenuViewBlacklist:")]
        public void ViewBlacklist(NSObject sender)
        {
            RiceDict.OpenBlacklistInEditor();
        }

        [Action("MenuHelp:")]
        public void Help(NSObject sender)
        {
            Util.OpenUrl(HELP_URL);
        }

        [Action("MenuQuit:")]
        public void Quit(NSObject sender)
        {
            AppSettings.Save(Settings);
            DesktopBackup.RestoreDesktops();
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

            NSApplication.SharedApplication.ActivateIgnoringOtherApps(true);
            controller.Window.Center();
            controller.Window.MakeKeyAndOrderFront(this);
        }
        #endregion
    }
}
