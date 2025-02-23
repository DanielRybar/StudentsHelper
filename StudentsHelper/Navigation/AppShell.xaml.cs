using StudentsHelper.Constants;
using StudentsHelper.Helpers;
using StudentsHelper.Interfaces;
using StudentsHelper.Views.Notes;
using StudentsHelper.Views.Tasks;

namespace StudentsHelper.Navigation
{
    public partial class AppShell : Shell
    {
        #region services
        private readonly ILocalStorage localStorage = DependencyService.Get<ILocalStorage>();
        #endregion

        public AppShell()
        {
            InitializeComponent();
            RegisterOtherRoutes();
        }

        private void RegisterOtherRoutes()
        {
            Routing.RegisterRoute(nameof(AddNotePage), typeof(AddNotePage));
            Routing.RegisterRoute(nameof(EditNotePage), typeof(EditNotePage));
            Routing.RegisterRoute(nameof(AddTaskPage), typeof(AddTaskPage));
            Routing.RegisterRoute(nameof(DetailTaskPage), typeof(DetailTaskPage));
            Routing.RegisterRoute(nameof(EditTaskPage), typeof(EditTaskPage));
            Routing.RegisterRoute(nameof(ImageCarouselPage), typeof(ImageCarouselPage));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var selectedTheme = localStorage.Load(LocalStorageKeys.APP_THEME);
            if (!string.IsNullOrEmpty(selectedTheme))
            {
                Themes.ApplyTheme(selectedTheme, isKey: true);
            }
            var selectedModule = localStorage.Load(LocalStorageKeys.DEFAULT_MODULE);
            if (!string.IsNullOrEmpty(selectedModule))
            {
                var module = Modules.ModulesDictionary.FirstOrDefault(x => x.Value == selectedModule).Key;
                CurrentItem = Items.FirstOrDefault(x => x.Title == module);
            }
            // initialize settings
            var selectedShakerOption = localStorage.Load(LocalStorageKeys.SHAKE_DETECTOR);
            if (string.IsNullOrEmpty(selectedShakerOption))
            {
                localStorage.Save(LocalStorageKeys.SHAKE_DETECTOR, SimpleChoices.ChoicesDictionary.First().Value);
            }
            var selectedUpdateButtonOption = localStorage.Load(LocalStorageKeys.UPDATE_BUTTON);
            if (string.IsNullOrEmpty(selectedUpdateButtonOption))
            {
                localStorage.Save(LocalStorageKeys.UPDATE_BUTTON, UpdateButtonVisibilityChoices.ChoicesDictionary.First().Value);
            }
        }
    }
}
