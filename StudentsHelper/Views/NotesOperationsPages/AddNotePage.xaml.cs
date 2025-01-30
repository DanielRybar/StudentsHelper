using Microsoft.Maui.Platform;
using StudentsHelper.ViewModels.NotesOperationsViewModels;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace StudentsHelper.Views.NotesOperationsPages;

public partial class AddNotePage : ContentPage
{
    private readonly AddNoteViewModel viewModel;

    public AddNotePage()
    {
        InitializeComponent();
        BindingContext = viewModel = new AddNoteViewModel();

        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
            handler.PlatformView.Background = null;
            handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
        });
        Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(Editor), (handler, view) =>
        {
            handler.PlatformView.Background = null;
            handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
        });
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