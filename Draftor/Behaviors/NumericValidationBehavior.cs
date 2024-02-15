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
        string text = args.NewTextValue;
        bool IsValid = IsEntryValid(text);
        if (!IsValid)
        {
            ((Entry)sender).Text = args.OldTextValue;
        }
    }

    private static bool IsEntryValid(string text)
    {
        if (string.IsNullOrEmpty(text))
            return false;
        if (!double.TryParse(text, out _))
            return false;
        int i = text.IndexOf('.');
        if (i > 0 && i < text.Length - 1 - 2)
            return false;
        return true;
    }
}