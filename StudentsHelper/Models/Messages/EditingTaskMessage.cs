using CommunityToolkit.Mvvm.Messaging.Messages;

namespace StudentsHelper.Models.Messages
{
    public class EditingTaskMessage(TaskItem value) : ValueChangedMessage<TaskItem>(value)
    {
    }
}