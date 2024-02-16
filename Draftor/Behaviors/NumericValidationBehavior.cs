using System.Globalization;

namespace Draftor.Behaviors;

public class NumericValidationBehavior : Behavior<Entry>
{
    protected override void OnAttachedTo(Entry entry)
    {
        entry.TextChanged += OnEntryTextChanged;
        base.OnAttachedTo(entry);
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnEntryTextChanged;
        base.OnDetachingFrom(entry);
    }

    private static void OnEntryTextChanged(object? sender, TextChangedEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(args.NewTextValue) || sender is null)
        {
            return;
        }
        string properNumericText = args.NewTextValue.Trim();
        properNumericText = properNumericText.Replace(',', '.');
        properNumericText = TrimLeadingZeros(properNumericText);
        bool IsValid = IsEntryValid(properNumericText);
        if (!IsValid)
        {
            properNumericText = args.OldTextValue;
        }
        if (properNumericText != args.NewTextValue)
            ((Entry)sender).Text = properNumericText;
    }

    private static bool IsEntryValid(string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;
        if (!double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            return false;
        if (DoesHaxMaximumTwoDecimalPlaces(text) == false)
            return false;
        return true;
    }

    private static bool DoesHaxMaximumTwoDecimalPlaces(string text)
    {
        int i = text.IndexOf('.');
        if (i > 0 && i < (text.Length - 1) - 2)
            return false;
        return true;
    }

    private static string TrimLeadingZeros(string text)
    {
        if (text.Length <= 1)
            return text;
        int startIndex = 0;
        while (startIndex < text.Length && text[startIndex] == '0')
        {
            startIndex++;
        }
        if (startIndex < text.Length)
        {
            if (text[startIndex] == '.')
                startIndex--;
            return text.Substring(startIndex);
        }
        return "0";
    }
}