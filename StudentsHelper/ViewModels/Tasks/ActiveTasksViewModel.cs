using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StudentsHelper.ViewModels.Tasks
{
    public class ActiveTasksViewModel : BaseViewModel
    {
        #region variables
        private ObservableCollection<TaskItem> pendingTasks = [];
        #endregion

        #region services
        private readonly ITasksManager tasksManager = DependencyService.Get<ITasksManager>();
        #endregion

        #region constructor
        public ActiveTasksViewModel()
        {
            WeakReferenceMessenger.Default.Register<UpdatePendingTasksMessage>(this, async (r, m) =>
            {
                await LoadTasks();
            });

            SetCompletedCommand = new Command(
                async (item) =>
                {
                    if (item is TaskItem task)
                    {
                        await tasksManager.FinishTaskItem(task.Id);
                        await LoadTasks();
                        WeakReferenceMessenger.Default.Send(new UpdateCompletedTasksMessage("Collection modified"));
                    }
                },
                (item) => item is not null
            );

            RemoveCommand = new Command(
                async (item) =>
                {
                    if (item is TaskItem task)
                    {
                        await tasksManager.DeleteTaskItemAsync(task);
                        await LoadTasks();
                    }
                },
                (item) => item is not null
            );

            RemoveAllCommand = new Command(
                async () =>
                {
                    await tasksManager.DeleteAllPendingTaskItemsAsync();
                    await LoadTasks();
                }
            );

            SortCommand = new Command(
                async (option) =>
                {
                    if (option is TaskSortOption op)
                    {
                        IsBusy = true;
                        await Task.Delay(100);

                        switch (op)
                        {
                            case TaskSortOption.ByTitle:
                                PendingTasks = IsSortedByTitleAsc
                                    ? new ObservableCollection<TaskItem>(PendingTasks.OrderByDescending(n => n.Title))
                                    : new ObservableCollection<TaskItem>(PendingTasks.OrderBy(n => n.Title));
                                IsSortedByTitleAsc = !IsSortedByTitleAsc;
                                break;
                            case TaskSortOption.ByDateDue:
                                PendingTasks = IsSortedByDateDueAsc
                                    ? new ObservableCollection<TaskItem>(PendingTasks.OrderByDescending(n => n.DateDue))
                                    : new ObservableCollection<TaskItem>(PendingTasks.OrderBy(n => n.DateDue));
                                IsSortedByDateDueAsc = !IsSortedByDateDueAsc;
                                break;
                            case TaskSortOption.ByPhotosCount:
                                PendingTasks = IsSortedByPhotosCountAsc
                                    ? new ObservableCollection<TaskItem>(PendingTasks.OrderByDescending(n => n.Photos.Count))
                                    : new ObservableCollection<TaskItem>(PendingTasks.OrderBy(n => n.Photos.Count));
                                IsSortedByPhotosCountAsc = !IsSortedByPhotosCountAsc;
                                break;
                        }
                        IsBusy = false;
                    }
                }
            );
        }
        #endregion

        #region commands
        public ICommand SetCompletedCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand RemoveAllCommand { get; private set; }
        public ICommand SortCommand { get; private set; }
        #endregion

        #region events
        public event Action<int> TasksCountChanged;
        #endregion

        #region properties
        public bool IsSortedByTitleAsc { get; private set; } = false;
        public bool IsSortedByDateDueAsc { get; private set; } = true;
        public bool IsSortedByPhotosCountAsc { get; private set; } = false;

        public ObservableCollection<TaskItem> PendingTasks
        {
            get => pendingTasks;
            set => SetProperty(ref pendingTasks, value);
        }
        #endregion

        #region methods
        private async Task LoadTasks()
        {
            IsBusy = true;
            var pendingTasks = await tasksManager.GetPendingTasksAsync();
            pendingTasks = [.. pendingTasks.OrderBy(t => t.DateDue).ThenByDescending(t => t.DateCreated)];
            PendingTasks.Clear();
            foreach (var task in pendingTasks)
            {
                PendingTasks.Add(task);
            }
            TasksCountChanged?.Invoke(PendingTasks.Count);
            IsBusy = false;
        }
        #endregion
    }
}