namespace Draftor;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute($"HomePage/{nameof(Views.Actions.PersonView)}", typeof(Views.Actions.PersonView));
        Routing.RegisterRoute($"HomePage/{nameof(Views.Actions.TransactionView)}", typeof(Views.Actions.TransactionView));

        MainTabBar.CurrentItem = HomePage;
    }
}
