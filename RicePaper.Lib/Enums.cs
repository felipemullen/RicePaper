namespace RicePaper.Lib
{
    public enum DrawPosition
    {
        LeftTop,
        LeftMid,
        LeftBottom,
        CenterTop,
        CenterMid,
        CenterBottom,
        RightTop,
        RightMid,
        RightBottom
    }

    public enum ImageOptionType
    {
        //Japan,
        //Abstract,
        //Nature,
        MacDefault,
        Custom
    }

    public enum WordListSelection
    {
        MostFrequent100,
        MostFrequent1000,
        HeisigRTK,
        JLPTN5,
        JLPTN4,
        JLPTN3,
        JLPTN2,
        JLPTN1,
        Core6K,
        Custom
    }

    public enum AspectMode
    {
        Fill,
        Fit
    }

    public enum WordSelectionMode
    {
        Random,
        InOrder
    }

    public enum DictionarySelection
    {
        Jisho,
        JapanDict
    }

    public enum CycleIntervalUnit
    {
        Days,
        Hours,
        Minutes
    }

    public class Opacity
    {
        public static float Opaque = 1.0f;
        public static float Transparent = 0.0f;
    }
}