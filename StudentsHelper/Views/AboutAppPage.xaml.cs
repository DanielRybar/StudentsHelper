using CommunityToolkit.Maui.PlatformConfiguration.AndroidSpecific;
using StudentsHelper.ViewModels;

namespace StudentsHelper.Views;

public partial class AboutAppPage : ContentPage
{
    private readonly AboutAppViewModel viewModel;
    public AboutAppPage()
    {
        InitializeComponent();
        On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetColor(Colors.Black);
        BindingContext = viewModel = new AboutAppViewModel();
    }
    protected override bool OnBackButtonPressed() => true;

    private async void DevImage_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Image img)
        {
            switch (img.AutomationId)
            {
                case "csharp":
                    await Launcher.OpenAsync("https://learn.microsoft.com/en-us/dotnet/csharp/");
                    break;
                case "net":
                    await Launcher.OpenAsync("https://dotnet.microsoft.com/en-us/");
                    break;
                case "maui":
                    await Launcher.OpenAsync("https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui?view=net-maui-9.0");
                    break;
            }
        }
    }
}