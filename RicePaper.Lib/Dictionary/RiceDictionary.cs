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
        public RiceDictionary(AppSettings settings) : base(settings)
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
        protected override IList<string> LoadData()
        {
            var wordList = File.ReadAllLines(this.settings.WordListPath);

            return wordList;
        }

        protected override void PostIncrement(int preIncrement, int index)
        {
            this.index = this.index % this.currentList.Count;
            settings.State.WordIndex = this.index;
        }
        #endregion
    }
}
