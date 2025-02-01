using CommunityToolkit.Mvvm.Messaging.Messages;

namespace StudentsHelper.Models.Messages
{
    public class UpdatePendingTasksMessage(string value) : ValueChangedMessage<string>(value)
    {
    }
}