using Draftor.BindingContexts;

namespace Draftor.Views.Main;

public partial class MainView : ContentPage
{
    private MainDataContext? ViewModel => BindingContext as MainDataContext;

    public MainView(BindingContexts.MainDataContext MainDataContext)
    {
        InitializeComponent();
        BindingContext = MainDataContext;
    }

    private void PeopleView_Appearing(object sender, EventArgs e)
    {
        ViewModel?.RefreshCommand.Execute(null);
    }
}