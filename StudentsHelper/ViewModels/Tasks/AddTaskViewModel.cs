using StudentsHelper.Interfaces;
using StudentsHelper.ViewModels.Abstract;
using System.Windows.Input;

namespace StudentsHelper.ViewModels.Tasks
{
    public class AddTaskViewModel : BaseViewModel
    {
        #region variables
        private string title = string.Empty;
        private string description = string.Empty;
        private DateTime dueDate = DateTime.Now;
        #endregion

        #region services
        private readonly ITasksManager tasksManager = DependencyService.Get<ITasksManager>();
        #endregion

        #region constructor
        public AddTaskViewModel()
        {

        }
        #endregion

        #region commands
        public ICommand AddTaskCommand { get; private set; }
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
        #endregion
    }
}