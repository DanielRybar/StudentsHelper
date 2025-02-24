using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
                    await tasksManager.DeleteAllCompletedTaskItemsAsync();
                    await LoadTasks();
                }
            );

            SortCommand = new Command(
                async (option) =>
                {
                    if (option is TaskSortOption op)
                    {
                        IsBusy = true;
                        await Task.Delay(300);
                        List<TaskItem> items = [];
                        switch (op)
                        {
                            case TaskSortOption.ByTitle:
                                items = IsSortedByTitleAsc
                                    ? [.. CompletedTasks.OrderByDescending(n => n.Title)]
                                    : [.. CompletedTasks.OrderBy(n => n.Title)];
                                IsSortedByTitleAsc = !IsSortedByTitleAsc;
                                break;
                            case TaskSortOption.ByDateDue:
                                items = IsSortedByDateCreatedAsc
                                    ? [.. CompletedTasks.OrderByDescending(n => n.DateCreated)]
                                    : [.. CompletedTasks.OrderBy(n => n.DateCreated)];
                                IsSortedByDateCreatedAsc = !IsSortedByDateCreatedAsc;
                                break;
                            case TaskSortOption.ByPhotosCount:
                                items = IsSortedByPhotosCountAsc
                                    ? [.. CompletedTasks.OrderByDescending(n => n.Photos.Count)]
                                    : [.. CompletedTasks.OrderBy(n => n.Photos.Count)];
                                IsSortedByPhotosCountAsc = !IsSortedByPhotosCountAsc;
                                break;
                        }
                        CompletedTasks.Clear();
                        foreach (var item in items)
                        {
                            CompletedTasks.Add(item);
                        }
                        IsBusy = false;
                    }
                }
            );

            RefreshCommand = new Command(
                async () =>
                {
                    await LoadTasks();
                }
            );
        }
        #endregion

        #region commands
        public ICommand RemoveCommand { get; private set; }
        public ICommand RemoveAllCommand { get; private set; }
        public ICommand SortCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        #endregion

        #region events
        public event Action<int> TasksCountChanged;
        #endregion

        #region properties
        public bool IsSortedByTitleAsc { get; private set; } = false;
        public bool IsSortedByDateCreatedAsc { get; private set; } = true;
        public bool IsSortedByPhotosCountAsc { get; private set; } = false;

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
            completedTasks = [.. completedTasks.OrderBy(t => t.DateCreated)];
            CompletedTasks.Clear();
            foreach (var task in completedTasks)
            {
                CompletedTasks.Add(task);
            }
            TasksCountChanged?.Invoke(CompletedTasks.Count);
            InitializeSortingOptions();
            IsBusy = false;
        }

        private void InitializeSortingOptions()
        {
            IsSortedByTitleAsc = false;
            IsSortedByDateCreatedAsc = true;
            IsSortedByPhotosCountAsc = false;
        }
        #endregion
    }
}