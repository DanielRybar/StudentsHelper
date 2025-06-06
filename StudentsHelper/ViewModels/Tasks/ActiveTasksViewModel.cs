﻿using CommunityToolkit.Mvvm.Messaging;
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
                        WeakReferenceMessenger.Default.Send(new UpdateCompletedTasksMessage(MessageValues.COLLECTION_MODIFIED));
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
                        await Task.Delay(300);
                        List<TaskItem> items = [];
                        switch (op)
                        {
                            case TaskSortOption.ByTitle:
                                items = IsSortedByTitleAsc
                                    ? [.. PendingTasks.OrderByDescending(n => n.Title)]
                                    : [.. PendingTasks.OrderBy(n => n.Title)];
                                IsSortedByTitleAsc = !IsSortedByTitleAsc;
                                break;
                            case TaskSortOption.ByDateDue:
                                items = IsSortedByDateDueAsc
                                    ? [.. PendingTasks.OrderByDescending(n => n.DateDue)]
                                    : [.. PendingTasks.OrderBy(n => n.DateDue)];
                                IsSortedByDateDueAsc = !IsSortedByDateDueAsc;
                                break;
                            case TaskSortOption.ByPhotosCount:
                                items = IsSortedByPhotosCountAsc
                                    ? [.. PendingTasks.OrderByDescending(n => n.Photos.Count)]
                                    : [.. PendingTasks.OrderBy(n => n.Photos.Count)];
                                IsSortedByPhotosCountAsc = !IsSortedByPhotosCountAsc;
                                break;
                        }
                        PendingTasks.Clear();
                        foreach (var item in items)
                        {
                            PendingTasks.Add(item);
                        }
                        TasksCountChanged?.Invoke(PendingTasks.Count);
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
        public ICommand SetCompletedCommand { get; private set; }
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
            await Task.Delay(500);
            var pendingTasks = await tasksManager.GetPendingTasksAsync();
            pendingTasks = [.. pendingTasks.OrderBy(t => t.DateDue).ThenByDescending(t => t.DateCreated)];
            PendingTasks.Clear();
            foreach (var task in pendingTasks)
            {
                PendingTasks.Add(task);
            }
            TasksCountChanged?.Invoke(PendingTasks.Count);
            InitializeSortingOptions();
            IsBusy = false;
        }

        private void InitializeSortingOptions()
        {
            IsSortedByTitleAsc = false;
            IsSortedByDateDueAsc = true;
            IsSortedByPhotosCountAsc = false;
        }
        #endregion
    }
}