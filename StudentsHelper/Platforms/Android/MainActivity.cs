using Android.App;
using Android.Content.PM;
using Android.Content.Res;

namespace StudentsHelper
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : MauiAppCompatActivity
    {
        // restrict enlarging fonts in the app
        public override Android.Content.Res.Resources? Resources
        {
            get
            {
                Configuration configuration = new();
                configuration.SetToDefaults();
                return CreateConfigurationContext(configuration).Resources;
            }
        }
    }
}
