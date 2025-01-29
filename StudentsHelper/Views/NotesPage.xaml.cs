using StudentsHelper.Models;
using StudentsHelper.ViewModels;

namespace StudentsHelper.Views;

public partial class NotesPage : ContentPage
{
    private readonly NotesViewModel viewModel;
    private static double scrollY = 0;
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
        });
    }

    private async void LongPress_RemoveItem(object sender, CommunityToolkit.Maui.Core.LongPressCompletedEventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is NoteItem note)
        {
            scrollY = MainScrollView.ScrollY;
            if (await DisplayAlert("Potvrzení", "Opravdu chcete odstranit tuto položku?", "Ano", "Ne"))
                viewModel.RemoveCommand.Execute(note);
        }
    }

    //private async void Grid_BindingContextChanged(object sender, EventArgs e)
    //{
    //    if (sender is Grid grid && grid.BindingContext != null)
    //    {
    //        int index = viewModel.Notes.IndexOf((grid.BindingContext as NoteItem)!);
    //        if (index >= 0)
    //        {
    //            await Task.Delay(index * 150);
    //            await grid.ScaleTo(1, 250, Easing.CubicOut);
    //            if (index == 0) grid.Scale = 1;
    //        }
    //    }
    //}
}