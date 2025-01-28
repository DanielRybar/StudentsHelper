using StudentsHelper.Views;

namespace StudentsHelper.Helpers
{
    public static class Modules
    {
        public static readonly Dictionary<string, string> ModulesDictionary = new()
        {
            {"Poznámky", "notes"},
            {"Úkoly", "tasks"},
            {"Nastavení", "settings"},
            {"O aplikaci", "about"}
        };
    }
}