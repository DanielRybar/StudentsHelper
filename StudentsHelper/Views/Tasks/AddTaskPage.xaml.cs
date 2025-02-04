using CommunityToolkit.Maui.Core.Platform;
using StudentsHelper.ViewModels.Tasks;
using Toast = CommunityToolkit.Maui.Alerts.Toast;

namespace StudentsHelper.Views.Tasks;

public partial class AddTaskPage : ContentPage
{
    private readonly AddTaskViewModel viewModel;
    public AddTaskPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new AddTaskViewModel();
        viewModel.PhotoChanged += async () =>
        {
            //await Task.Delay(500);
            //await MainScrollView.ScrollToAsync(0, MainScrollView.ContentSize.Height, false);
        };
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

    private async void AddTask_Clicked(object sender, EventArgs e)
    {
        if (sender is ToolbarItem)
        {
            await TitleEntry.HideKeyboardAsync();
            await ContentEditor.HideKeyboardAsync();
        }
    }

    private async void AddPhotos_Clicked(object sender, EventArgs e)
    {
        if (sender is Button)
        {
            string cancel = "Zru�it";
            string camera = "Fotoapar�t";
            string gallery = "Galerie";
            string action = await DisplayActionSheet(
                "Zvolte zp�sob nahr�n� fotografi�", cancel, null,
                camera, gallery);

            if (action is not null && action != cancel)
            {
                if (action == camera)
                {
                    viewModel.AddPhotosCommand.Execute(true);
                }
                else if (action == gallery)
                {
                    viewModel.AddPhotosCommand.Execute(false);
                }
            }
        }
    }
}