using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RicePaper.Lib.Model;
using RicePaper.Lib.Utilities;

namespace RicePaper.Lib.Dictionary
{
    public class RiceDictionary : ListIterator<string>
    {
        #region Private Fields
        private readonly JishoApi jishoApi;
        private readonly SentenceFinder sentenceFinder;
        #endregion

        #region Constructor
        public RiceDictionary() : base()
        {
            this.jishoApi = new JishoApi();
            this.sentenceFinder = new SentenceFinder();
        }
        #endregion

        #region Public Methods
        public TextDetails CurrentDefinition()
        {
            string currentWord = CurrentItem;

            var sentence = sentenceFinder.GetEntry(currentWord);
            if (sentence != null)
            {
                return TextDetails.FromSentence(sentence);
            }
            else
            {
                var data = jishoApi.Search(currentWord);
                var textDetails = TextDetails.FromJisho(data);

                try
                {
                    textDetails.Romaji = RomajiConvert.FromKana(textDetails.Furigana);
                }
                catch (Exception) { }

                try
                {
                    var tatoebaSentence = sentenceFinder.FindSentences(currentWord);
                    if (sentence != null)
                    {
                        textDetails.JapaneseSentence = tatoebaSentence.JapaneseSentence;
                        textDetails.EnglishSentence = tatoebaSentence.EnglishSentence;
                    }
                }
                catch (Exception) { }

                return textDetails;
            }
        }
        #endregion

        #region ListIterator Implementation
        protected override IList<string> LoadData(string path)
        {
            var wordList = File.ReadAllLines(path);

            return wordList;
        }
        #endregion

        #region Public Methods
        public void Load(WordListSelection option)
        {
            var filePath = AppSettings.GetFilePath(option);
            Load(filePath);
        }
        #endregion
    }
}
