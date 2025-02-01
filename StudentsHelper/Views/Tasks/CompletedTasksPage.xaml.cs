using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Tasks;

namespace StudentsHelper.Views.Tasks;

public partial class CompletedTasksPage : ContentPage
{
    private readonly CompletedTasksViewModel viewModel;
    private static double scrollY = 0;
    private static bool isLoaded = false;

    public CompletedTasksPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new CompletedTasksViewModel();
        viewModel.TasksCountChanged += CheckToolbarItems;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isLoaded)
        {
            WeakReferenceMessenger.Default.Send(new UpdateCompletedTasksMessage("Collection modified"));
            isLoaded = true;
        }
    }

    private void CheckToolbarItems(int count)
    {
        MainScrollView.ScrollToAsync(0, scrollY, false);
        MainThread.BeginInvokeOnMainThread(() =>
        {
            CheckEmptyView(count);
        });
    }

    private void CheckEmptyView(int count)
    {
        if (count == 0)
        {
            MainCollectionView.IsVisible = false;
            EmptyLayout.IsVisible = true;
        }
        else
        {
            MainCollectionView.IsVisible = true;
            EmptyLayout.IsVisible = false;
        }
    }
}