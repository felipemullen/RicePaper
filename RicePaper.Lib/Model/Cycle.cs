using System;
namespace RicePaper.Lib.Model
{
    public class Cycle
    {
        public CycleType CycleType { get; set; }
        public int Duration { get; set; }

        public static Cycle Default
        {
            get
            {
                return new Cycle()
                {
                    CycleType = CycleType.Days,
                    Duration = 1
                };
            }
        }
    }
}
