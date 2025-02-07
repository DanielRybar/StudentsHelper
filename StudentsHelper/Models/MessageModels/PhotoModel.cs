namespace StudentsHelper.Models.MessageModels
{
    public class PhotoModel(List<string> photos, string photo)
    {
        public List<string>? Photos { get; } = photos;
        public string Photo { get; } = photo;
    }
}