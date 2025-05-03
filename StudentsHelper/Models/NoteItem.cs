using SQLite;
using StudentsHelper.Models.Abstract;

namespace StudentsHelper.Models
{
    public class NoteItem : BaseModel
    {
        private int id;
        private string title = string.Empty;
        private string content = string.Empty;
        private DateTime date = DateTime.Now;

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

        public string Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }

        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }
    }

    public enum NoteSortOption
    {
        ByTitle,
        ByDate
    }
}