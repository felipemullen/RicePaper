﻿public class AppState
{
    public int ImageIndex;
    public int WordIndex;
    public static AppState Default
    {
        get
        {
            return new AppState()
            {
                ImageIndex = 0,
                WordIndex = 0
            };
        }
    }
}