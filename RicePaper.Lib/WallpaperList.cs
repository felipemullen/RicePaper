using System.Collections.Generic;
using RicePaper.Lib.Model;

namespace RicePaper.Lib
{
    public class WallpaperList : ListIterator<string>
    {
        #region Private Fields
        private readonly AppSettings settings;
        #endregion

        #region Constructor
        public WallpaperList(AppSettings settings) : base()
        {
            this.settings = settings;
        }
        #endregion

        #region ListIterator Implementation
        protected override List<string> LoadData()
        {
            // TODO: load from directory
            return new List<string>()
            {
                { "/Users/fmullen/Desktop/backgrounds/bigger_bigger.png" },
                { "/Users/fmullen/Desktop/backgrounds/long_bigger.png" },
                { "/Users/fmullen/Desktop/backgrounds/long_smaller.png" },
                { "/Users/fmullen/Desktop/backgrounds/tall_bigger.png" },
                { "/Users/fmullen/Desktop/backgrounds/tall_smaller.png" }
            };
        }

        protected override void PostIncrement(int preIncrement, int currentIndex)
        {
            // TODO: implement random ordering
            this.index = this.index % this.currentList.Count;
            settings.State.ImageIndex = index;
        }
        #endregion
    }
}
