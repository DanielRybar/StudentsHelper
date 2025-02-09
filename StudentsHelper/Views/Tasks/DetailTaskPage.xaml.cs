using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Tasks;

namespace StudentsHelper.Views.Tasks;

public partial class DetailTaskPage : ContentPage
{
    private readonly DetailTaskViewModel viewModel;
    private readonly IShakeDetector shakeDetector = DependencyService.Get<IShakeDetector>();
    private bool isItemClicked = false;

    public DetailTaskPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new DetailTaskViewModel();
        shakeDetector.OnShaken += () =>
        {
            viewModel.RemoveCommand.Execute(null);
        };
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(800);
        MainLayout.IsVisible = true;
        shakeDetector.Start();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        shakeDetector.Stop();
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

    private async void EditItem_Clicked(object sender, EventArgs e)
    {
        if ((sender is ToolbarItem || sender is Button) && !isItemClicked)
        {
            isItemClicked = true;
            await Shell.Current.GoToAsync(nameof(EditTaskPage));
            // ...
            isItemClicked = false;
        }
    }
}