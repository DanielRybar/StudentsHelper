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

        private async Task Init()
        {
            if (database is not null) return;
            database = new SQLiteAsyncConnection(dbPath, flags);
            await database.CreateTableAsync<NoteItem>();
        }
        public async Task<List<NoteItem>> GetNoteItemsAsync()
        {
            await Init();
            return await database.Table<NoteItem>().ToListAsync();
        }

        public async Task<NoteItem> GetNoteItemAsync(int id)
        {
            await Init();
            return await database.Table<NoteItem>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteNoteItemAsync(NoteItem noteItem)
        {
            await Init();
            return await database.DeleteAsync(noteItem);
        }

        public async Task<int> StoreNoteItemAsync(NoteItem noteItem)
        {
            await Init();
            if (noteItem.Id != 0)
            {
                return await database.UpdateAsync(noteItem);
            }
            else
            {
                return await database.InsertAsync(noteItem);
            }
        }
    }
}