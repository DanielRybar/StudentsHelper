using StudentsHelper.Models;

namespace StudentsHelper.Interfaces
{
    public interface INotesManager
    {
        Task<List<NoteItem>> GetNoteItemsAsync();
        Task<NoteItem> GetNoteItemAsync(int id);
        Task<int> StoreNoteItemAsync(NoteItem noteItem);
        Task<int> DeleteNoteItemAsync(NoteItem noteItem);
    }
}