using System.Collections;
using System.Globalization;

namespace StudentsHelper.Converters
{
    public class CollectionToCountConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is IEnumerable collection)
            {
                return collection.Cast<object>().Count();
            }
            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
