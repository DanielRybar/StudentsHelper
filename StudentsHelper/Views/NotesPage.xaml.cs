using StudentsHelper.Models;
using StudentsHelper.ViewModels;

namespace StudentsHelper.Views;

public partial class NotesPage : ContentPage
{
    private readonly NotesViewModel viewModel;
    public NotesPage()
    {
        InitializeComponent();
        BindingContext = viewModel = new NotesViewModel();
        viewModel.NotesCountChanged += CheckToolbarItems;
    }

    private void CheckToolbarItems(int count)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            this.ToolbarItems.Clear();
            if (count > 0)
            {
                this.ToolbarItems.Add(new ToolbarItem()
                {
                    IconImageSource = App.Current!.Resources["SortIcon"] as FontImageSource,
                    Command = viewModel.SortCommand
                });
            }
        });
    }

    private async void LongPress_RemoveItem(object sender, CommunityToolkit.Maui.Core.LongPressCompletedEventArgs e)
    {
        if (sender is Grid grid && grid.BindingContext is NoteItem note)
        {
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