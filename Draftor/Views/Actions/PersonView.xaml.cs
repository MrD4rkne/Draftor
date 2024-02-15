using Draftor.BindingContexts;

namespace Draftor.Views.Actions;

public partial class PersonView : ContentPage
{
    public PersonView(PersonDataContext PersonDataContext)
    {
        InitializeComponent();
        BindingContext = PersonDataContext;
    }
}