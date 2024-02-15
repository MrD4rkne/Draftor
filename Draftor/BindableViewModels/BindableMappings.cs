using Draftor.Core.ViewModels;

namespace Draftor.BindableViewModels;

public static class BindableMappings
{
    public static TransactionBindableVM GetTransactionBindableFromTransactionVM(TransactionVM transaction)
    {
        TransactionBindableVM transactionVM = new()
        {
            Id = transaction.Id,
            Value = transaction.Value,
            Date = transaction.Date,
            Description = transaction.Description,
            Title = transaction.Title,
            IsArchived = transaction.IsArchived,
            IsToRemove = false
        };
        return transactionVM;
    }
}