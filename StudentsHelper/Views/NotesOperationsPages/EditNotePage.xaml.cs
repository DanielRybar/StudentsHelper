using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Platform;
using StudentsHelper.ViewModels.NotesOperationsViewModels;

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
}