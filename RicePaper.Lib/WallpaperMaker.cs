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
        private const int CHANNELS = 4;

        private const int LINE_SPACER = 20;

        private const int SHADOW_BLUR = 1;
        private CGColor SHADOW_COLOR = new CGColor(0, 0, 0, 0.7f);
        private static CGSize SHADOW_OFFSET = new CGSize(2, -2);
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
                foreach (var _screen in NSScreen.Screens)
                {
                    string cachePath = DrawImage(_screen, filepath, drawDetails);

                    if (cachePath != null)
                    {
                        NSUrl url = NSUrl.FromFilename(cachePath);
                        NSError errorContainer = new NSError();

                        var workspace = NSWorkspace.SharedWorkspace;
                        var options = workspace.DesktopImageOptions(_screen);
                        var result = workspace.SetDesktopImageUrl(url, _screen, options, errorContainer);

                        if (result)
                            Console.WriteLine($"successfully set {cachePath}");
                        else
                            Console.WriteLine(errorContainer.ToString());
                    }

                    CleanupCache();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
#endif
        }

        private void CleanupCache()
        {
            var fiveMinutesAgo = DateTime.Now.AddMinutes(-5);

            var cacheDirectory = new DirectoryInfo(CacheDirectory);
            foreach (var file in cacheDirectory.EnumerateFiles())
            {
                if (file.CreationTime < fiveMinutesAgo)
                    file.Delete();
            }
        }
        #endregion

        #region Drawing
        private string DrawImage(NSScreen screen, string originalImagePath, DrawParameters drawDetails)
        {
            string outputPath = GetNewImagePath();

            var screenRect = screen.Frame;
            byte[] rawData = new byte[(int)screenRect.Width * (int)screenRect.Height * CHANNELS];
            int BYTES_PER_ROW = CHANNELS * (int)screenRect.Width;

            using (var pool = new NSAutoreleasePool())
            using (var colorSpace = CGColorSpace.CreateDeviceRGB())
            using (var bitmapContext = new CGBitmapContext(rawData, (nint)screenRect.Width, (nint)screenRect.Height, BITS_PER_COMPONENT, BYTES_PER_ROW, colorSpace, CGBitmapFlags.PremultipliedLast | CGBitmapFlags.ByteOrder32Big))
            using (var previousContext = NSGraphicsContext.CurrentContext)
            {
                NSGraphicsContext.CurrentContext = NSGraphicsContext.FromCGContext(bitmapContext, true);

                /// Draw: Begin
                {
                    var originalImage = OpenAsNSImage(originalImagePath);
                    DrawImage(originalImage, screenRect);

                    var flipVertical = new CGAffineTransform(1, 0, 0, -1, 0, screenRect.Height);
                    bitmapContext.ConcatCTM(flipVertical);

                    var pixelAverage = Util.GetAverageColor(rawData, (int)screenRect.Width, (int)screenRect.Height);
                    var color = Util.ContrastColor(pixelAverage);

                    drawDetails.TextColor = NSColor.FromCGColor(color);
                    bitmapContext.SetShadow(SHADOW_OFFSET, SHADOW_BLUR, SHADOW_COLOR);
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

        #region Text Measurement / Drawing
        private ExpansionBox MeasureTextBlock(DrawParameters drawParameters)
        {
            var _H = FontParams.Heading(drawParameters.TextColor, drawParameters.PrimaryTextScale);
            var _L = FontParams.Label(drawParameters.TextColor, drawParameters.SecondaryTextScale);
            var _P = FontParams.Paragraph(drawParameters.TextColor, drawParameters.SecondaryTextScale);

            var box = new ExpansionBox();
            var block = drawParameters.Text;

            if (string.IsNullOrWhiteSpace(block.Kanji))
            {
                var textSize = CalculateTextSize(_H(block.Furigana));
                box.AddHeight(textSize.Height).MaxWidth(textSize.Width);
            }
            else
            {
                var headingSize = CalculateTextSize(_H(block.Kanji));

                var subheadingText = block.Furigana;
                if (string.IsNullOrWhiteSpace(block.Romaji) == false)
                    subheadingText = $"{subheadingText} ({block.Romaji})";

                var subHeadingSize = CalculateTextSize(_L(subheadingText));

                box.AddHeight(headingSize.Height, subHeadingSize.Height).MaxWidth(headingSize.Width, subHeadingSize.Width);
            }

            if (string.IsNullOrWhiteSpace(block.Definition) == false)
            {
                var labelSize = CalculateTextSize(_L("Definition:"));

                box.AddHeight(LINE_SPACER, labelSize.Height).MaxWidth(labelSize.Width);

                var lines = block.Definition.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var lineSize = CalculateTextSize(_P(line));

                    box.AddHeight(lineSize.Height).MaxWidth(lineSize.Width);
                }

            }

            if (string.IsNullOrWhiteSpace(block.JapaneseSentence) == false)
            {
                var labelSize = CalculateTextSize(_L("Japanese sentence:"));
                var sentenceSize = CalculateTextSize(_P(block.JapaneseSentence));

                box.AddHeight(LINE_SPACER, labelSize.Height, sentenceSize.Height).MaxWidth(labelSize.Width, sentenceSize.Width);
            }

            if (string.IsNullOrWhiteSpace(block.EnglishSentence) == false)
            {
                var labelSize = CalculateTextSize(_L("English sentence:"));
                var sentenceSize = CalculateTextSize(_P(block.EnglishSentence));

                box.AddHeight(LINE_SPACER, labelSize.Height, sentenceSize.Height).MaxWidth(labelSize.Width, sentenceSize.Width);
            }

            return box;
        }

        private CGRect DrawTextBlock(DrawParameters drawParameters, CGRect screenBounds)
        {
            var _H = FontParams.Heading(drawParameters.TextColor, drawParameters.PrimaryTextScale);
            var _L = FontParams.Label(drawParameters.TextColor, drawParameters.SecondaryTextScale);
            var _P = FontParams.Paragraph(drawParameters.TextColor, drawParameters.SecondaryTextScale);

            double totalWidth = 0;
            double totalHeight = 0;

            CGRect textBounds = GetScreenSafeBounds(drawParameters, screenBounds);
            var block = drawParameters.Text;

            if (string.IsNullOrWhiteSpace(block.Kanji))
            {
                var heading = _H(block.Furigana);
                var textSize = DrawText(heading, textBounds);
                textBounds = OffsetBounds(textSize, textBounds);

                totalWidth = Math.Max(totalWidth, textSize.Width);
                totalHeight += textSize.Height;
            }
            else
            {
                var heading = _H(block.Kanji);
                var headingSize = DrawText(heading, textBounds);
                textBounds = OffsetBounds(headingSize, textBounds);

                var subheadingText = block.Furigana;
                if (string.IsNullOrWhiteSpace(block.Romaji) == false)
                    subheadingText = $"{subheadingText} ({block.Romaji})";

                var subheading = _L(subheadingText);
                var subHeadingSize = DrawText(subheading, textBounds);
                textBounds = OffsetBounds(subHeadingSize, textBounds);

                double largeWidth = Math.Max(headingSize.Width, subHeadingSize.Width);
                totalWidth = Math.Max(totalWidth, largeWidth);
                totalHeight += headingSize.Height + subHeadingSize.Height;
            }

            if (string.IsNullOrWhiteSpace(block.Definition) == false)
            {
                textBounds = OffsetBounds(new CGSize(0, LINE_SPACER), textBounds);
                var definitionLabel = _L("Definition:");
                var labelSize = DrawText(definitionLabel, textBounds);
                textBounds = OffsetBounds(labelSize, textBounds);

                totalWidth = Math.Max(totalWidth, labelSize.Width);
                totalHeight += LINE_SPACER + labelSize.Height;

                var lines = block.Definition.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var definitionLine = _P(line);
                    var lineSize = DrawText(definitionLine, textBounds);
                    textBounds = OffsetBounds(lineSize, textBounds);

                    totalWidth = Math.Max(totalWidth, lineSize.Width);
                    totalHeight += lineSize.Height;
                }

            }

            if (string.IsNullOrWhiteSpace(block.JapaneseSentence) == false)
            {
                textBounds = OffsetBounds(new CGSize(0, LINE_SPACER), textBounds);

                var japLabel = _L("Japanese sentence:");
                var labelSize = DrawText(japLabel, textBounds);
                textBounds = OffsetBounds(labelSize, textBounds);

                var japSentence = _P(block.JapaneseSentence);
                var sentenceSize = DrawText(japSentence, textBounds);
                textBounds = OffsetBounds(sentenceSize, textBounds);

                var largeWidth = Math.Max(labelSize.Width, sentenceSize.Width);
                totalWidth = Math.Max(totalWidth, largeWidth);
                totalHeight += LINE_SPACER + labelSize.Height + sentenceSize.Height;
            }

            if (string.IsNullOrWhiteSpace(block.EnglishSentence) == false)
            {
                textBounds = OffsetBounds(new CGSize(0, LINE_SPACER), textBounds);

                var engLabel = _L("English sentence:");
                var labelSize = DrawText(engLabel, textBounds);
                textBounds = OffsetBounds(labelSize, textBounds);

                var engSentence = _P(block.EnglishSentence);
                var sentenceSize = DrawText(engSentence, textBounds);
                textBounds = OffsetBounds(sentenceSize, textBounds);

                var largeWidth = Math.Max(labelSize.Width, sentenceSize.Width);
                totalWidth = Math.Max(totalWidth, largeWidth);
                totalHeight += LINE_SPACER + labelSize.Height + sentenceSize.Height;
            }

            return textBounds;
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

        private CGRect GetScreenSafeBounds(DrawParameters drawParameters, CGRect screenBounds)
        {
            var textDimensions = MeasureTextBlock(drawParameters);

            // Multiple screens end up having coordinates based on their layouts
            CGRect newBounds = new CGRect(CGPoint.Empty, screenBounds.Size);

            nfloat center = (nfloat)(screenBounds.Width - textDimensions.Width) / 2.0f;
            nfloat right = (nfloat)(screenBounds.Width - textDimensions.Width);
            nfloat mid = (nfloat)(screenBounds.Height - textDimensions.Height) / 2.0f;
            nfloat bottom = (nfloat)(screenBounds.Height - textDimensions.Height);

            int BLOCK_PADDING = (int)(screenBounds.Size.Height * 0.04f);

            switch (drawParameters.Position)
            {
                case DrawPosition.LeftTop:
                    newBounds.X += BLOCK_PADDING;
                    newBounds.Y += BLOCK_PADDING;
                    break;
                case DrawPosition.LeftMid:
                    newBounds.X += BLOCK_PADDING;
                    newBounds.Y = mid;
                    break;
                case DrawPosition.LeftBottom:
                    newBounds.X += BLOCK_PADDING;
                    newBounds.Y = bottom - BLOCK_PADDING;
                    break;
                case DrawPosition.CenterTop:
                    newBounds.X = center;
                    newBounds.Y += BLOCK_PADDING;
                    break;
                case DrawPosition.CenterMid:
                    newBounds.X = center;
                    newBounds.Y = mid;
                    break;
                case DrawPosition.CenterBottom:
                    newBounds.X = center;
                    newBounds.Y = bottom - BLOCK_PADDING;
                    break;
                case DrawPosition.RightTop:
                    newBounds.X = right - BLOCK_PADDING;
                    newBounds.Y += BLOCK_PADDING;
                    break;
                case DrawPosition.RightMid:
                    newBounds.X = right - BLOCK_PADDING;
                    newBounds.Y = mid;
                    break;
                case DrawPosition.RightBottom:
                    newBounds.X = right - BLOCK_PADDING;
                    newBounds.Y = bottom - BLOCK_PADDING;
                    break;
            }

            return newBounds;
        }

        /// <summary>
        /// Shifts a boundary by the Height provided in lastSize
        /// </summary>
        /// <param name="lastSize"></param>
        /// <param name="lastBounds"></param>
        /// <returns></returns>
        private CGRect OffsetBounds(CGSize lastSize, CGRect lastBounds)
        {
            return new CGRect(
                lastBounds.X,
                lastBounds.Y + lastSize.Height,
                lastBounds.Width,
                lastBounds.Height - lastSize.Height
            );
        }

        /// <summary>
        /// Invokes NSString.DrawInRect() to perform a draw call
        /// against the current graphics context
        /// </summary>
        /// <param name="fontParams"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        private CGSize DrawText(FontParams fontParams, CGRect bounds)
        {
            var options = FontParams.GetFontAttrs(fontParams);
            var nsString = fontParams.AsNSString;
            nsString.DrawInRect(bounds, options);

            var textSize = CalculateTextSize(fontParams);
            return textSize;
        }

        private CGSize CalculateTextSize(FontParams fontParams)
        {
            var bounds = new CGSize();
            var attr = new NSStringAttributes() { Font = fontParams.Font };

            var frame = fontParams.AsNSString.GetBoundingRect(
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

        private string GetNewImagePath()
        {
            Guid guid = Guid.NewGuid();
            string extension = "png";
            string fileName = $"{guid}.{extension}";
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
