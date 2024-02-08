using System.Windows.Input;
using Draftor.ViewModels;

namespace Draftor.Controls;

public partial class PersonStripe : ContentView
{
    public static readonly BindableProperty DetailsCommandProperty =
        BindableProperty.Create(nameof(DetailsCommand), typeof(ICommand), typeof(PersonStripe));

    public ICommand DetailsCommand
    {
        get { return (ICommand)GetValue(PersonStripe.DetailsCommandProperty); }
        set { SetValue(PersonStripe.DetailsCommandProperty, value); }
    }

    public static readonly BindableProperty DeleteCommandProperty =
        BindableProperty.Create(nameof(DeleteCommand), typeof(ICommand), typeof(PersonStripe));

    public ICommand DeleteCommand
    {
        get { return (ICommand)GetValue(PersonStripe.DeleteCommandProperty); }
        set { SetValue(PersonStripe.DeleteCommandProperty, value); }
    }

    private PersonMainVM PersonMainVM
    {
        get
        {
            if(BindingContext is PersonMainVM personVM)
                return personVM;
            throw new ArgumentException("BindingContext is not of type PersonMainVM");
        }
    }

    public PersonStripe()
	{
		InitializeComponent();
	}

    private void Delete_Clicked(object sender, EventArgs e)
    {
        DeleteCommand?.Execute(PersonMainVM.Id);
    }

    private void Stripe_Tapped(object sender, EventArgs e)
    {
        DetailsCommand?.Execute(PersonMainVM.Id);
    }
}