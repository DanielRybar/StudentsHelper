using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using System.Collections.ObjectModel;

namespace StudentsHelper.ViewModels.Tasks
{
    public class CompletedTasksViewModel : BaseViewModel
    {
        #region variables
        private ObservableCollection<TaskItem> completedTasks = [];
        #endregion

        #region services
        private readonly ITasksManager tasksManager = DependencyService.Get<ITasksManager>();
        #endregion

        #region constructor
        public CompletedTasksViewModel()
        {
            WeakReferenceMessenger.Default.Register<UpdateCompletedTasksMessage>(this, async (r, m) =>
            {
                await LoadTasks();
            });
        }
        #endregion

        #region commands

        #endregion

        #region events
        public Action<int> TasksCountChanged;
        #endregion

        #region properties
        public ObservableCollection<TaskItem> CompletedTasks
        {
            get => completedTasks;
            set => SetProperty(ref completedTasks, value);
        }
        #endregion

        #region methods
        private async Task LoadTasks()
        {
            IsBusy = true;
            var completedTasks = await tasksManager.GetFinishedTasksAsync();
            completedTasks = [.. completedTasks.OrderByDescending(t => t.DateCreated)];
            CompletedTasks.Clear();
            foreach (var task in completedTasks)
            {
                CompletedTasks.Add(task);
            }
            TasksCountChanged?.Invoke(CompletedTasks.Count);
            IsBusy = false;
        }
        #endregion
    }
}