using System.Collections;
using System.Globalization;

namespace StudentsHelper.Converters
{
    public class IListToCountConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is IList list)
            {
                return list.Count;
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
