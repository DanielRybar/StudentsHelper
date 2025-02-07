using StudentsHelper.ViewModels.Tasks;

namespace StudentsHelper.Views.Tasks;

public partial class ImageCarouselPage : ContentPage
{
    private readonly ImageCarouselViewModel viewModel;

    public ImageCarouselPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new ImageCarouselViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(500);
        CarouselLayout.IsVisible = true;
    }
}