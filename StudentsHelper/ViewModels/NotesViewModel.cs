using StudentsHelper.Interfaces;
using StudentsHelper.ViewModels.Abstract;

namespace StudentsHelper.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        #region services
        private readonly INotesManager notesManager = DependencyService.Get<INotesManager>();
        #endregion

        #region constructor
        public NotesViewModel()
        {
            Add();

        }
        #endregion

        async void Add()
        {
            await notesManager.StoreNoteItemAsync(new Models.NoteItem
            {
                Title = "Test",
                Date = DateTime.Now
            });
            var items = await notesManager.GetNoteItemsAsync();
        }
    }
}
