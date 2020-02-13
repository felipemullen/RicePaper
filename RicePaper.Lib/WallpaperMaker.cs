using RicePaper.Lib.Model;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

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

        #region Constructor
        public WallpaperMaker()
        {
            try
            {
                if (Directory.Exists(Util.CacheDirectory) == false)
                    Directory.CreateDirectory(Util.CacheDirectory);

                if (Directory.Exists(Util.DesktopBackupDirectory) == false)
                    Directory.CreateDirectory(Util.DesktopBackupDirectory);
            }
            catch (Exception ex)
            {
                NSAlert alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Critical,
                    MessageText = "Unable to create cache directory!",
                    InformativeText = ex.Message
                };

                alert.RunModal();
                System.Environment.Exit(0);
            }
        }
        #endregion

        #region Public Methods
        public void SetWallpaper(string imagePath, DrawParameters drawDetails)
        {
#if __MACOS__
            try
            {
                var workspace = NSWorkspace.SharedWorkspace;

                foreach (var _screen in NSScreen.Screens)
                {
                    string filepath = imagePath;

                    // Use current wallpapers if option is "unchanged"
                    if (string.IsNullOrWhiteSpace(filepath))
                    {
                        string id = Util.ScreenId(_screen);
                        string screenImage = Util.ScreenImagePath(_screen);

                        // First, check if this image has been backed up
                        if (DesktopBackup.Backups.ContainsKey(id))
                        {
                            var backupFile = DesktopBackup.Backups[id];

                            if (File.Exists(backupFile.OriginalLocation))
                            {
                                filepath = backupFile.OriginalLocation;
                            }
                        }
                        // In the event of multiple screens being connected, there may not be a backup
                        // but we don't ever want to set an image that already has text on it
                        else if (screenImage.Contains(Util.CACHE_DIR) == false)
                        {
                            filepath = screenImage;
                        }
                        // We can copy the existing backup as a last resort
                        else if (DesktopBackup.Backups.Count > 0)
                        {
                            filepath = DesktopBackup.Backups.First().Value.OriginalLocation;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(filepath))
                        filepath = Util.NotFoundImagePath;

                    string cachePath = DrawImage(_screen, filepath, drawDetails);

                    if (cachePath != null)
                    {
                        NSUrl url = NSUrl.FromFilename(cachePath);
                        NSError errorContainer = new NSError();

                        var options = workspace.DesktopImageOptions(_screen);
                        var result = workspace.SetDesktopImageUrl(url, _screen, options, errorContainer);

                        if (result)
                            Console.WriteLine($"successfully set {cachePath}");
                        else
                            Console.WriteLine(errorContainer.ToString());
                    }

                    Task.Run(() =>
                    {
                        CleanupCache();
                    });
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

            var cacheDirectory = new DirectoryInfo(Util.CacheDirectory);
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

                    if (pixelAverage.R == 255)
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
        private TextMeasurements MeasureTextBlock(DrawParameters drawParameters)
        {
            var measurements = new TextMeasurements();

            var _H = FontParams.Heading(drawParameters.TextColor, drawParameters.PrimaryTextScale);
            var _L = FontParams.Label(drawParameters.TextColor, drawParameters.SecondaryTextScale);
            var _P = FontParams.Paragraph(drawParameters.TextColor, drawParameters.SecondaryTextScale);

            var block = drawParameters.Text;

            if (string.IsNullOrWhiteSpace(block.Kanji))
            {
                measurements.Kanji = CalculateTextSize(_H(block.Furigana));
            }
            else
            {
                measurements.Kanji = CalculateTextSize(_H(block.Kanji));

                var subheadingText = block.Furigana;
                if (string.IsNullOrWhiteSpace(block.Romaji) == false)
                    subheadingText = $"{subheadingText} ({block.Romaji})";

                measurements.Furigana = CalculateTextSize(_L(subheadingText));
            }

            if (string.IsNullOrWhiteSpace(block.Definition) == false)
            {
                measurements.DefinitionLabel = CalculateTextSize(_L("Definition:"));
                measurements.Spacing.AddHeight(LINE_SPACER);

                var labelBox = new ExpansionBox();
                var lines = block.Definition.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var lineSize = CalculateTextSize(_P(line));
                    labelBox.AddHeight(lineSize.Height).MaxWidth(lineSize.Width);
                }
                measurements.Definition = labelBox.CGrect.Size;
            }

            if (string.IsNullOrWhiteSpace(block.JapaneseSentence) == false)
            {
                measurements.JapaneseSentenceLabel = CalculateTextSize(_L("Japanese sentence:"));
                measurements.JapaneseSentence = CalculateTextSize(_P(block.JapaneseSentence));
            }

            if (string.IsNullOrWhiteSpace(block.EnglishSentence) == false)
            {
                measurements.EnglishSentenceLabel = CalculateTextSize(_L("English sentence:"));
                measurements.EnglishSentence = CalculateTextSize(_P(block.EnglishSentence));
            }

            return measurements;
        }

        private CGRect DrawTextBlock(DrawParameters drawParameters, CGRect screenBounds)
        {
            var _H = FontParams.Heading(drawParameters.TextColor, drawParameters.PrimaryTextScale);
            var _L = FontParams.Label(drawParameters.TextColor, drawParameters.SecondaryTextScale);
            var _P = FontParams.Paragraph(drawParameters.TextColor, drawParameters.SecondaryTextScale);

            var measurements = MeasureTextBlock(drawParameters);
            CGRect textBoxBounds = measurements.Totals.CGrect;
            CGRect drawBounds = GetScreenSafeBounds(drawParameters, measurements, screenBounds);

            var block = drawParameters.Text;
            bool isRightAligned = ShouldRightAlign(drawParameters);

            if (string.IsNullOrWhiteSpace(block.Kanji))
            {
                var heading = _H(block.Furigana);
                var textSize = DrawText(heading, drawBounds, textBoxBounds, measurements.Kanji, isRightAligned);
                drawBounds = OffsetBounds(textSize, drawBounds);
            }
            else
            {
                var heading = _H(block.Kanji);
                var headingSize = DrawText(heading, drawBounds, textBoxBounds, measurements.Kanji, isRightAligned);
                drawBounds = OffsetBounds(headingSize, drawBounds);

                var subheadingText = block.Furigana;
                if (string.IsNullOrWhiteSpace(block.Romaji) == false)
                    subheadingText = $"{subheadingText} ({block.Romaji})";

                var subheading = _L(subheadingText);
                var subHeadingSize = DrawText(subheading, drawBounds, textBoxBounds, measurements.Furigana, isRightAligned);
                drawBounds = OffsetBounds(subHeadingSize, drawBounds);
            }

            if (string.IsNullOrWhiteSpace(block.Definition) == false)
            {
                drawBounds = OffsetBounds(new CGSize(0, LINE_SPACER), drawBounds);
                var definitionLabel = _L("Definition:");
                var labelSize = DrawText(definitionLabel, drawBounds, textBoxBounds, measurements.DefinitionLabel, isRightAligned);
                drawBounds = OffsetBounds(labelSize, drawBounds);

                var lines = block.Definition.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var definitionLine = _P(line);
                    var lineSize = DrawText(definitionLine, drawBounds, textBoxBounds, measurements.Definition, isRightAligned);
                    drawBounds = OffsetBounds(lineSize, drawBounds);
                }

            }

            if (string.IsNullOrWhiteSpace(block.JapaneseSentence) == false)
            {
                drawBounds = OffsetBounds(new CGSize(0, LINE_SPACER), drawBounds);

                var japLabel = _L("Japanese sentence:");
                var labelSize = DrawText(japLabel, drawBounds, textBoxBounds, measurements.JapaneseSentenceLabel, isRightAligned);
                drawBounds = OffsetBounds(labelSize, drawBounds);

                var japSentence = _P(block.JapaneseSentence);
                var sentenceSize = DrawText(japSentence, drawBounds, textBoxBounds, measurements.JapaneseSentence, isRightAligned);
                drawBounds = OffsetBounds(sentenceSize, drawBounds);
            }

            if (string.IsNullOrWhiteSpace(block.EnglishSentence) == false)
            {
                drawBounds = OffsetBounds(new CGSize(0, LINE_SPACER), drawBounds);

                var engLabel = _L("English sentence:");
                var labelSize = DrawText(engLabel, drawBounds, textBoxBounds, measurements.EnglishSentenceLabel, isRightAligned);
                drawBounds = OffsetBounds(labelSize, drawBounds);

                var engSentence = _P(block.EnglishSentence);
                var sentenceSize = DrawText(engSentence, drawBounds, textBoxBounds, measurements.EnglishSentence, isRightAligned);
                drawBounds = OffsetBounds(sentenceSize, drawBounds);
            }

            return drawBounds;
        }
        #endregion

        #region Private Helpers
        private void WriteImageToFile(string filePath, CGImage image)
        {
            var fileURL = NSUrl.FromFilename(filePath);
            var imageDestination = CGImageDestination.Create(fileURL, UTType.JPEG, 1);
            imageDestination.AddImage(image);
            imageDestination.Close();
        }

        private CGRect GetScreenSafeBounds(DrawParameters drawParameters, TextMeasurements measurements, CGRect screenBounds)
        {
            var textDimensions = measurements.Totals;

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

        private bool ShouldRightAlign(DrawParameters parameters)
        {
            switch (parameters.Position)
            {
                case DrawPosition.RightTop:
                case DrawPosition.RightMid:
                case DrawPosition.RightBottom:
                    return true;
                case DrawPosition.LeftTop:
                case DrawPosition.LeftMid:
                case DrawPosition.LeftBottom:
                case DrawPosition.CenterTop:
                case DrawPosition.CenterMid:
                case DrawPosition.CenterBottom:
                default:
                    return false;
            }
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
        private CGSize DrawText(FontParams fontParams, CGRect screenBounds, CGRect textBlockBounds, CGSize textDimensions, bool rightAlign)
        {
            var options = FontParams.GetFontAttrs(fontParams);
            var nsString = fontParams.AsNSString;

            if (rightAlign)
                screenBounds.X = screenBounds.X + textBlockBounds.Width - textDimensions.Width;

            nsString.DrawInRect(screenBounds, options);

            return textDimensions;
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
                stream.Close();
            }

            return img;
        }

        private string GetNewImagePath()
        {
            Guid guid = Guid.NewGuid();
            string extension = "png";
            string fileName = $"{guid}.{extension}";
            return Path.Combine(Util.CacheDirectory, fileName);
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
