using CommunityToolkit.Maui.Core.Platform;
using StudentsHelper.ViewModels.Notes;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace StudentsHelper.Views.Notes;

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
                await Toast.Make("Maximální délka názvu je 50 znaků.").Show();
            }
        }
    }

    private async void RemoveItem_Clicked(object sender, EventArgs e)
    {
        if (sender is ToolbarItem)
        {
            await TitleEntry.HideKeyboardAsync();
            await ContentEditor.HideKeyboardAsync();
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            if (await DisplayAlert("Potvrzení", "Opravdu chcete odstranit tuto položku?", "Ano", "Ne"))
            {
                this.LoadingText.Text = App.Current!.Resources["DeletingData"] as string;
                viewModel.RemoveCommand.Execute(null);
            }
        }
    }

    private async void EditNote_Clicked(object sender, EventArgs e)
    {
        if (sender is ToolbarItem)
        {
            await TitleEntry.HideKeyboardAsync();
            await ContentEditor.HideKeyboardAsync();
        }
    }
}