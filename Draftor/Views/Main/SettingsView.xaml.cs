using Draftor.BindingContexts;

namespace Draftor.Views.Main;

public partial class SettingsView : ContentPage
{
    public SettingsView(SettingsDataContext SettingsDataContext)
    {
        InitializeComponent();
        BindingContext = SettingsDataContext;
    }
}