using Draftor.BindingContexts;

namespace Draftor.Views.Actions;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class PersonView : ContentPage
{
    // TODO: Fix views (pancakes)
    public PersonView(PersonDataContext PersonDataContext)
    {
        InitializeComponent();
        BindingContext = PersonDataContext;
    }
}