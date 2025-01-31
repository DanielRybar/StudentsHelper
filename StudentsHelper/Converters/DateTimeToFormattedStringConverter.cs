using System.Globalization;

namespace StudentsHelper.Converters
{
    public class DateTimeToFormattedStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                if (date.Year == DateTime.Now.Year)
                {
                    return date.ToString("d. M.", CultureInfo.InvariantCulture);
                }
                return date.ToString("d. M. yyyy", CultureInfo.InvariantCulture);
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}