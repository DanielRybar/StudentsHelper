using Android.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using StudentsHelper.Controls;

namespace StudentsHelper.Platforms.Android.Handlers
{
    public class ExtendedDatePickerHandler : DatePickerHandler
    {
        private new readonly static IPropertyMapper<ExtendedDatePicker, ExtendedDatePickerHandler> Mapper
           = new PropertyMapper<ExtendedDatePicker, ExtendedDatePickerHandler>(DatePickerHandler.Mapper)
           {
               [nameof(ExtendedDatePicker.UnderlineColor)] = MapUnderlineColor,
           };

        public ExtendedDatePickerHandler() : base(Mapper)
        {
        }

        private static void MapUnderlineColor(ExtendedDatePickerHandler handler, ExtendedDatePicker picker)
        {
            if (!CanBeUpdated(handler)) return;
            if (handler.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText editText)
            {
#pragma warning disable CA1422 // Platform compatibility validated in manifest file
                editText.Background?.Mutate().SetColorFilter(picker.UnderlineColor.ToPlatform(), PorterDuff.Mode.SrcIn!);
#pragma warning restore CA1422 // Platform compatibility validated in manifest file
            }
        }

        private static bool CanBeUpdated(ExtendedDatePickerHandler handler)
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