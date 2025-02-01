using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Tasks;

namespace StudentsHelper.Views.Tasks;

public partial class ActiveTasksPage : ContentPage
{
    private readonly PendingTasksViewModel viewModel;
    private static double scrollY = 0;
    private static bool isLongPress = false;
    private static bool isLoaded = false;

    public ActiveTasksPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new PendingTasksViewModel();
        viewModel.TasksCountChanged += CheckToolbarItems;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isLoaded)
        {
            WeakReferenceMessenger.Default.Send(new UpdatePendingTasksMessage("Collection modified"));
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

    private async void LongPress_ItemOptions(object sender, CommunityToolkit.Maui.Core.LongPressCompletedEventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is TaskItem task)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            isLongPress = true;
            scrollY = MainScrollView.ScrollY;

            string cancel = "Zrušit";
            string setAsCompleted = "Oznaèit úkol jako dokonèený";
            string remove = "Smazat úkol";
            string action = await DisplayActionSheet(
                "Vyberte akci", cancel, null,
                setAsCompleted, remove);
            if (action is not null && action != cancel)
            {
                if (action == setAsCompleted)
                {
                    viewModel.SetCompletedCommand.Execute(task);
                }
                else if (action == remove)
                {
                    viewModel.RemoveCommand.Execute(task);
                }
            }
            isLongPress = false;
        }
    }
}