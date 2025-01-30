using CommunityToolkit.Mvvm.Messaging.Messages;

namespace StudentsHelper.Models.Messages
{
    public class UpdateNotesMessage(string value) : ValueChangedMessage<string>(value)
    {
    }
}
