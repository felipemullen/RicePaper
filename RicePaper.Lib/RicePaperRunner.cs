using RicePaper.Lib.Model;

namespace RicePaper.Lib
{
    public class RicePaperRunner
    {
        #region Private Fields
        private readonly WallpaperMaker wallpaperUtility;
        #endregion

        #region Constructor
        public RicePaperRunner()
        {
            wallpaperUtility = new WallpaperMaker();

            // TODO: Load saved state
        }
        #endregion

        #region Public Methods
        public void BeginScheduling()
        {
            string filepath = "/Users/fmullen/Desktop/sell/4Q1A4544.JPG";

            var details = new DrawParameters
            {
                Position = DrawPosition.CenterMid,
                Text = GetDetailsFromWord("")
            };

            wallpaperUtility.SetWallpaper(filepath, details);
        }
        #endregion

        #region Private Helpers
        public TextDetails GetDetailsFromWord(string word)
        {
            // TODO: Dictionary lookup
            return new TextDetails
            {
                Kanji = "音楽",
                Furigana = "おんがく",
                Romaji = "ongaku",
                Definition = "song/music",
                Sentence = "僕は音楽が大好きですね"
            };
        }
        #endregion

    }
}
