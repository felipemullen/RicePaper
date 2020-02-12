using System;
using AppKit;
using Foundation;

namespace RicePaper.MacOS
{
    [Register("ReplaceViewSegue")]
    public class ReplaceViewSegue : NSStoryboardSegue
    {
        #region Constructors
        public ReplaceViewSegue() { }

        public ReplaceViewSegue(string identifier, NSObject sourceController, NSObject destinationController) : base(identifier, sourceController, destinationController) { }

        public ReplaceViewSegue(IntPtr handle) : base(handle) { }

        public ReplaceViewSegue(NSObjectFlag x) : base(x) { }
        #endregion

        #region Override Methods
        public override void Perform()
        {
            // Cast the source and destination controllers
            var source = SourceController as NSViewController;
            var destination = DestinationController as NSViewController;

            // Is there a source?
            if (source == null)
            {
                // No, get the current key window
                var window = NSApplication.SharedApplication.KeyWindow;

                // Swap the controllers
                window.ContentViewController = destination;

                // Release memory
                window.ContentViewController?.RemoveFromParentViewController();
            }
            else
            {
                // Swap the controllers
                source.View.Window.ContentViewController = destination;

                // Release memory
                source.RemoveFromParentViewController();
            }

        }
        #endregion
    }
}