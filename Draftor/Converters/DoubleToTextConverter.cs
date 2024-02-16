using System.Globalization;

namespace Draftor.Converters;

public class DoubleToTextConverter : IValueConverter
{
    public DoubleToTextConverter()
    {
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double a)
        {
            return a.ToString();
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if(value is not string text)
            return value;
        if (text.EndsWith('.'))
            text = text.Substring(0, text.Length - 1);
        if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
        {
            return result;
        }
        return value;
    }
}