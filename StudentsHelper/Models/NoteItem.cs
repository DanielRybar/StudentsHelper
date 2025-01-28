using SQLite;
using StudentsHelper.ViewModels.Abstract;

namespace StudentsHelper.Models
{
    public class NoteItem : BaseViewModel
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
}