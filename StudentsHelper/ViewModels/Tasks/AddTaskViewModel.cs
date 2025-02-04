using StudentsHelper.Interfaces;
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
        private ObservableCollection<string> photos = new();
        private string selectedPhoto = string.Empty;
        private string carouselStatus = string.Empty;
        #endregion

        #region services
        private readonly ITasksManager tasksManager = DependencyService.Get<ITasksManager>();
        #endregion

        #region constructor
        public AddTaskViewModel()
        {
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
                                string localPath = Path.Combine(FileSystem.AppDataDirectory, uniqueFileName);
                                using var stream = await result.OpenReadAsync();
                                using var newStream = File.OpenWrite(localPath);
                                await stream.CopyToAsync(newStream);
                                Photos.Add(localPath);
                                SelectedPhoto = localPath;
                                RecalculateCarouselStatus();
                            }
                        }
                    }
                }
            );
        }
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
            }
        }
        public string CarouselStatus
        {
            get => carouselStatus;
            set => SetProperty(ref carouselStatus, value);
        }
        #endregion

        #region methods
        private void RecalculateCarouselStatus()
        {
            CarouselStatus = (Photos.IndexOf(SelectedPhoto) + 1) + "/" + Photos.Count;
        }
        #endregion
    }
}