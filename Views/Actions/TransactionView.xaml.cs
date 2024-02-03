using Draftor.BindingContexts;

namespace Draftor.Views.Actions;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class TransactionView : ContentPage
{
    public TransactionView(TransactionDataContext TransactionDataContext)
    {
        InitializeComponent();
        BindingContext = TransactionDataContext;
    }
}