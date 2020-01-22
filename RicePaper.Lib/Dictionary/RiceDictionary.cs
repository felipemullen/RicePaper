using System;
using System.Collections.Generic;
using RicePaper.Lib.Model;

namespace RicePaper.Lib.Dictionary
{
    public class RiceDictionary : ListIterator<string>
    {
        #region Private Fields
        private readonly AppSettings settings;
        private readonly JishoApi jishoApi;
        #endregion

        #region Constructor
        public RiceDictionary(AppSettings settings)
        {
            this.settings = settings;
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
        protected override List<string> LoadData()
        {
            // TODO: load from a list
            return new List<string>() { "音楽", "食べ物", "日本語", "塵", "台所" };
        }

        protected override void PostIncrement(int preIncrement, int index)
        {
            this.index = this.index % this.currentList.Count;
            settings.State.WordIndex = this.index;
        }
        #endregion
    }
}
