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
                (option) =>
                {
                    if (option is NoteSortOption op)
                    {
                        switch (op)
                        {
                            case NoteSortOption.ByTitle:
                                Notes = IsSortedByTitleAsc 
                                    ? new ObservableCollection<NoteItem>(Notes.OrderByDescending(n => n.Title))
                                    : new ObservableCollection<NoteItem>(Notes.OrderBy(n => n.Title));
                                IsSortedByTitleAsc = !IsSortedByTitleAsc;
                                break;
                            case NoteSortOption.ByDate:
                                Notes = IsSortedByDateAsc
                                    ? new ObservableCollection<NoteItem>(Notes.OrderByDescending(n => n.Date))
                                    : new ObservableCollection<NoteItem>(Notes.OrderBy(n => n.Date));
                                IsSortedByDateAsc = !IsSortedByDateAsc;
                                break;
                        }
                    }
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
        public bool IsSortedByTitleAsc { get; private set; } = false;
        public bool IsSortedByDateAsc { get; private set; } = false;

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
            notes = [.. notes.OrderByDescending(n => n.Date)];
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