using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StudentsHelper.ViewModels
{
    public class NotesViewModel : BaseViewModel
    {
        #region variables
        private ObservableCollection<NoteItem> notes = [];
        private NoteItem selectedNote;
        #endregion

        #region services
        private readonly INotesManager notesManager = DependencyService.Get<INotesManager>();
        #endregion

        #region constructor
        public NotesViewModel()
        {
            Task.Run(LoadNotes);

            RemoveCommand = new Command(
                async (item) =>
                {
                    if (item is NoteItem note)
                    {
                        await notesManager.DeleteNoteItemAsync(note);
                        await LoadNotes();
                    }
                },
                (item) => item is not null
            );

            SortCommand = new Command(
                () =>
                {
                    // todo
                }
            );
        }
        #endregion

        #region commands
        public ICommand RemoveCommand { get; private set; }
        public ICommand SortCommand { get; private set; }
        #endregion

        #region events
        public Action<int> NotesCountChanged;
        #endregion

        #region properties
        public ObservableCollection<NoteItem> Notes
        {
            get => notes;
            set => SetProperty(ref notes, value);
        }

        public NoteItem SelectedNote
        {
            get => selectedNote;
            set => SetProperty(ref selectedNote, value);
        }
        #endregion

        #region methods
        private async Task LoadNotes()
        {
            var notes = await notesManager.GetNoteItemsAsync();
            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
            NotesCountChanged?.Invoke(Notes.Count);
        }
        #endregion
    }
}