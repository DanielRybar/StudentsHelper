using System.Globalization;

namespace StudentsHelper.Converters
{
    public class TaskStatusToIconConverter : IValueConverter
    {
        public FontImageSource? CompletedIcon { get; set; }
        public FontImageSource? PendingIcon { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool state)
            {
                return state ? CompletedIcon : PendingIcon;
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}