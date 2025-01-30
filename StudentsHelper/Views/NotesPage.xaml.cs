using StudentsHelper.Models;
using StudentsHelper.ViewModels;
using StudentsHelper.Views.NotesOperationsPages;

namespace StudentsHelper.Views;

public partial class NotesPage : ContentPage
{
    private readonly NotesViewModel viewModel;
    private static double scrollY = 0;
    private static bool isLongPress = false;
    public NotesPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new NotesViewModel();
        viewModel.NotesCountChanged += CheckToolbarItems;
    }

    private void CheckToolbarItems(int count)
    {
        MainScrollView.ScrollToAsync(0, scrollY, false);
        MainThread.BeginInvokeOnMainThread(() =>
        {
            this.ToolbarItems.Clear();
            if (count > 0)
            {
                this.ToolbarItems.Add(new ToolbarItem()
                {
                    IconImageSource = App.Current!.Resources["SortIcon"] as FontImageSource,
                    Command = new Command(async () =>
                    {
                        string cancel = "Zrušit";
                        string name = "Dle názvu (" + (viewModel.IsSortedByTitleAsc ? "sestupnì" : "vzestupnì") + ")";
                        string date = "Dle data (" + (viewModel.IsSortedByDateAsc ? "sestupnì" : "vzestupnì") + ")";
                        string action = await DisplayActionSheet(
                            "Øadit poznámky", cancel, null,
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
                    await Shell.Current.GoToAsync(nameof(AddNotePage));
                    await this.MainScrollView.ScrollToAsync(0, 0, false);
                })
            });
        });
    }

    private async void LongPress_RemoveItem(object sender, CommunityToolkit.Maui.Core.LongPressCompletedEventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is NoteItem note)
        {
            isLongPress = true;
            scrollY = MainScrollView.ScrollY;
            if (await DisplayAlert("Potvrzení", "Opravdu chcete odstranit tuto položku?", "Ano", "Ne"))
                viewModel.RemoveCommand.Execute(note);
            isLongPress = false;
        }
    }

    private async void Grid_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Grid grid && !isLongPress)
        {
            await Task.Delay(100);
            await grid.ScaleTo(0.8, 100);
            await grid.ScaleTo(1, 100);
        }
    }
}