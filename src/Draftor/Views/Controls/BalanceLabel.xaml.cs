using CommunityToolkit.Maui.ImageSources;

namespace Draftor.Controls;

public partial class BalanceLabel : Label
{
    public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(nameof(Value), typeof(decimal), typeof(BalanceLabel), default(decimal));

    public decimal Value
    {
        get { return (decimal)GetValue(BalanceLabel.ValueProperty); }
        set { SetValue(BalanceLabel.ValueProperty, value); }
    }

    public BalanceLabel()
    {
        InitializeComponent();
    }
}