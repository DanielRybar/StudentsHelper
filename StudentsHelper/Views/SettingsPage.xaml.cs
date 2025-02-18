using StudentsHelper.ViewModels;

namespace StudentsHelper.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel viewModel;

    public SettingsPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new SettingsViewModel();
    }
    protected override bool OnBackButtonPressed() => true;
}