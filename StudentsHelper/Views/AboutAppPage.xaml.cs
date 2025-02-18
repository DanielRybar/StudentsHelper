using StudentsHelper.ViewModels;

namespace StudentsHelper.Views;

public partial class AboutAppPage : ContentPage
{
	private readonly AboutAppViewModel viewModel;
    public AboutAppPage()
	{
		InitializeComponent();
        BindingContext = viewModel = new AboutAppViewModel();
    }
    protected override bool OnBackButtonPressed() => true;
}