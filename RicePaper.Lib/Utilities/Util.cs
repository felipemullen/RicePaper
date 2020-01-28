using System;
using System.IO;
using AppKit;

namespace RicePaper.Lib
{
    public class Util
    {
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
    }
}
