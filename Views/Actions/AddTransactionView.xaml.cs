using Draftor.BindingContexts;

namespace Draftor.Views.Actions;

public partial class AddTransactionView : ContentPage
{
    public AddTransactionView(TransactionDataContext TransactionDataContext)
    {
        InitializeComponent();
        BindingContext = TransactionDataContext;
    }
}