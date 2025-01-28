namespace StudentsHelper.Helpers
{
    public static class Themes
    {
        public static readonly Dictionary<string, string> ThemesDictionary = new()
        {
            {"Adaptivní", "adaptive"},
            {"Světlý", "light"},
            {"Tmavý", "dark"}
        };

        public static void ApplyTheme(string theme, bool isKey = false)
        {
            var themeKey = isKey ? theme : ThemesDictionary[theme];
            switch (themeKey)
            {
                case "adaptive":
                    App.Current!.UserAppTheme = AppTheme.Unspecified;
                    break;
                case "light":
                    App.Current!.UserAppTheme = AppTheme.Light;
                    break;
                case "dark":
                    App.Current!.UserAppTheme = AppTheme.Dark;
                    break;
            }
        }
    }
}