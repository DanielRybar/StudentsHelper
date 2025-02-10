using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using System.Windows.Input;

namespace StudentsHelper.ViewModels.Tasks
{
    public class DetailTaskViewModel : BaseViewModel
    {
        #region variables
        private TaskItem taskItem;
        private string title = string.Empty;
        private string description = string.Empty;
        private DateTime dateCreated;
        private DateTime dateDue;
        private bool isCompleted;
        private List<string> photos = [];
        private string photosString = string.Empty;
        #endregion

        #region services
        private readonly ITasksManager tasksManager = DependencyService.Get<ITasksManager>();
        #endregion

        #region constructor
        public DetailTaskViewModel()
        {
            WeakReferenceMessenger.Default.Register<DetailTaskMessage>(this, (r, m) =>
            {
                if (m.Value is not null)
                {
                    taskItem = m.Value;
                    Title = taskItem.Title;
                    Description = taskItem.Description;
                    DateCreated = taskItem.DateCreated;
                    DateDue = taskItem.DateDue;
                    IsCompleted = taskItem.IsCompleted;
                    Photos = taskItem.Photos;
                    PhotosString = taskItem.PhotosString;
                }
            });

            RemoveCommand = new Command(
                async () =>
                {
                    IsBusy = true;
                    await tasksManager.DeleteTaskItemAsync(taskItem!);
                    await Shell.Current.GoToAsync("..");
                    WeakReferenceMessenger.Default.Send(new UpdatePendingTasksMessage("Collection modified"));
                    WeakReferenceMessenger.Default.Send(new UpdateCompletedTasksMessage("Collection modified"));
                },
                () => taskItem is not null
            );

            SetCompletedCommand = new Command(
                async () =>
                {
                    IsBusy = true;
                    await tasksManager.FinishTaskItem(taskItem!.Id);
                    await Shell.Current.GoToAsync("..");
                    WeakReferenceMessenger.Default.Send(new UpdatePendingTasksMessage("Collection modified"));
                    WeakReferenceMessenger.Default.Send(new UpdateCompletedTasksMessage("Collection modified"));
                },
                () => taskItem is not null && !taskItem.IsCompleted
            );
        }
        #endregion

        #region commands
        public ICommand RemoveCommand { get; private set; }
        public ICommand SetCompletedCommand { get; private set; }
        #endregion

        #region properties
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        public DateTime DateCreated
        {
            get => dateCreated;
            set => SetProperty(ref dateCreated, value);
        }
        public DateTime DateDue
        {
            get => dateDue;
            set => SetProperty(ref dateDue, value);
        }
        public bool IsCompleted
        {
            get => isCompleted;
            set => SetProperty(ref isCompleted, value);
        }
        public List<string> Photos
        {
            get => photos;
            set => SetProperty(ref photos, value);
        }
        public string PhotosString
        {
            get => photosString;
            set => SetProperty(ref photosString, value);
        }
        #endregion
    }
}