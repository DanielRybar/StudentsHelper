using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using System.Windows.Input;

namespace StudentsHelper.ViewModels.Notes
{
    public class AddNoteViewModel : BaseViewModel
    {
        #region variables
        private string title = string.Empty;
        private string content = string.Empty;
        private readonly string defaultTitle = "Nová poznámka";
        #endregion

        #region services
        private readonly INotesManager notesManager = DependencyService.Get<INotesManager>();
        #endregion

        #region constructor
        public AddNoteViewModel()
        {
            AddNoteCommand = new Command(
                async () =>
                {
                    var item = new NoteItem
                    {
                        Title = string.IsNullOrEmpty(Title) ? defaultTitle : Title,
                        Content = Content
                    };
                    IsBusy = true;
                    await notesManager.StoreNoteItemAsync(item);
                    await Shell.Current.GoToAsync("..");
                    WeakReferenceMessenger.Default.Send(new UpdateNotesMessage(MessageValues.COLLECTION_MODIFIED));
                }
            );
        }
        #endregion

        #region commands
        public ICommand AddNoteCommand { get; private set; }
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