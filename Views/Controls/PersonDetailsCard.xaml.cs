namespace Draftor.Controls;

public partial class PersonDetailsCard : ContentView
{
    public static readonly BindableProperty NameProperty =
            BindableProperty.Create(nameof(Name), typeof(string), typeof(BalanceLabel), string.Empty);

    public string Name
    {
        get { return (string)GetValue(PersonDetailsCard.NameProperty); }
        set { SetValue(PersonDetailsCard.NameProperty, value); }
    }

    public static readonly BindableProperty DescriptionProperty =
            BindableProperty.Create(nameof(Description), typeof(string), typeof(BalanceLabel), string.Empty);

    public string Description
    {
        get { return (string)GetValue(PersonDetailsCard.DescriptionProperty); }
        set { SetValue(PersonDetailsCard.DescriptionProperty, value); }
    }

    public PersonDetailsCard()
	{
		InitializeComponent();
	}
}