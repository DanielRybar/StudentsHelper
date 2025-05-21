using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private readonly string defaultTitle = "Nový úkol";
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
                    if (Photos is not null)
                    {
                        foreach (var photo in Photos)
                        {
                            string originalFileName = Path.GetFileName(photo);
                            string destinationPath = Path.Combine(FileSystem.AppDataDirectory, originalFileName);
                            try
                            {
                                File.Copy(photo, destinationPath, true);
                                newPhotos.Add(destinationPath);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Unable to upload photo with path " + destinationPath);
                                Debug.WriteLine("Using photo from temporary storage...");
                                Debug.WriteLine(ex.Message);
                                newPhotos.Add(photo);
                            }
                        }
                    }
                    var task = new TaskItem
                    {
                        Title = string.IsNullOrEmpty(Title) ? defaultTitle : Title,
                        Description = Description,
                        DateDue = DueDate.Date + SelectedTime,
                        Photos = newPhotos
                    };
                    IsBusy = true;
                    await tasksManager.StoreTaskItemAsync(task);
                    await Shell.Current.GoToAsync("..");
                    WeakReferenceMessenger.Default.Send(new UpdatePendingTasksMessage(MessageValues.COLLECTION_MODIFIED));
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
                            if (result.ContentType == "image/jpeg" || result.ContentType == "image/png")
                            {
                                var options = new Android.Graphics.BitmapFactory.Options { InJustDecodeBounds = true };
                                Android.Graphics.BitmapFactory.DecodeFile(result.FullPath, options);
                                float ratioWidthToHeight = (float)options.OutWidth / (options.OutHeight != 0 ? options.OutHeight : 1);
                                float ratioHeightToWidth = (float)options.OutHeight / (options.OutWidth != 0 ? options.OutWidth : 1);
                                if (ratioWidthToHeight > 10 || ratioHeightToWidth > 10)
                                {
                                    await Toast.Make("Obrázek má extrémní poměr stran.").Show();
                                    return;
                                }
                                string uniqueFileName = $"{Guid.NewGuid()}_{result.FileName}";
                                string localPath = Path.Combine(FileSystem.CacheDirectory, uniqueFileName);
                                using var stream = await result.OpenReadAsync();
                                try
                                {
                                    using var newStream = File.OpenWrite(localPath);
                                    await stream.CopyToAsync(newStream);
                                    Photos.Add(localPath);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Unable to save photo with path " + localPath);
                                    Debug.WriteLine(ex.Message);
                                }
                            }
                            else
                            {
                                await Toast.Make("Obrázek má nepodporovaný formát.").Show();
                            }
                        }
                    }
                }
            );

            RemovePhotoCommand = new Command(
                (photo) =>
                {
                    if (photo is string photoPath)
                    {
                        Photos.Remove(photoPath);
                    }
                },
                (photo) => photo is not null && Photos is not null
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
        #endregion
    }
}