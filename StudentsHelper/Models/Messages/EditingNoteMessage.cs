using CommunityToolkit.Mvvm.Messaging.Messages;

namespace StudentsHelper.Models.Messages
{
    public class EditingNoteMessage(NoteItem value) : ValueChangedMessage<NoteItem>(value)
    {
    }
}