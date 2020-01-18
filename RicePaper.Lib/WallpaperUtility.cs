using System;

#if __MACOS__
using AppKit;
using Foundation;
#endif


namespace RicePaper.Lib
{
    public class WallpaperUtility
    {
        public WallpaperUtility()
        {
        }

        public void SetWallpaper(string filepath)
        {
#if __MACOS__
            var workspace = NSWorkspace.SharedWorkspace;
            var screen = NSScreen.MainScreen;

            try
            {
                NSUrl url = NSUrl.FromFilename(filepath);


                NSDictionary options = new NSDictionary();
                NSError errorContainer = new NSError();
                workspace.SetDesktopImageUrl(url, NSScreen.MainScreen, options, errorContainer);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
#endif
        }
    }
}
