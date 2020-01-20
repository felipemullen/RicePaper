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
        Japan,
        Abstract,
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
        CORE6K,
        Custom
    }

    public enum AspectMode
    {
        Fill,
        Fit
    }

    public enum WordSelection
    {
        Random,
        InOrder
    }

    public enum DictionarySelection
    {
        Jisho,
        JapanDict
    }

    public enum CycleType
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