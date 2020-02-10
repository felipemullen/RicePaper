using System;
using System.IO;
using Accelerate;
using AppKit;
using CoreGraphics;

namespace RicePaper.Lib
{
    public class Util
    {
        public static string AppContainer => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static string AppRoot => Directory.GetParent(AppContext.BaseDirectory.TrimEnd('/')).FullName;

        public static void Alert(string title, string message, NSWindow window, NSAlertStyle alertStyle = NSAlertStyle.Critical)
        {
            var alert = new NSAlert()
            {
                AlertStyle = NSAlertStyle.Critical,
                MessageText = title,
                InformativeText = message
            };
            alert.BeginSheet(window);
        }

        public static Pixel8888 GetAverageColor(byte[] data, int width, int height)
        {
            long[] totals = new long[] { 0, 0, 0 };

            for (int i = 0; i < data.Length; i += 4)
            {
                for (int color = 0; color < totals.Length; color++)
                {
                    totals[color] += data[i + color];
                }
            }

            return new Pixel8888()
            {
                A = 255,
                B = (byte)(totals[0] / (width * height)),
                G = (byte)(totals[1] / (width * height)),
                R = (byte)(totals[2] / (width * height))
            };
        }

        /// <summary>
        /// Uses perceptive luminance to calculate contrast
        /// https://www.w3.org/TR/AERT/#color-contrast
        /// </summary>
        public static CGColor ContrastColor(Pixel8888 average)
        {
            double luminance = (0.299 * average.R + 0.587 * average.G + 0.114 * average.B) / 255;
            int hue = (luminance > 0.5) ? 0 : 255;

            return new CGColor(hue, hue, hue);
        }

        public static NSColor ToNSColor(Pixel8888 c)
        {
            return NSColor.FromCalibratedRgba(c.R, c.G, c.B, c.A);
        }
    }
}
