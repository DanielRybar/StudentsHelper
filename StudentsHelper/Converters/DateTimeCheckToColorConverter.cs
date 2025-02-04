using System.Globalization;

namespace StudentsHelper.Converters
{
    public class DateTimeCheckToColorConverter : IValueConverter
    {
        public Color? ValidColor { get; set; }
        public Color? InvalidColor { get; set; }

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                if (date < DateTime.Now)
                {
                    return InvalidColor;
                }
                return ValidColor;
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
