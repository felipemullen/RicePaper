using System;
using System.Diagnostics;
using System.IO;
using Accelerate;
using AppKit;
using CoreGraphics;
using Foundation;

namespace RicePaper.Lib
{
    public class Util
    {
        public const string CACHE_DIR = "rp_cache";

        private static NSString DISPLAY_ID_KEY = new NSString("NSScreenNumber");

        public static string AppContainer => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static string AppRoot => Directory.GetParent(AppContext.BaseDirectory.TrimEnd('/')).FullName;

        public static string CacheDirectory => Path.Combine(Util.AppContainer, CACHE_DIR);

        public static string DesktopBackupDirectory => Path.Combine(Util.AppContainer, CACHE_DIR, "desktop_backups");

        public static string NotFoundImagePath => Path.Combine(Util.AppRoot, "Resources/Content/images/image_not_found.jpg");

        public static void Alert(string title, string message, NSWindow window = null, NSAlertStyle alertStyle = NSAlertStyle.Critical)
        {
            if (window == null)
            {
                window = NSApplication.SharedApplication.KeyWindow;
            }

            var alert = new NSAlert()
            {
                AlertStyle = alertStyle,
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

        public static string ScreenId(NSScreen screen)
        {
            return screen.DeviceDescription.ValueForKey(DISPLAY_ID_KEY).ToString();
        }

        public static string ScreenImagePath(NSScreen screen)
        {
            var workspace = NSWorkspace.SharedWorkspace;
            var p = workspace.DesktopImageUrl(screen);

            return p.Path;
        }

        public static void RunOSAScript(string args)
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = @"/usr/bin/osascript",
                Arguments = args,
                CreateNoWindow = false,
                UseShellExecute = false,
                ErrorDialog = true,
                WindowStyle = ProcessWindowStyle.Normal,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            var prc = Process.Start(processInfo);
            prc.WaitForExit();
        }
    }
}
