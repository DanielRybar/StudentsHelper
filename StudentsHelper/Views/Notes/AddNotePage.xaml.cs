using CommunityToolkit.Maui.Core.Platform;
using StudentsHelper.ViewModels.Notes;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace StudentsHelper.Views.Notes;

public partial class AddNotePage : ContentPage
{
    private readonly AddNoteViewModel viewModel;

    public AddNotePage()
    {
        InitializeComponent();
        BindingContext = viewModel = new AddNoteViewModel();
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

    private async void AddNote_Clicked(object sender, EventArgs e)
    {
        if (sender is ToolbarItem)
        {
            await TitleEntry.HideKeyboardAsync();
            await ContentEditor.HideKeyboardAsync();
        }
    }
}