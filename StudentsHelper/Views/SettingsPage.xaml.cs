using CommunityToolkit.Maui.PlatformConfiguration.AndroidSpecific;
using StudentsHelper.ViewModels;

namespace StudentsHelper.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel viewModel;

    public SettingsPage()
    {
        InitializeComponent();
        On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetColor(Colors.Black);
        BindingContext = viewModel = new SettingsViewModel();
    }
    protected override bool OnBackButtonPressed() => true;
}