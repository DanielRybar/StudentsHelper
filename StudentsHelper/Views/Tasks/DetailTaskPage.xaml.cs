using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Tasks;

namespace StudentsHelper.Views.Tasks;

public partial class DetailTaskPage : ContentPage
{
    private readonly DetailTaskViewModel viewModel;
    private bool isItemClicked = false;

    public DetailTaskPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new DetailTaskViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(800);
        MainLayout.IsVisible = true;
    }

    private async void Image_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Image image)
        {
            await Shell.Current.GoToAsync(nameof(ImageCarouselPage));
            WeakReferenceMessenger.Default.Send(
                new ImageDetailMessage(
                    new Models.MessageModels.PhotoModel([.. viewModel.Photos], (image.BindingContext as string)!)));
        }
    }

    private async void RemoveItem_Clicked(object sender, EventArgs e)
    {
        if (sender is ToolbarItem)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            if (await DisplayAlert("Potvrzení", "Opravdu chcete odstranit tuto položku?", "Ano", "Ne"))
            {
                viewModel.RemoveCommand.Execute(null);
            }
        }
    }

    private void EditItem_Clicked(object sender, EventArgs e)
    {
        if ((sender is ToolbarItem || sender is Button) && !isItemClicked)
        {
            isItemClicked = true;
            // ...
            isItemClicked = false;
        }
    }
}