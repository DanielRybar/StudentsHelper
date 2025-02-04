using CommunityToolkit.Maui.Core.Platform;
using StudentsHelper.Models;
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
            string cancel = "Zrušit";
            string camera = "Fotoaparát";
            string gallery = "Galerie";
            string action = await DisplayActionSheet(
                "Zvolte zpùsob nahrání fotografií", cancel, null,
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