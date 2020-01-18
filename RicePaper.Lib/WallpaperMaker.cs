using RicePaper.Lib.Model;
using System;
using System.IO;

#if __MACOS__
using AppKit;
using CoreGraphics;
using Foundation;
using ImageIO;
using MobileCoreServices;
#endif

namespace RicePaper.Lib
{
    public class WallpaperMaker
    {
        #region Static Constants
        private const int BITS_PER_COMPONENT = 8;
        private const int BYTES_PER_ROW = 0;
        #endregion

        #region Properties
        public string CacheDirectory
        {
            get { return Path.Combine(AppContext.BaseDirectory, "cache"); }
        }
        #endregion

        #region Constructor
        public WallpaperMaker()
        {
            var cachePath = new DirectoryInfo(CacheDirectory);
            if (cachePath.Exists == false)
                cachePath.Create();
        }
        #endregion

        #region Public Methods
        public void SetWallpaper(string filepath, DrawParameters drawDetails)
        {
#if __MACOS__
            try
            {
                string cachePath = DrawImage(filepath, drawDetails);
                NSWorkspace.SharedWorkspace.OpenFile(AppContext.BaseDirectory);

                NSUrl url = NSUrl.FromFilename(cachePath);
                NSError errorContainer = new NSError();

                foreach (var _screen in NSScreen.Screens)
                {
                    var workspace = NSWorkspace.SharedWorkspace;
                    var options = workspace.DesktopImageOptions(_screen);
                    //options.Add(new NSString("NSWorkspaceDesktopImageScalingKey"), NSImageScaling.ProportionallyUpOrDown);
                    //options.Add(new NSString("NSWorkspaceDesktopImageAllowClippingKey"), );
                    workspace.SetDesktopImageUrl(url, _screen, options, errorContainer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
#endif
        }
        #endregion

        #region Drawing
        private string DrawImage(string originalImagePath, DrawParameters drawDetails)
        {
            string outputPath = GetOutputPath(originalImagePath);

            var originalImage = OpenAsNSImage(originalImagePath);
            var sizeRect = CGRect.FromLTRB(
                0,
                0,
                originalImage.Size.Width,
                originalImage.Size.Height
            );

            using (var pool = new NSAutoreleasePool())
            using (var colorSpace = CGColorSpace.CreateDeviceRGB())
            using (var bitmapContext = new CGBitmapContext(null, (nint)sizeRect.Width, (nint)sizeRect.Height, BITS_PER_COMPONENT, BYTES_PER_ROW, colorSpace, CGImageAlphaInfo.PremultipliedLast))
            using (var previousContext = NSGraphicsContext.CurrentContext)
            {
                var flipVertical = new CGAffineTransform(1, 0, 0, -1, 0, sizeRect.Height);
                bitmapContext.ConcatCTM(flipVertical);

                NSGraphicsContext.CurrentContext = NSGraphicsContext.FromCGContext(bitmapContext, true);

                // Draw calls to context
                {
                    DrawImage(originalImage, sizeRect);
                    DrawText(drawDetails.Text, sizeRect);
                }

                var bmContextImage = bitmapContext.ToImage();
                CreateImageFile(outputPath, bmContextImage);

                if (previousContext != null)
                    NSGraphicsContext.CurrentContext = previousContext;
            }

            return outputPath;
        }

        #endregion

        #region Private Helpers
        private void CreateImageFile(string filePath, CGImage image)
        {
            var fileURL = NSUrl.FromFilename(filePath);
            var imageDestination = CGImageDestination.Create(fileURL, UTType.PNG, 1);
            imageDestination.AddImage(image);
            imageDestination.Close();
        }

        private void DrawText(TextDetails text, CGRect sizeRect)
        {
            var sentenceString = new NSString(text.Sentence);
            var options = new NSMutableDictionary();
            var textColor = NSColor.White;
            var textFont = NSFont.FromFontName("Helvetica Bold", 100);
            options.Add(NSStringAttributeKey.Font, textFont);
            options.Add(NSStringAttributeKey.ForegroundColor, textColor);
            sentenceString.DrawInRect(sizeRect, options);
        }

        private void DrawImage(NSImage image, CGRect rect)
        {
            image.DrawInRect(rect, rect, NSCompositingOperation.Copy, 1);
        }

        private NSImage OpenAsNSImage(string filePath)
        {
            NSImage img;
            using (var stream = File.OpenRead(filePath))
            {
                img = NSImage.FromStream(stream);
            }

            return img;
        }

        private string GetOutputPath(string originalPath)
        {
            //string extension = new FileInfo(originalPath).Extension;
            string extension = "png";
            string fileName = $"rp_current_image.{extension}";
            return Path.Combine(CacheDirectory, fileName);
        }
        #endregion
    }
}
