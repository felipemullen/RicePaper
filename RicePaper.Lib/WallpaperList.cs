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
        public WallpaperList() : base() { }
        #endregion

        #region ListIterator Implementation
        protected override IList<string> LoadData(string path)
        {
            var imageDirectory = new DirectoryInfo(path);

            var imageFiles = imageDirectory
                .EnumerateFiles()
                .Where(x => VALID_FILES.Contains(x.Extension))
                .Select(x => x.FullName);

            return imageFiles.ToList();
        }
        #endregion

        #region Public Methods
        public void Load(ImageOptionType option)
        {
            string path = AppSettings.GetFolderPath(option);
            Load(path);
        }
        #endregion
    }
}
