using Draftor.BindingContexts;

namespace Draftor.Views.Main;

public partial class GroupsView : ContentPage
{
    public GroupsView(GroupsDataContext GroupsDataContext)
    {
        InitializeComponent();
        BindingContext = GroupsDataContext;
    }
}