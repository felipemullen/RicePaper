using System.Collections.Generic;
using System.IO;
using RicePaper.Lib.Model;

namespace RicePaper.Lib.Dictionary
{
    public class RiceDictionary : ListIterator<string>
    {
        #region Private Fields
        private readonly JishoApi jishoApi;
        #endregion

        #region Constructor
        public RiceDictionary() : base()
        {
            this.jishoApi = new JishoApi();
        }
        #endregion

        #region Public Methods
        public TextDetails CurrentDefinition()
        {
            string currentWord = CurrentItem;
            var data = jishoApi.Search(currentWord);

            return TextDetails.FromJisho(data);
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
