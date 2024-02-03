using System.Globalization;

namespace Draftor.Converters;

public class DoubleToTextConverter : IValueConverter
{
    public DoubleToTextConverter()
    {
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double a)
        {
            return value.ToString();
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double ret;
        return double.TryParse(value as string, out ret) ? ret : 0;
    }
}