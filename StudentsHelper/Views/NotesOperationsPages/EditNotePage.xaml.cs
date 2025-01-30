using StudentsHelper.ViewModels.NotesOperationsViewModels;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace StudentsHelper.Views.NotesOperationsPages;

public partial class EditNotePage : ContentPage
{
    private readonly EditNoteViewModel viewModel;
    public EditNotePage()
    {
        InitializeComponent();
        BindingContext = viewModel = new EditNoteViewModel();
    }

    private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry)
        {
            int length = e.NewTextValue.Length;
            if (length == 50)
            {
                await Toast.Make("Maxim�ln� d�lka n�zvu je 50 znak�.").Show();
            }
        }
    }

    private async void RemoveItem_Clicked(object sender, EventArgs e)
    {
        if (sender is ToolbarItem)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            if (await DisplayAlert("Potvrzen�", "Opravdu chcete odstranit tuto polo�ku?", "Ano", "Ne"))
            {
                this.LoadingText.Text = App.Current!.Resources["DeletingData"] as string;
                viewModel.RemoveCommand.Execute(null);
            }
        }
    }
}