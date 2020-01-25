public class AppState
{
    public int ImageIndex;
    public int WordIndex;
    public string LastImagePath;
    public string LastWordListPath;
    public static AppState Default
    {
        get
        {
            return new AppState()
            {
                ImageIndex = 0,
                WordIndex = 0,
                LastImagePath = string.Empty,
                LastWordListPath = string.Empty
            };
        }
    }

}