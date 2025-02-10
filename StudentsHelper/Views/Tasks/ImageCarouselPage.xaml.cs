using StudentsHelper.Controls;
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

    private void ZoomableView_GestureStarted(object sender, EventArgs e)
    {
        this.MainCarousel.IsSwipeEnabled = false;
    }

    private void ZoomableView_GestureEnded(object sender, EventArgs e)
    {
        if (sender is ZoomableView zv && !zv.IsZoomActive)
        {
            this.MainCarousel.IsSwipeEnabled = true;
        }
    }
}