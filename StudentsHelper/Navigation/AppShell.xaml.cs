using StudentsHelper.Constants;
using StudentsHelper.Helpers;
using StudentsHelper.Interfaces;
using StudentsHelper.Views.NotesOperationsPages;

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
        }
    }
}
