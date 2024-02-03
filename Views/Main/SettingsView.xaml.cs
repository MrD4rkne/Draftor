using Draftor.BindingContexts;

namespace Draftor.Views.Main;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingsView : ContentPage
{
    public SettingsView(SettingsDataContext SettingsDataContext)
    {
        InitializeComponent();
        BindingContext = SettingsDataContext;
    }
}