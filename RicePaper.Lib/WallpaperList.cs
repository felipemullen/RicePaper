using System.Collections.Generic;
using System.IO;
using System.Linq;
using RicePaper.Lib.Model;

namespace RicePaper.Lib
{
    public class WallpaperList : ListIterator<string>
    {
        #region Constants
        private readonly string[] VALID_FILES = { ".png", ".jpg", ".gif" };
        #endregion

        #region Constructor
        public WallpaperList(AppSettings settings) : base(settings)
        {
        }
        #endregion

        #region ListIterator Implementation
        protected override IList<string> LoadData()
        {
            var imageDirectory = new DirectoryInfo(this.settings.ImagePath);

            var imageFiles = imageDirectory
                .EnumerateFiles()
                .Where(x => VALID_FILES.Contains(x.Extension))
                .Select(x => x.FullName);

            return imageFiles.ToList();
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
