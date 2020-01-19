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

    public enum AspectMode
    {
        Fill,
        Fit
    }

    public class Opacity
    {
        public static float Opaque = 1.0f;
        public static float Transparent = 0.0f;
    }
}