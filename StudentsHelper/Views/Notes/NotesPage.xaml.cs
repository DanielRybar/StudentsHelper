using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Notes;

namespace StudentsHelper.Views.Notes;

public partial class NotesPage : ContentPage
{
    private readonly NotesViewModel viewModel;
    private static double scrollY = 0;
    private static bool isLongPress = false;
    private static bool isLoaded = false;
    private bool isItemClicked = false;

    public NotesPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new NotesViewModel();
        viewModel.NotesCountChanged += CheckToolbarItems;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (!isLoaded)
        {
            WeakReferenceMessenger.Default.Send(new UpdateNotesMessage("Collection modified"));
            isLoaded = true;
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
                        string cancel = "Zru�it";
                        string name = "Dle n�zvu (" + (viewModel.IsSortedByTitleAsc ? "sestupn�" : "vzestupn�") + ")";
                        string date = "Dle data (" + (viewModel.IsSortedByDateAsc ? "sestupn�" : "vzestupn�") + ")";
                        string action = await DisplayActionSheet(
                            "�adit pozn�mky", cancel, null,
                            name, date);

                        if (action is not null && action != cancel)
                        {
                            await this.MainScrollView.ScrollToAsync(0, 0, false);
                            if (action == name)
                            {
                                viewModel.SortCommand.Execute(NoteSortOption.ByTitle);
                            }
                            else if (action == date)
                            {
                                viewModel.SortCommand.Execute(NoteSortOption.ByDate);
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
                        await Shell.Current.GoToAsync(nameof(AddNotePage));
                        await this.MainScrollView.ScrollToAsync(0, 0, false);
                        isItemClicked = false;
                    }
                })
            });
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

    private async void LongPress_RemoveItem(object sender, CommunityToolkit.Maui.Core.LongPressCompletedEventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is NoteItem note)
        {
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
            isLongPress = true;
            scrollY = MainScrollView.ScrollY;
            if (await DisplayAlert("Potvrzen�", "Opravdu chcete odstranit tuto polo�ku?", "Ano", "Ne"))
                viewModel.RemoveCommand.Execute(note);
            isLongPress = false;
        }
    }

    private async void Grid_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Grid grid && !isLongPress && !isItemClicked)
        {
            isItemClicked = true;
            await AnimateTile(grid);
            await Shell.Current.GoToAsync(nameof(EditNotePage));
            WeakReferenceMessenger.Default.Send(new EditingNoteMessage((grid.BindingContext as NoteItem)!));
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