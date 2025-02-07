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

    private async void TimePicker_TimeSelected(object sender, TimeChangedEventArgs e)
    {
        if (sender is TimePicker && ((viewModel.DueDate + e.NewTime) < DateTime.Now.AddHours(2)))
        {
            await Toast.Make("�kol mus� b�t napl�nov�n minim�ln� 2 hodiny dop�edu.").Show();
            viewModel.SelectedTime = e.OldTime;
        }
    }

    private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (sender is DatePicker && ((e.NewDate + viewModel.SelectedTime) < DateTime.Now.AddHours(2)))
        {
            await Toast.Make("�kol mus� b�t napl�nov�n minim�ln� 2 hodiny dop�edu.").Show();
            viewModel.DueDate = e.OldDate;
        }
    }

    private void RemovePhoto_Clicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            viewModel.RemovePhotoCommand.Execute(btn.CommandParameter);
        }
    }
}