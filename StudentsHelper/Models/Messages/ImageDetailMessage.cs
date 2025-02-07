using CommunityToolkit.Mvvm.Messaging.Messages;
using StudentsHelper.Models.MessageModels;

namespace StudentsHelper.Models.Messages
{
    public class ImageDetailMessage(PhotoModel model) : ValueChangedMessage<PhotoModel>(model)
    {
    }
}