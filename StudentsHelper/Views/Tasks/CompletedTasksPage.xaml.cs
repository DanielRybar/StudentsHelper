using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Constants;
using StudentsHelper.Helpers;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Tasks;

namespace StudentsHelper.Views.Tasks;

public partial class CompletedTasksPage : ContentPage
{
    private readonly CompletedTasksViewModel viewModel;
    private readonly IShakeDetector shakeDetector = DependencyService.Get<IShakeDetector>();
    private readonly ILocalStorage localStorage = DependencyService.Get<ILocalStorage>();
    private static double scrollY = 0;
    private static bool isLongPress = false;
    private static bool isLoaded = false;
    private bool isItemClicked = false;

    public CompletedTasksPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new CompletedTasksViewModel();
        viewModel.TasksCountChanged += CheckToolbarItems;
        viewModel.UpdatePage += () => isLoaded = false;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isLoaded)
        {
            WeakReferenceMessenger.Default.Send(new UpdateCompletedTasksMessage(MessageValues.UPDATE_FROM_VIEWMODEL));
            isLoaded = true;
        }
        var visibilityChoice = localStorage.Load(LocalStorageKeys.UPDATE_BUTTON);
        if (!string.IsNullOrEmpty(visibilityChoice))
        {
            RefreshButton.IsVisible = visibilityChoice == UpdateButtonVisibilityChoices.ChoicesDictionary.First().Value;
        }
        shakeDetector.OnShaken += OnShakeDetected;
        shakeDetector.Start();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        shakeDetector.OnShaken -= OnShakeDetected;
        shakeDetector.Stop();
    }

    private async void OnShakeDetected()
    {
        if (viewModel.CompletedTasks.Count > 0)
        {
            Vibration.Default.Vibrate();
            if (await DisplayAlert("Potvrzení", "Opravdu chcete odstranit všechny dokončené úkoly?", "Ano", "Ne"))
            {
                viewModel.RemoveAllCommand.Execute(null);
                scrollY = 0;
            }
        }
    }

    private void CheckToolbarItems(int count)
    {
        MainScrollView.ScrollToAsync(0, scrollY, false);
        MainThread.BeginInvokeOnMainThread(() =>
        {
            CheckEmptyView(count);
            this.ToolbarItems.Clear();
            if (count > 0)
            {
                this.ToolbarItems.Add(new ToolbarItem()
                {
                    IconImageSource = App.Current!.Resources["SortIcon"] as FontImageSource,
                    Command = new Command(async () =>
                    {
                        string cancel = "Zrušit";
                        string name = "Dle názvu (" + (viewModel.IsSortedByTitleAsc ? "sestupně" : "vzestupně") + ")";
                        string date = "Dle data vytvoření (" + (viewModel.IsSortedByDateCreatedAsc ? "sestupně" : "vzestupně") + ")";
                        string photosCount = "Dle počtu fotografií (" + (viewModel.IsSortedByPhotosCountAsc ? "sestupně" : "vzestupně") + ")";
                        string action = await DisplayActionSheet(
                            "Řadit úkoly", cancel, null,
                            name, date, photosCount);

                        if (action is not null && action != cancel)
                        {
                            await this.MainScrollView.ScrollToAsync(0, 0, false);
                            if (action == name)
                            {
                                viewModel.SortCommand.Execute(TaskSortOption.ByTitle);
                            }
                            else if (action == date)
                            {
                                viewModel.SortCommand.Execute(TaskSortOption.ByDateDue);
                            }
                            else if (action == photosCount)
                            {
                                viewModel.SortCommand.Execute(TaskSortOption.ByPhotosCount);
                            }
                        }
                    })
                });
            }
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
            if (await DisplayAlert("Potvrzení", "Opravdu chcete odstranit tuto položku?", "Ano", "Ne"))
                viewModel.RemoveCommand.Execute(task);
            isLongPress = false;
        }
    }

    private async void Grid_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Grid grid && !isLongPress && !isItemClicked)
        {
            isItemClicked = true;
            await AnimateTile((grid.Children[0] as Grid)!);
            await Shell.Current.GoToAsync(nameof(DetailTaskPage));
            WeakReferenceMessenger.Default.Send(new DetailTaskMessage((grid.BindingContext as TaskItem)!));
            isItemClicked = false;
        }
    }

    private static async Task AnimateTile(Grid grid)
    {
        await Task.Delay(100);
        await grid.ScaleTo(0.8, 100);
        await grid.ScaleTo(1, 100);
    }

    protected override bool OnBackButtonPressed() => true;
}