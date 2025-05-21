using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;
using StudentsHelper.Views.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace StudentsHelper.ViewModels.Tasks
{
    public class EditTaskViewModel : BaseViewModel
    {
        #region variables
        private string title = string.Empty;
        private string description = string.Empty;
        private DateTime dueDate = DateTime.Now.AddDays(1);
        private TimeSpan selectedTime = new(23, 59, 0);
        private ObservableCollection<string> photos = [];
        private TaskItem taskItem;
        private readonly string defaultTitle = "Nový úkol";
        #endregion

        #region services
        private readonly ITasksManager tasksManager = DependencyService.Get<ITasksManager>();
        #endregion

        #region constructor
        public EditTaskViewModel()
        {
            WeakReferenceMessenger.Default.Register<EditingTaskMessage>(this, (r, m) =>
            {
                if (m.Value is not null)
                {
                    taskItem = m.Value;
                    Title = taskItem.Title;
                    Description = taskItem.Description;
                    DueDate = taskItem.DateDue < DateTime.Now ? DateTime.Now.AddDays(1) : taskItem.DateDue;
                    SelectedTime = taskItem.DateDue.TimeOfDay;
                    Photos = [.. taskItem.Photos];
                }
            });

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

            EditTaskCommand = new Command(
                async () =>
                {
                    if (taskItem is not null && !taskItem.IsCompleted)
                    {
                        // delete old photos (that are deleted by user)
                        if (taskItem.Photos is not null)
                        {
                            foreach (var photo in taskItem.Photos)
                            {
                                if (Photos is not null && Photos.Contains(photo))
                                {
                                    continue;
                                }
                                try
                                {
                                    File.Delete(photo);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Failed to delete photo: " + photo);
                                    Debug.WriteLine(ex.Message);
                                }
                            }
                        }
                        // copy new photos from temporary storage
                        List<string> newPhotos = [];
                        if (Photos is not null)
                        {
                            foreach (var photo in Photos)
                            {
                                string originalFileName = Path.GetFileName(photo);
                                string destinationPath = Path.Combine(FileSystem.AppDataDirectory, originalFileName);
                                if (File.Exists(destinationPath))
                                {
                                    newPhotos.Add(photo);
                                    continue;
                                }
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
                        taskItem.Title = string.IsNullOrEmpty(Title) ? defaultTitle : Title;
                        taskItem.Description = Description;
                        taskItem.DateDue = DueDate.Date + SelectedTime;
                        taskItem.Photos = newPhotos;
                        IsBusy = true;
                        await tasksManager.StoreTaskItemAsync(taskItem);
                        await Shell.Current.GoToAsync("//" + nameof(ActiveTasksPage));
                        WeakReferenceMessenger.Default.Send(new UpdatePendingTasksMessage(MessageValues.COLLECTION_MODIFIED));
                    }
                }
            );
        }
        #endregion

        #region commands
        public ICommand AddPhotosCommand { get; private set; }
        public ICommand RemovePhotoCommand { get; private set; }
        public ICommand EditTaskCommand { get; private set; }
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
