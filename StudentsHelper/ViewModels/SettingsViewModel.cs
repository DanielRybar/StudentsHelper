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
        private List<string> shakerChoicesList = [.. SimpleChoices.ChoicesDictionary.Keys];
        private string selectedShakerChoice;
        private List<string> visibilityChoicesList = [.. UpdateButtonVisibilityChoices.ChoicesDictionary.Keys];
        private string selectedVisibilityChoice;
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

            var selectedShakerChoice = localStorage.Load(LocalStorageKeys.SHAKE_DETECTOR);
            if (!string.IsNullOrEmpty(selectedShakerChoice))
            {
                SelectedShakerChoice = SimpleChoices.ChoicesDictionary.FirstOrDefault(x => x.Value == selectedShakerChoice).Key;
            }
            else
            {
                SelectedShakerChoice = SimpleChoices.ChoicesDictionary.FirstOrDefault().Key;
                localStorage.Save(LocalStorageKeys.SHAKE_DETECTOR, SelectedShakerChoice);
            }

            var selectedVisibilityChoice = localStorage.Load(LocalStorageKeys.UPDATE_BUTTON);
            if (!string.IsNullOrEmpty(selectedVisibilityChoice))
            {
                SelectedVisibilityChoice = UpdateButtonVisibilityChoices.ChoicesDictionary.FirstOrDefault(x => x.Value == selectedVisibilityChoice).Key;
            }
            else
            {
                SelectedVisibilityChoice = UpdateButtonVisibilityChoices.ChoicesDictionary.FirstOrDefault().Key;
                localStorage.Save(LocalStorageKeys.UPDATE_BUTTON, SelectedVisibilityChoice);
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
                if (!string.IsNullOrEmpty(value))
                {
                    SetProperty(ref selectedTheme, value);
                    Themes.ApplyTheme(value);
                    localStorage.Save(LocalStorageKeys.APP_THEME, Themes.ThemesDictionary[value]);
                }
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
                if (!string.IsNullOrEmpty(value))
                {
                    SetProperty(ref selectedModule, value);
                    localStorage.Save(LocalStorageKeys.DEFAULT_MODULE, Modules.ModulesDictionary[value]);
                }
            }
        }

        public List<string> ShakerChoicesList
        {
            get => shakerChoicesList;
            set => SetProperty(ref shakerChoicesList, value);
        }
        public string SelectedShakerChoice
        {
            get => selectedShakerChoice;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    SetProperty(ref selectedShakerChoice, value);
                    localStorage.Save(LocalStorageKeys.SHAKE_DETECTOR, SimpleChoices.ChoicesDictionary[value]);
                }
            }
        }

        public List<string> VisibilityChoicesList
        {
            get => visibilityChoicesList;
            set => SetProperty(ref visibilityChoicesList, value);
        }
        public string SelectedVisibilityChoice
        {
            get => selectedVisibilityChoice;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    SetProperty(ref selectedVisibilityChoice, value);
                    localStorage.Save(LocalStorageKeys.UPDATE_BUTTON, UpdateButtonVisibilityChoices.ChoicesDictionary[value]);
                }
            }
        }
        #endregion
    }
}