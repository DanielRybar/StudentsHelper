using CommunityToolkit.Maui.PlatformConfiguration.AndroidSpecific;
using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Constants;
using StudentsHelper.Helpers;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Tasks;

namespace StudentsHelper.Views.Tasks;

public partial class ActiveTasksPage : ContentPage
{
    private readonly ActiveTasksViewModel viewModel;
    private readonly IShakeDetector shakeDetector = DependencyService.Get<IShakeDetector>();
    private readonly ILocalStorage localStorage = DependencyService.Get<ILocalStorage>();
    private static double scrollY = 0;
    private static bool isLongPress = false;
    private static bool isLoaded = false;
    private bool isItemClicked = false;

    public ActiveTasksPage()
    {
        InitializeComponent();
        On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetColor(Colors.Black);
        BindingContext = viewModel = new ActiveTasksViewModel();
        viewModel.TasksCountChanged += CheckToolbarItems;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isLoaded)
        {
            WeakReferenceMessenger.Default.Send(new UpdatePendingTasksMessage(MessageValues.COLLECTION_MODIFIED));
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
        if (viewModel.PendingTasks.Count > 0)
        {
            Vibration.Default.Vibrate();
            if (await DisplayAlert("Potvrzení", "Opravdu chcete odstranit všechny aktivní úkoly?", "Ano", "Ne"))
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
            if (count >= 2)
            {
                this.ToolbarItems.Add(new ToolbarItem()
                {
                    IconImageSource = App.Current!.Resources["SortIcon"] as FontImageSource,
                    Command = new Command(async () =>
                    {
                        string cancel = "Zrušit";
                        string name = "Dle názvu (" + (viewModel.IsSortedByTitleAsc ? "sestupně" : "vzestupně") + ")";
                        string date = "Dle data splnění (" + (viewModel.IsSortedByDateDueAsc ? "sestupně" : "vzestupně") + ")";
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
            this.ToolbarItems.Add(new ToolbarItem()
            {
                IconImageSource = App.Current!.Resources["AddIcon"] as FontImageSource,
                Order = ToolbarItemOrder.Primary,
                Command = new Command(async () =>
                {
                    if (!isItemClicked)
                    {
                        isItemClicked = true;
                        await Shell.Current.GoToAsync(nameof(AddTaskPage));
                        await this.MainScrollView.ScrollToAsync(0, 0, false);
                        isItemClicked = false;
                    }
                })
            });

            // hack to prevent incorrect caching strategy
            this.MainCollectionView.ItemsSource = null;
            this.MainCollectionView.ItemsSource = viewModel.PendingTasks;
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
        if (sender is Grid grid && grid.BindingContext is TaskItem task && !isLongPress && !isItemClicked)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            isLongPress = true;
            scrollY = MainScrollView.ScrollY;

            string cancel = "Zrušit";
            string setAsCompleted = "Označit úkol jako dokončený";
            string edit = "Upravit úkol";
            string remove = "Smazat úkol";
            string action = await DisplayActionSheet(
                "Vyberte akci", cancel, null,
                setAsCompleted, edit, remove);
            if (action is not null && action != cancel)
            {
                if (action == setAsCompleted)
                {
                    viewModel.SetCompletedCommand.Execute(task);
                }
                else if (action == edit)
                {
                    await Shell.Current.GoToAsync(nameof(EditTaskPage));
                    WeakReferenceMessenger.Default.Send(new EditingTaskMessage((grid.BindingContext as TaskItem)!));
                }
                else if (action == remove)
                {
                    viewModel.RemoveCommand.Execute(task);
                }
            }
            isLongPress = false;
        }
    }

    private async void Grid_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Grid grid && !isLongPress && !isItemClicked)
        {
            isItemClicked = true;
            await (grid.Children[0] as Grid)!.ApplyScaleClickAnimation(0.8);
            await Shell.Current.GoToAsync(nameof(DetailTaskPage));
            WeakReferenceMessenger.Default.Send(new DetailTaskMessage((grid.BindingContext as TaskItem)!));
            isItemClicked = false;
            await (grid.Children[0] as Grid)!.ResetAnimation();
        }
    }

    protected override bool OnBackButtonPressed() => true;
}