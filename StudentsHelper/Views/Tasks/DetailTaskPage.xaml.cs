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
        Microsoft.Maui.Handlers.LabelHandler.Mapper.AppendToMapping(nameof(Label) + "Selectable", (handler, view) =>
        {
            handler.PlatformView.SetTextIsSelectable(true);
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(1000);
        MainLayout.IsVisible = true;
        shakeDetector.OnShaken += OnShakeDetected;
        shakeDetector.Start();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        shakeDetector.OnShaken -= OnShakeDetected;
        shakeDetector.Stop();
    }

    private void OnShakeDetected()
    {
        Vibration.Default.Vibrate();
        viewModel.RemoveCommand.Execute(null);
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
            WeakReferenceMessenger.Default.Send(new EditingTaskMessage(viewModel.TaskItem));
            isItemClicked = false;
        }
    }

    private void FinishItem_Clicked(object sender, EventArgs e)
    {
        if (sender is ToolbarItem && !isItemClicked)
        {
            isItemClicked = true;
            this.LoadingText.Text = App.Current!.Resources["LoadingData"] as string;
            viewModel.SetCompletedCommand.Execute(null);
            isItemClicked = false;
        }
    }
}