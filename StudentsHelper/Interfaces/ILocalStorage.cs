namespace StudentsHelper.Interfaces
{
    public interface ILocalStorage
    {
        void Save(string key, string value);
        string Load(string key);
        void Delete(string key);
        void DeleteAll();
    }
}
