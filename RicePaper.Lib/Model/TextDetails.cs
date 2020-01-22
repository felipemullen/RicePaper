using System;
using System.Linq;
using RicePaper.Lib.Dictionary;

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

            return new TextDetails()
            {
                Kanji = topResult.japanese
                    .Select(x => x?.word)
                    .DefaultIfEmpty("")
                    .First(),

                Furigana = topResult.japanese
                    .Select(x => x?.reading)
                    .DefaultIfEmpty("")
                    .First(),

                Definition = string.Join("/", topResult.senses
                    .Select(x => x?.english_definitions)
                    .DefaultIfEmpty()
                    .First()),

                EnglishSentence = topResult.senses
                    .Select(x => x?.sentences?.ToString())
                    .DefaultIfEmpty()
                    .First()

                // TODO: Translate to romaji? Find sentences?
                //Romaji =
                //JapaneseSentence =
            };
        }
    }
}