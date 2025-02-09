using StudentsHelper.Constants;
using StudentsHelper.Helpers;
using StudentsHelper.Interfaces;
using StudentsHelper.ViewModels.Abstract;

namespace StudentsHelper.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        #region variables
        private List<string> themesList = [.. Themes.ThemesDictionary.Keys];
        private string selectedTheme;
        private List<string> modulesList = [.. Modules.ModulesDictionary.Keys];
        private string selectedModule;
        private List<string> choicesList = [.. SimpleChoices.ChoicesDictionary.Keys];
        private string selectedChoice;
        #endregion

        #region services
        private readonly ILocalStorage localStorage = DependencyService.Get<ILocalStorage>();
        #endregion

        #region constructor
        public SettingsViewModel()
        {
            var selectedTheme = localStorage.Load(LocalStorageKeys.APP_THEME);
            if (!string.IsNullOrEmpty(selectedTheme))
            {
                SelectedTheme = Themes.ThemesDictionary.FirstOrDefault(x => x.Value == selectedTheme).Key;
            }
            else
            {
                SelectedTheme = Themes.ThemesDictionary.FirstOrDefault().Key;
                localStorage.Save(LocalStorageKeys.APP_THEME, SelectedTheme);
            }

            var selectedModule = localStorage.Load(LocalStorageKeys.DEFAULT_MODULE);
            if (!string.IsNullOrEmpty(selectedModule))
            {
                SelectedModule = Modules.ModulesDictionary.FirstOrDefault(x => x.Value == selectedModule).Key;
            }
            else
            {
                SelectedModule = Modules.ModulesDictionary.FirstOrDefault().Key;
                localStorage.Save(LocalStorageKeys.DEFAULT_MODULE, SelectedModule);
            }

            var selectedChoice = localStorage.Load(LocalStorageKeys.SHAKE_DETECTOR);
            if (!string.IsNullOrEmpty(selectedChoice))
            {
                SelectedChoice = SimpleChoices.ChoicesDictionary.FirstOrDefault(x => x.Value == selectedChoice).Key;
            }
            else
            {
                SelectedChoice = SimpleChoices.ChoicesDictionary.FirstOrDefault().Key;
                localStorage.Save(LocalStorageKeys.SHAKE_DETECTOR, SelectedChoice);
            }

        }
        #endregion

        #region properties
        public List<string> ThemesList
        {
            get => themesList;
            set => SetProperty(ref themesList, value);
        }

        public string SelectedTheme
        {
            get => selectedTheme;
            set
            {
                SetProperty(ref selectedTheme, value);
                Themes.ApplyTheme(value);
                localStorage.Save(LocalStorageKeys.APP_THEME, Themes.ThemesDictionary[value]);
            }
        }

        public List<string> ModulesList
        {
            get => modulesList;
            set => SetProperty(ref modulesList, value);
        }

        public string SelectedModule
        {
            get => selectedModule;
            set
            {
                SetProperty(ref selectedModule, value);
                localStorage.Save(LocalStorageKeys.DEFAULT_MODULE, Modules.ModulesDictionary[value]);
            }
        }

        public List<string> ChoicesList
        {
            get => choicesList;
            set => SetProperty(ref choicesList, value);
        }
        public string SelectedChoice
        {
            get => selectedChoice;
            set
            {
                SetProperty(ref selectedChoice, value);
                localStorage.Save(LocalStorageKeys.SHAKE_DETECTOR, SimpleChoices.ChoicesDictionary[value]);
            }
        }
        #endregion
    }
}