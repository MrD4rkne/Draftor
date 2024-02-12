using Draftor.BindingContexts;

namespace Draftor.Views.Actions;

public partial class AddPersonView : ContentPage
{
	public AddPersonView(AddPersonDataContext addPersonDataContext)
	{
		InitializeComponent();
		BindingContext = addPersonDataContext;
	}
}