using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using System.Windows.Input;

namespace StudentsHelper.ViewModels.Notes
{
    public class EditNoteViewModel : BaseViewModel
    {
        #region variables
        private string title = string.Empty;
        private string content = string.Empty;
        private NoteItem noteItem;
        #endregion

        #region services
        private readonly INotesManager notesManager = DependencyService.Get<INotesManager>();
        #endregion

        #region constructor
        public EditNoteViewModel()
        {
            WeakReferenceMessenger.Default.Register<EditingNoteMessage>(this, (r, m) =>
            {
                if (m.Value is not null)
                {
                    noteItem = m.Value;
                    Title = noteItem.Title;
                    Content = noteItem.Content;
                }
            });

            EditNoteCommand = new Command(
                async () =>
                {
                    var item = new NoteItem
                    {
                        Id = noteItem is not null ? noteItem.Id : 0,
                        Title = string.IsNullOrEmpty(Title) ? "Nová poznámka" : Title,
                        Content = Content
                    };
                    IsBusy = true;
                    await notesManager.StoreNoteItemAsync(item);
                    await Shell.Current.GoToAsync("..");
                    WeakReferenceMessenger.Default.Send(new UpdateNotesMessage("Collection modified"));
                }
            );

            RemoveCommand = new Command(
                async () =>
                {
                    if (noteItem is not null)
                    {
                        IsBusy = true;
                        await notesManager.DeleteNoteItemAsync(noteItem);
                        await Shell.Current.GoToAsync("..");
                        WeakReferenceMessenger.Default.Send(new UpdateNotesMessage("Collection modified"));
                    }
                }
            );
        }
        #endregion

        #region commands
        public ICommand EditNoteCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        #endregion

        #region properties
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        public string Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }
        #endregion
    }
}