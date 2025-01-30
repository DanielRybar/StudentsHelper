using SQLite;
using StudentsHelper.Interfaces;
using StudentsHelper.Models;

namespace StudentsHelper.Services
{
    public class NotesManager : INotesManager
    {
        private SQLiteAsyncConnection database;

        private readonly string dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "StudentsHelper-Notes.sqlite");
        private readonly SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;

        private async Task<bool> Init()
        {
            if (database is not null) return true;
            try
            {
                database = new SQLiteAsyncConnection(dbPath, flags);
                await database.CreateTableAsync<NoteItem>();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<NoteItem>> GetNoteItemsAsync()
        {
            if (await Init())
            {
                return await database.Table<NoteItem>().ToListAsync();
            }
            return [];
        }

        public async Task<NoteItem> GetNoteItemAsync(int id)
        {
            if (await Init())
            {
                return await database.Table<NoteItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
            }
            return new();
        }

        public async Task<int> DeleteNoteItemAsync(NoteItem noteItem)
        {
            if (await Init())
            {
                return await database.DeleteAsync(noteItem);
            }
            return -1;
        }

        public async Task<int> StoreNoteItemAsync(NoteItem noteItem)
        {
            if (await Init())
            {
                if (noteItem.Id != 0)
                {
                    return await database.UpdateAsync(noteItem);
                }
                else
                {
                    return await database.InsertAsync(noteItem);
                }
            }
            return -1;
        }
    }
}