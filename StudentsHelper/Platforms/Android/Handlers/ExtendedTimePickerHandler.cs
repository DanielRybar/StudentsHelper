using Android.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using StudentsHelper.Controls;

namespace StudentsHelper.Platforms.Android.Handlers
{
    public class ExtendedTimePickerHandler : TimePickerHandler
    {
        private new readonly static IPropertyMapper<ExtendedTimePicker, ExtendedTimePickerHandler> Mapper
           = new PropertyMapper<ExtendedTimePicker, ExtendedTimePickerHandler>(TimePickerHandler.Mapper)
           {
               [nameof(ExtendedTimePicker.UnderlineColor)] = MapUnderlineColor,
           };

        public ExtendedTimePickerHandler() : base(Mapper)
        {
        }

        private static void MapUnderlineColor(ExtendedTimePickerHandler handler, ExtendedTimePicker picker)
        {
            if (!CanBeUpdated(handler)) return;
            if (handler.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText editText)
            {
#pragma warning disable CA1422 // Platform compatibility validated in manifest file
                editText.Background?.Mutate().SetColorFilter(picker.UnderlineColor.ToPlatform(), PorterDuff.Mode.SrcIn!);
#pragma warning restore CA1422 // Platform compatibility validated in manifest file
            }
        }

        private static bool CanBeUpdated(ExtendedTimePickerHandler handler)
        {
            try
            {
                return handler?.PlatformView is not null;
            }
            catch (ObjectDisposedException)
            {
                return false;
            }
        }
    }
}