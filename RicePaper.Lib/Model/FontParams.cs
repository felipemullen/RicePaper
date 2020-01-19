using System;
using System.Drawing;
using AppKit;
using Foundation;

namespace RicePaper.Lib.Model
{
    public class FontParams
    {
        #region Constants
        public static string HEADING_FONT = "Helvetica Bold";
        public static int HEADING_SIZE = 100;
        public static string PARAGRAPH_FONT = "Helvetica";
        public static int PARAGRAPH_SIZE = 20;
        public static int LABEL_SIZE = 14;
        #endregion

        #region Factory
        static Func<string, FontParams> Make(string font, int size, NSColor color)
        {
            return (string text) =>
            {
                return new FontParams(text, font, size, color);
            };
        }
        #endregion

        #region Static Functions
        public static Func<string, FontParams> Heading(NSColor color)
        {
            return Make(HEADING_FONT, HEADING_SIZE, color);
        }

        public static Func<string, FontParams> Paragraph(NSColor color)
        {
            return Make(PARAGRAPH_FONT, PARAGRAPH_SIZE, color);
        }

        public static Func<string, FontParams> Label(NSColor color)
        {
            return Make(PARAGRAPH_FONT, LABEL_SIZE, color);
        }

        public static NSFont ToFont(string fontName, int fontSize)
        {
            return NSFont.FromFontName(fontName, fontSize);
        }

        public static NSDictionary GetFontAttrs(FontParams fontParams)
        {
            var options = new NSMutableDictionary();
            options.Add(NSStringAttributeKey.Font, fontParams.Font);
            options.Add(NSStringAttributeKey.ForegroundColor, fontParams.Color);
            return options;
        }
        #endregion

        #region Instance Fields
        public string TextContent;
        public string FontName;
        public int FontSize;
        public int FontColor;
        public NSFont Font;
        public NSColor Color;
        #endregion

        #region Instance Properties
        public NSString AsNSString
        {
            get { return new NSString(TextContent); }
        }
        #endregion

        #region Instance Constructor
        public FontParams(string text, string name, int size, NSColor color)
        {
            TextContent = text;
            FontName = name;
            FontSize = size;
            Color = color;
            Font = ToFont(name, size);
        }
        #endregion
    }
}
