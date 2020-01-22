﻿using System;
using AppKit;
using CoreGraphics;
using Foundation;
using RicePaper.Lib;
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
            riceScheduler = new RiceScheduler(settings);

            popoverView = new NSPopover();

            statusItem = NSStatusBar.SystemStatusBar.CreateStatusItem(NSStatusItemLength.Square);
            statusItem.Button.Target = this;
            statusItem.Button.Image = NSImage.ImageNamed("TrayIcon");
            statusItem.Button.Image.Size = new CGSize(ICON_SIZE, ICON_SIZE);
            statusItem.Button.Action = new ObjCRuntime.Selector("StatusBarClicked:");
        }
        #endregion

        #region NS Lifecycle
        public override void DidFinishLaunching(NSNotification notification)
        {
            mainStoryboard = NSStoryboard.FromName("Main", null);

            viewController = mainStoryboard.InstantiateControllerWithIdentifier("ViewController") as ViewController;
            viewController.AppSettings = this.settings;
            viewController.RiceScheduler = riceScheduler;

            popoverView.ContentViewController = viewController;
            popoverView.Behavior = NSPopoverBehavior.Semitransient;

            // TODO: implement logging

            // TODO: remove after implementation
            //OpenMainWindow();

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
