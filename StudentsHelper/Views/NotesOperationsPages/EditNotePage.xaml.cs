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
                await Toast.Make("Maximální délka názvu je 50 znakù.").Show();
            }
        }
    }

    private async void RemoveItem_Clicked(object sender, EventArgs e)
    {
        if (sender is ToolbarItem)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            if (await DisplayAlert("Potvrzení", "Opravdu chcete odstranit tuto položku?", "Ano", "Ne"))
            {
                this.LoadingText.Text = App.Current!.Resources["DeletingData"] as string;
                viewModel.RemoveCommand.Execute(null);
            }
        }
    }
}