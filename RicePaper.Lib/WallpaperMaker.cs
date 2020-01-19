using RicePaper.Lib.Model;
using System;
using System.IO;
using SceneKit;
using CoreText;
using System.Runtime.Remoting.Messaging;

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

        private const int BLOCK_PADDING = 40;
        private const int LINE_SPACER = 20;
        private const int PARAGRAPH_PADDING = 10;
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
                for (int i = 0; i < NSScreen.Screens.Length; i++)
                {
                    var _screen = NSScreen.Screens[i];

                    string cachePath = DrawImage(i, _screen, filepath, drawDetails);
                    //NSWorkspace.SharedWorkspace.OpenFile(AppContext.BaseDirectory);

                    NSUrl url = NSUrl.FromFilename(cachePath);
                    NSError errorContainer = new NSError();

                    var workspace = NSWorkspace.SharedWorkspace;
                    var options = workspace.DesktopImageOptions(_screen);
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
        private string DrawImage(int iteration, NSScreen screen, string originalImagePath, DrawParameters drawDetails)
        {
            string outputPath = GetOutputPath(iteration, originalImagePath);

            var screenRect = screen.Frame;

            using (var pool = new NSAutoreleasePool())
            using (var colorSpace = CGColorSpace.CreateDeviceRGB())
            using (var bitmapContext = new CGBitmapContext(null, (nint)screenRect.Width, (nint)screenRect.Height, BITS_PER_COMPONENT, BYTES_PER_ROW, colorSpace, CGImageAlphaInfo.PremultipliedLast))
            using (var previousContext = NSGraphicsContext.CurrentContext)
            {
                NSGraphicsContext.CurrentContext = NSGraphicsContext.FromCGContext(bitmapContext, true);

                /// Draw: Begin
                {
                    // TODO: remove this
                    //bitmapContext.SetFillColor(NSColor.Red.CGColor);
                    //bitmapContext.FillRect(screenRect);

                    var originalImage = OpenAsNSImage(originalImagePath);
                    DrawImage(originalImage, screenRect);

                    var flipVertical = new CGAffineTransform(1, 0, 0, -1, 0, screenRect.Height);
                    bitmapContext.ConcatCTM(flipVertical);

                    DrawTextBlock(drawDetails, screenRect);
                }
                /// Draw: End

                var bmContextImage = bitmapContext.ToImage();
                WriteImageToFile(outputPath, bmContextImage);

                if (previousContext != null)
                    NSGraphicsContext.CurrentContext = previousContext;
            }

            return outputPath;
        }
        #endregion

        #region Private Helpers
        private void WriteImageToFile(string filePath, CGImage image)
        {
            var fileURL = NSUrl.FromFilename(filePath);
            var imageDestination = CGImageDestination.Create(fileURL, UTType.PNG, 1);
            imageDestination.AddImage(image);
            imageDestination.Close();
        }

        private void DrawTextBlock(DrawParameters drawParameters, CGRect bounds)
        {
            // TODO: Get font color from average RGB perception
            var textColor = NSColor.White;

            // TODO: Positioning shenanigans
            switch (drawParameters.Position)
            {
                case DrawPosition.CenterMid:
                    break;
            }

            var textDetails = drawParameters.Text;
            CGRect nextBounds = bounds;
            nextBounds.X = BLOCK_PADDING;
            nextBounds.Y = BLOCK_PADDING;
            if (string.IsNullOrWhiteSpace(textDetails.Kanji))
            {
                nextBounds = DrawText(textDetails.Furigana, FontParams.Heading, textColor, nextBounds);
            }
            else
            {
                nextBounds = DrawText(textDetails.Kanji, FontParams.Heading, textColor, nextBounds);

                var subheading = textDetails.Furigana;
                if (string.IsNullOrWhiteSpace(textDetails.Romaji) == false)
                    subheading = $"{subheading} ({textDetails.Romaji})";

                nextBounds = DrawText(subheading, FontParams.Label, textColor, nextBounds);
            }

            if (string.IsNullOrWhiteSpace(textDetails.Definition) == false)
            {
                nextBounds = OffsetBounds(new CGSize(0, LINE_SPACER), nextBounds);
                nextBounds = DrawText("Definition:", FontParams.Label, textColor, nextBounds);

                var lines = textDetails.Definition.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    nextBounds = DrawText(line, FontParams.Paragraph, textColor, nextBounds);
                }
            }

            if (string.IsNullOrWhiteSpace(textDetails.JapaneseSentence) == false)
            {
                nextBounds = OffsetBounds(new CGSize(0, LINE_SPACER), nextBounds);

                nextBounds = DrawText("Japanese sentence:", FontParams.Label, textColor, nextBounds);
                nextBounds = DrawText(textDetails.JapaneseSentence, FontParams.Paragraph, textColor, nextBounds);
            }

            if (string.IsNullOrWhiteSpace(textDetails.EnglishSentence) == false)
            {
                nextBounds = OffsetBounds(new CGSize(0, LINE_SPACER), nextBounds);

                nextBounds = DrawText("English sentence:", FontParams.Label, textColor, nextBounds);
                nextBounds = DrawText(textDetails.EnglishSentence, FontParams.Paragraph, textColor, nextBounds);
            }

        }

        private CGRect OffsetBounds(CGSize lastSize, CGRect lastBounds)
        {
            return new CGRect(
                lastBounds.X,
                lastBounds.Y + lastSize.Height,
                lastBounds.Width,
                lastBounds.Height - lastSize.Height
            );
        }

        private CGRect DrawText(string text, NSFont font, NSColor color, CGRect bounds)
        {
            var options = FontParams.GetFontAttrs(font, color);
            var nsString = new NSString(text);
            nsString.DrawInRect(bounds, options);

            if (font == FontParams.Paragraph)
                bounds.X += PARAGRAPH_PADDING;

            var textSize = CalculateTextSize(nsString, font);
            var nextBounds = OffsetBounds(textSize, bounds);
            return nextBounds;
        }

        private CGSize CalculateTextSize(NSString text, NSFont font)
        {
            var bounds = new CGSize();
            var attr = new NSStringAttributes() { Font = font };

            var frame = text.GetBoundingRect(
                bounds,
                NSStringDrawingOptions.UsesLineFragmentOrigin,
                attr,
                null);

            return frame.Size;
        }

        private static CGRect FitToBounds(CGRect image, CGRect bounds, AspectMode mode = AspectMode.Fill)
        {
            double widthScale = bounds.Width / image.Width;
            double heightScale = bounds.Height / image.Height;

            double scale = 1;
            switch (mode)
            {
                case AspectMode.Fit: scale = Math.Min(widthScale, heightScale); break;
                case AspectMode.Fill: scale = Math.Max(widthScale, heightScale); break;
            }

            var scaled = new CGRect(0, 0, image.Width * scale, image.Height * scale);
            scaled.X = (nfloat)((bounds.Width - scaled.Width) / 2.0);
            scaled.Y = (nfloat)((bounds.Height - scaled.Height) / 2.0);

            return scaled;
        }

        private void DrawImage(NSImage image, CGRect drawRect)
        {
            var sourceRect = new CGRect(CGPoint.Empty, image.Size);
            if (sourceRect.Width == 0 || sourceRect.Height == 0)
                return;

            var expanded = FitToBounds(sourceRect, drawRect);
            image.DrawInRect(expanded, sourceRect, NSCompositingOperation.SourceOver, 1);
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

        private string GetOutputPath(int iteration, string originalPath)
        {
            //string extension = new FileInfo(originalPath).Extension;
            string extension = "png";
            string fileName = $"rp_current_image_{iteration}.{extension}";
            return Path.Combine(CacheDirectory, fileName);
        }

        private nfloat GetAspect(CGRect rect)
        {
            return rect.Width / rect.Height;
        }

        private nfloat GetAspect(NSImage image)
        {
            return image.Size.Width / image.Size.Height;
        }
        #endregion
    }
}
