using Draftor.BindingContexts;

namespace Draftor.Views.Main;

public partial class MainView : ContentPage
{
    public MainView(BindingContexts.MainDataContext MainDataContext)
    {
        InitializeComponent();
        BindingContext = MainDataContext;
    }
}