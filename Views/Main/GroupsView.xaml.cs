using Draftor.BindingContexts;

namespace Draftor.Views.Main;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class GroupsView : ContentPage
{
    public GroupsView(GroupsDataContext GroupsDataContext)
    {
        InitializeComponent();
        BindingContext = GroupsDataContext;
    }
}