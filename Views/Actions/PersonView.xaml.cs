using Draftor.BindingContexts;

namespace Draftor.Views.Actions;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PersonView : ContentPage
{
    // TODO: Diverse adding and editing person
    public PersonView(PersonDataContext PersonDataContext)
    {
        InitializeComponent();
        BindingContext = PersonDataContext;
    }
}