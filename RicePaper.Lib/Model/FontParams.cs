﻿using System;
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
        public static int PARAGRAPH_SIZE = 32;
        public static int LABEL_SIZE = 22;
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
        public static Func<string, FontParams> Heading(NSColor color, float scale)
        {
            return Make(HEADING_FONT, (int)(HEADING_SIZE * scale), color);
        }

        public static Func<string, FontParams> Paragraph(NSColor color, float scale)
        {
            return Make(PARAGRAPH_FONT, (int)(PARAGRAPH_SIZE * scale), color);
        }

        public static Func<string, FontParams> Label(NSColor color, float scale)
        {
            return Make(PARAGRAPH_FONT, (int)(LABEL_SIZE * scale), color);
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
