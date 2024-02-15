namespace Draftor.Controls;

public partial class BalanceLabel : Label
{
    public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(double), typeof(BalanceLabel), 0.0);

    public double Value
    {
        get { return (double)GetValue(BalanceLabel.ValueProperty); }
        set { SetValue(BalanceLabel.ValueProperty, value); }
    }

    public BalanceLabel()
    {
        InitializeComponent();
    }
}