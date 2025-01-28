using StudentsHelper.Interfaces;

namespace StudentsHelper.Services
{
    public class LocalStorage : ILocalStorage
    {
        public void Delete(string key) => Preferences.Remove(key);
        public void DeleteAll() => Preferences.Clear();
        public string Load(string key) => Preferences.Get(key, string.Empty);
        public void Save(string key, string value) => Preferences.Set(key, value);
    }
}