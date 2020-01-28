using System;
using System.Linq;
using RicePaper.Lib.Dictionary;
using RicePaper.Lib.Utilities;

namespace RicePaper.Lib.Model
{
    public class TextDetails
    {
        public string Kanji;
        public string Furigana;
        public string Romaji;
        public string Definition;
        public string JapaneseSentence;
        public string EnglishSentence;

        public static TextDetails FromJisho(JishoResponse response)
        {
            JishoWordEntry topResult = response.data.First();
            var furigana = topResult.japanese
                    .Select(x => x?.reading)
                    .DefaultIfEmpty("")
                    .First();

            string romaji = "";
            try
            {
                romaji = RomajiConvert.FromKana(furigana);
            }
            catch (Exception) { }

            return new TextDetails()
            {
                Kanji = topResult.japanese
                    .Select(x => x?.word)
                    .DefaultIfEmpty("")
                    .First(),

                Furigana = furigana,

                Definition = string.Join("/", topResult.senses
                    .Select(x => x?.english_definitions)
                    .DefaultIfEmpty()
                    .First()),

                EnglishSentence = topResult.senses
                    .Select(x => x?.sentences?.ToString())
                    .DefaultIfEmpty()
                    .First(),

                Romaji = romaji,
                //JapaneseSentence =
            };
        }
    }
}