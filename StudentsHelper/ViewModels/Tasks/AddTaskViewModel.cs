using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StudentsHelper.ViewModels.Tasks
{
    public class AddTaskViewModel : BaseViewModel
    {
        #region variables
        private string title = string.Empty;
        private string description = string.Empty;
        private DateTime dueDate = DateTime.Now.AddDays(1);
        private TimeSpan selectedTime = new(23, 59, 0);
        private ObservableCollection<string> photos = [];
        private string selectedPhoto = string.Empty;
        private string carouselStatus = string.Empty;
        private bool isPhotoVisible = false;
        #endregion

        #region services
        private readonly ITasksManager tasksManager = DependencyService.Get<ITasksManager>();
        #endregion

        #region constructor
        public AddTaskViewModel()
        {
            AddTaskCommand = new Command(
                async () =>
                {
                    List<string> newPhotos = [];
                    if (Photos is not null && SelectedPhoto is not null)
                    {
                        foreach (var photo in Photos)
                        {
                            string originalFileName = Path.GetFileName(photo);
                            string destinationPath = Path.Combine(FileSystem.AppDataDirectory, originalFileName);
                            File.Copy(photo, destinationPath, true);
                            newPhotos.Add(destinationPath);
                        }
                    }
                    var task = new TaskItem
                    {
                        Title = string.IsNullOrEmpty(Title) ? "Nový úkol" : Title,
                        Description = Description,
                        DateDue = DueDate.Date + SelectedTime,
                        Photos = newPhotos
                    };
                    IsBusy = true;
                    await tasksManager.StoreTaskItemAsync(task);
                    await Shell.Current.GoToAsync("..");
                    WeakReferenceMessenger.Default.Send(new UpdatePendingTasksMessage("Collection modified"));
                }
            );

            AddPhotosCommand = new Command(
                async (mode) =>
                {
                    if (mode is bool camera)
                    {
                        FileResult? result;
                        if (camera && MediaPicker.Default.IsCaptureSupported)
                        {
                            result = await MediaPicker.CapturePhotoAsync();
                        }
                        else
                        {
                            result = await MediaPicker.PickPhotoAsync();
                        }
                        if (result is not null)
                        {
                            if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)
                            || result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                            {
                                string uniqueFileName = $"{Guid.NewGuid()}_{result.FileName}";
                                string localPath = Path.Combine(FileSystem.CacheDirectory, uniqueFileName);
                                using var stream = await result.OpenReadAsync();
                                using var newStream = File.OpenWrite(localPath);
                                await stream.CopyToAsync(newStream);
                                Photos.Add(localPath);
                                SelectedPhoto = localPath;
                            }
                        }
                    }
                }
            );

            RemovePhotoCommand = new Command(
                () =>
                {
                    Photos.Remove(SelectedPhoto);
                    SelectedPhoto = Photos.LastOrDefault();
                },
                () => SelectedPhoto is not null && Photos is not null
            );
        }
        #endregion

        #region events
        public Action PhotoChanged;
        #endregion

        #region commands
        public ICommand AddTaskCommand { get; private set; }
        public ICommand AddPhotosCommand { get; private set; }
        public ICommand RemovePhotoCommand { get; private set; }
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
        public DateTime DueDate
        {
            get => dueDate;
            set => SetProperty(ref dueDate, value);
        }
        public TimeSpan SelectedTime
        {
            get => selectedTime;
            set => SetProperty(ref selectedTime, value);
        }
        public ObservableCollection<string> Photos
        {
            get => photos;
            set => SetProperty(ref photos, value);
        }
        public string SelectedPhoto
        {
            get => selectedPhoto;
            set
            {
                SetProperty(ref selectedPhoto, value);
                RecalculateCarouselStatus();
                PhotoChanged?.Invoke();
            }
        }
        public string CarouselStatus
        {
            get => carouselStatus;
            set => SetProperty(ref carouselStatus, value);
        }
        public bool IsPhotoVisible
        {
            get => isPhotoVisible;
            set => SetProperty(ref isPhotoVisible, value);
        }
        #endregion

        #region methods
        private void RecalculateCarouselStatus()
        {
            var count = Photos.Count;
            IsPhotoVisible = count > 0;
            CarouselStatus = (Photos.IndexOf(SelectedPhoto) + 1) + "/" + count;
        }
        #endregion
    }
}