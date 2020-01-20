using System;
namespace RicePaper.Lib.Model
{
    public class CycleInfo
    {
        public CycleIntervalUnit CycleType { get; set; }
        public int Interval { get; set; }

        public TimeSpan Period
        {
            get
            {
                switch (CycleType)
                {
                    case CycleIntervalUnit.Days: return TimeSpan.FromDays(Interval);
                    case CycleIntervalUnit.Hours: return TimeSpan.FromHours(Interval);
                    case CycleIntervalUnit.Minutes: return TimeSpan.FromMinutes(Interval);
                    default: return TimeSpan.Zero;
                }
            }
        }


        public static CycleInfo Default
        {
            get
            {
                return new CycleInfo()
                {
                    CycleType = CycleIntervalUnit.Days,
                    Interval = 1
                };
            }
        }
    }
}
