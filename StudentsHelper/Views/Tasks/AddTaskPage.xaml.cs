using StudentsHelper.ViewModels.Tasks;

namespace StudentsHelper.Views.Tasks;

public partial class AddTaskPage : ContentPage
{
	private readonly AddTaskViewModel viewModel;
    public AddTaskPage()
	{
		InitializeComponent();
        BindingContext = viewModel = new AddTaskViewModel();
    }
}