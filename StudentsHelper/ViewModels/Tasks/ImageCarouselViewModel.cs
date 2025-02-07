using CommunityToolkit.Mvvm.Messaging;
using StudentsHelper.Models.MessageModels;
using StudentsHelper.Models.Messages;
using StudentsHelper.ViewModels.Abstract;

namespace StudentsHelper.ViewModels.Tasks
{
    public class ImageCarouselViewModel : BaseViewModel
    {
        #region variables
        private string selectedPhoto;
        private List<string> photos;
        private string carouselStatus = string.Empty;
        #endregion

        #region constructor
        public ImageCarouselViewModel()
        {
            WeakReferenceMessenger.Default.Register<ImageDetailMessage>(this, (r, m) =>
            {
                if (m.Value is not null && m.Value is PhotoModel pm)
                {
                    Photos = pm.Photos!;
                    SelectedPhoto = pm.Photo;
                }
            });
        }
        #endregion

        #region properties
        public string SelectedPhoto
        {
            get => selectedPhoto;
            set 
            {
                SetProperty(ref selectedPhoto, value);
                RecalculateCarouselStatus();
            }
        }
        public List<string> Photos
        {
            get => photos;
            set => SetProperty(ref photos, value);
        }
        public string CarouselStatus
        {
            get => carouselStatus;
            set => SetProperty(ref carouselStatus, value);
        }
        #endregion

        #region methods
        private void RecalculateCarouselStatus()
        {
            var count = Photos.Count;
            CarouselStatus = (Photos.IndexOf(SelectedPhoto) + 1) + "/" + count;
        }
        #endregion
    }
}