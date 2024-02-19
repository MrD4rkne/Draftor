using System.Globalization;

namespace Draftor.Converters;

public class DecimalToTextConverter : IValueConverter
{
    public DecimalToTextConverter()
    {
    }

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is decimal a)
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
        if (decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
        {
            return result;
        }
        return value;
    }
}