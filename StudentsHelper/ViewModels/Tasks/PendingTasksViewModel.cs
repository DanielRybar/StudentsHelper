using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StudentsHelper.ViewModels.Tasks
{
    public class PendingTasksViewModel : BaseViewModel
    {
        #region variables
        private ObservableCollection<TaskItem> pendingTasks = [];
        #endregion

        #region services
        private readonly ITasksManager tasksManager = DependencyService.Get<ITasksManager>();
        #endregion

        #region constructor
        public PendingTasksViewModel()
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
        }
        #endregion

        #region commands
        public ICommand SetCompletedCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        #endregion

        #region events
        public Action<int> TasksCountChanged;
        #endregion

        #region properties

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
            pendingTasks = [.. pendingTasks.OrderByDescending(t => t.DateDue)];
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