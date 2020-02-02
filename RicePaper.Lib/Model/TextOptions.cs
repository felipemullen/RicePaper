namespace RicePaper.Lib.Model
{
    public class TextOptions
    {
        public bool Kanji { get; set; }
        public bool Furigana { get; set; }
        public bool Romaji { get; set; }
        public bool Definition { get; set; }
        public bool JapaneseSentence { get; set; }
        public bool EnglishSentence { get; set; }

        public static TextOptions Default
        {
            get
            {
                return new TextOptions()
                {
                    Kanji = true,
                    Furigana = true,
                    Romaji = true,
                    Definition = true,
                    JapaneseSentence = true,
                    EnglishSentence = true
                };
            }
        }
    }
}
