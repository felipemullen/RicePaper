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
            string filepath1 = "/Users/fmullen/Desktop/backgrounds/bigger_bigger.png";
            string filepath2 = "/Users/fmullen/Desktop/backgrounds/long_bigger.png";
            string filepath3 = "/Users/fmullen/Desktop/backgrounds/long_smaller.png";
            string filepath4 = "/Users/fmullen/Desktop/backgrounds/tall_bigger.png";
            string filepath5 = "/Users/fmullen/Desktop/backgrounds/tall_smaller.png";

            var details = new DrawParameters
            {
                Position = DrawPosition.CenterMid,
                Text = GetDetailsFromWord("")
            };

            wallpaperUtility.SetWallpaper(filepath1, details);
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
                JapaneseSentence = "僕は音楽が大好きですね",
                EnglishSentence = "I love music ya kno"
            };
        }
        #endregion

    }
}
