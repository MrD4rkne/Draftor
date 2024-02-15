namespace Draftor;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute($"HomePage/{nameof(Views.Actions.AddPersonView)}", typeof(Views.Actions.AddPersonView));
        Routing.RegisterRoute($"HomePage/{nameof(Views.Actions.PersonView)}", typeof(Views.Actions.PersonView));
        Routing.RegisterRoute($"HomePage/{nameof(Views.Actions.AddTransactionView)}", typeof(Views.Actions.AddTransactionView));

        MainTabBar.CurrentItem = HomePage;
    }
}