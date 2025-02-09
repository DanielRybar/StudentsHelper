using System.Globalization;

namespace StudentsHelper.Converters
{
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string str && parameter is bool neg)
            {
                return neg ^ string.IsNullOrEmpty(str);
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}