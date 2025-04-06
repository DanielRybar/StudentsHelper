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
        if (sender is Entry entry)
        {
            int length = e.NewTextValue.Length;
            if (length == entry.MaxLength)
            {
                entry.Text = e.NewTextValue[..(entry.MaxLength - 1)];
                await Toast.Make($"Maximální délka názvu je {entry.MaxLength - 1} znaků.").Show();
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