namespace RicePaper.Lib.Model
{
    public class UserPreferences
    {
        public bool ShowInDock { get; set; }
        public bool StartOnBoot { get; set; }

        public static UserPreferences Default
        {
            get
            {
                return new UserPreferences()
                {
                    ShowInDock = false,
                    StartOnBoot = false
                };
            }
        }
    }
}
