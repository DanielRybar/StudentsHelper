using StudentsHelper.ViewModels;

namespace StudentsHelper.Views;

public partial class NotesPage : ContentPage
{
	private readonly NotesViewModel viewModel;
    public NotesPage()
	{
		InitializeComponent();
        BindingContext = viewModel = new NotesViewModel();
    }
}