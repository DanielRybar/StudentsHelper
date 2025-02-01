using CommunityToolkit.Mvvm.Messaging.Messages;

namespace StudentsHelper.Models.Messages
{
    public class UpdateCompletedTasksMessage(string value) : ValueChangedMessage<string>(value)
    {
    }
}
