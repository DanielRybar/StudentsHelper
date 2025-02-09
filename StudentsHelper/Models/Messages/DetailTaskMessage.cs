using CommunityToolkit.Mvvm.Messaging.Messages;

namespace StudentsHelper.Models.Messages
{
    public class DetailTaskMessage(TaskItem value) : ValueChangedMessage<TaskItem>(value)
    {
    }
}
