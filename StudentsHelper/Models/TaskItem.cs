using SQLite;
using StudentsHelper.ViewModels.Abstract;

namespace StudentsHelper.Models
{
    public class TaskItem : BaseViewModel
    {
        private int id;
        private string title = string.Empty;
        private string description = string.Empty;
        private DateTime dateCreated = DateTime.Now;
        private DateTime dateDue;
        private bool isCompleted = false;
        private string photosString = string.Empty;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

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

        public string PhotosString
        {
            get => photosString;
            set => SetProperty(ref photosString, value);
        }

        [Ignore]
        public List<string> Photos
        {
            get => string.IsNullOrEmpty(photosString) ? [] : [.. photosString.Split('|')];
            set => PhotosString = string.Join("|", value);
        }
    }
}