using System.Globalization;

namespace Draftor.Converters;

public class TotalToIntConverter : IValueConverter
{
    public TotalToIntConverter()
    {
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double a)
        {
            if (a > 0)
                return 1;
            if (a < 0)
                return -1;
            return 0;
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}