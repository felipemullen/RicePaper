using System;
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

        #region Static Functions
        public static NSFont Heading
        {
            get { return ToFont(HEADING_FONT, HEADING_SIZE); }
        }

        public static NSFont Paragraph
        {
            get { return ToFont(PARAGRAPH_FONT, PARAGRAPH_SIZE); }
        }

        public static NSFont Label
        {
            get { return ToFont(PARAGRAPH_FONT, LABEL_SIZE); }
        }

        public static NSFont ToFont(FontParams fontParams)
        {
            return ToFont(fontParams.FontName, fontParams.FontSize);
        }

        public static NSFont ToFont(string fontName, int fontSize)
        {
            return NSFont.FromFontName(fontName, fontSize);
        }

        public static NSDictionary GetFontAttrs(NSFont font, NSColor color)
        {
            var options = new NSMutableDictionary();
            options.Add(NSStringAttributeKey.Font, font);
            options.Add(NSStringAttributeKey.ForegroundColor, color);
            return options;
        }
        #endregion

        #region Instance Fields
        public string FontName;
        public int FontSize;
        public int FontColor;
        #endregion
    }
}
