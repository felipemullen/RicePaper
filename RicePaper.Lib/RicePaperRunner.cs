using System;
namespace RicePaper.Lib
{
    public class RicePaperRunner
    {
        private WallpaperUtility wallpaperUtility;

        public RicePaperRunner()
        {
            wallpaperUtility = new WallpaperUtility();
            // TODO: Load saved state
        }

        public void BeginScheduling()
        {
            string filepath = "/Users/fmullen/Desktop/sell/4Q1A4544.JPG";
            wallpaperUtility.SetWallpaper(filepath);
        }


    }
}
