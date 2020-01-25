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
        public WallpaperList(AppSettings settings) : base(settings, settings.State.ImageIndex)
        {
        }
        #endregion

        #region ListIterator Implementation
        protected override IList<string> LoadData()
        {
            return LoadData(this.settings.ImagePath);
        }

        protected override IList<string> LoadData(string path)
        {
            settings.State.LastImagePath = path;

            var imageDirectory = new DirectoryInfo(path);

            var imageFiles = imageDirectory
                .EnumerateFiles()
                .Where(x => VALID_FILES.Contains(x.Extension))
                .Select(x => x.FullName);

            return imageFiles.ToList();
        }

        public override void LoadNewList(string path)
        {
            base.LoadNewList(path);
            settings.State.LastImagePath = path;
        }

        protected override void PostIncrement(int preIncrement, int postIncrement)
        {
            settings.State.ImageIndex = postIncrement;
        }
        #endregion
    }
}
