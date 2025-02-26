using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StudentsHelper.ViewModels.Notes
{
    public class NotesViewModel : BaseViewModel
    {
        #region variables
        private ObservableCollection<NoteItem> notes = [];
        #endregion

        #region services
        private readonly INotesManager notesManager = DependencyService.Get<INotesManager>();
        #endregion

        #region constructor
        public NotesViewModel()
        {
            WeakReferenceMessenger.Default.Register<UpdateNotesMessage>(this, async (r, m) =>
            {
                await LoadNotes();
            });

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
                async (option) =>
                {
                    if (option is NoteSortOption op)
                    {
                        IsBusy = true;
                        await Task.Delay(300);
                        List<NoteItem> items = [];
                        switch (op)
                        {
                            case NoteSortOption.ByTitle:
                                items = IsSortedByTitleAsc
                                    ? [.. Notes.OrderByDescending(n => n.Title)]
                                    : [.. Notes.OrderBy(n => n.Title)];
                                IsSortedByTitleAsc = !IsSortedByTitleAsc;
                                break;
                            case NoteSortOption.ByDate:
                                items = IsSortedByDateAsc
                                    ? [.. Notes.OrderByDescending(n => n.Date)]
                                    : [.. Notes.OrderBy(n => n.Date)];
                                IsSortedByDateAsc = !IsSortedByDateAsc;
                                break;
                        }
                        Notes.Clear();
                        foreach (var item in items)
                        {
                            Notes.Add(item);
                        }
                        IsBusy = false;
                    }
                }
            );

            RefreshCommand = new Command(
                async () =>
                {
                    await LoadNotes();
                }
            );
        }
        #endregion

        #region commands
        public ICommand RemoveCommand { get; private set; }
        public ICommand SortCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        #endregion

        #region events
        public event Action<int> NotesCountChanged;
        #endregion

        #region properties
        public bool IsSortedByTitleAsc { get; private set; } = false;
        public bool IsSortedByDateAsc { get; private set; } = false;

        public ObservableCollection<NoteItem> Notes
        {
            get => notes;
            set => SetProperty(ref notes, value);
        }
        #endregion

        #region methods
        private async Task LoadNotes()
        {
            IsBusy = true;
            await Task.Delay(500);
            var notes = await notesManager.GetNoteItemsAsync();
            notes = [.. notes.OrderByDescending(n => n.Date)];
            Notes.Clear();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
            NotesCountChanged?.Invoke(Notes.Count);
            InitializeSortingOptions();
            IsBusy = false;
        }

        private void InitializeSortingOptions()
        {
            IsSortedByTitleAsc = false;
            IsSortedByDateAsc = false;
        }
        #endregion
    }
}