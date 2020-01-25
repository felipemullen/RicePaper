using System.Collections.Generic;
using System.IO;
using AppKit;
using RicePaper.Lib.Model;

namespace RicePaper.Lib.Dictionary
{
    public class RiceDictionary : ListIterator<string>
    {
        #region Private Fields
        private readonly JishoApi jishoApi;
        #endregion

        #region Constructor
        public RiceDictionary(AppSettings settings) : base(settings, settings.State.WordIndex)
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
            return LoadData(this.settings.WordListPath);
        }

        protected override IList<string> LoadData(string path)
        {
            settings.State.LastWordListPath = path;

            var wordList = File.ReadAllLines(this.settings.WordListPath);

            return wordList;
        }

        public override void LoadNewList(string path)
        {
            base.LoadNewList(path);
            settings.State.LastWordListPath = path;
        }

        protected override void PostIncrement(int preIncrement, int postIncrement)
        {
            settings.State.WordIndex = postIncrement;
        }
        #endregion
    }
}
