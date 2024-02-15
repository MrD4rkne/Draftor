using Draftor.Core.Interfaces;
using Draftor.Core.ViewModels;
using Draftor.Domain.Models;

namespace Draftor.Core.Mapping;

public static class Mappings
{
    public static void RegisterMappings(this IMapper mapper)
    {
        mapper.RegisterMap<Person, PersonVM>(Mappings.GetPersonVMFromPerson);
        mapper.RegisterMap<PersonVM, Person>(Mappings.GetPersonFromPersonVM);
        mapper.RegisterMap<PersonForListVM, Person>(Mappings.GetPersonFromPersonForListVM);
        mapper.RegisterMap<Person, PersonForListVM>(Mappings.GetPersonForListVMFromPerson);
        mapper.RegisterMap<Transaction, TransactionVM>(Mappings.GetTransactionListViewModelFromTransaction);
        mapper.RegisterMap<TransactionVM, Transaction>(Mappings.GetTransactionFromTransactionListViewModel);
    }

    public static PersonVM GetPersonVMFromPerson(Person person)
    {
        PersonVM personVM = new()
        {
            Id = person.Id,
            Name = person.Name,
            Description = person.Description
        };

        return personVM;
    }

    public static Person GetPersonFromPersonVM(PersonVM personVM)
    {
        Person person = new()
        {
            Id = personVM.Id,
            Name = personVM.Name,
            Description = personVM.Description
        };
        return person;
    }

    public static PersonForListVM GetPersonForListVMFromPerson(Person person)
    {
        PersonForListVM personForListVM = new()
        {
            Id = person.Id,
            Name = person.Name,
            Checked = false
        };
        return personForListVM;
    }

    public static Person GetPersonFromPersonForListVM(PersonForListVM personForListVM)
    {
        Person person = new()
        {
            Id = personForListVM.Id,
            Name = personForListVM.Name
        };
        return person;
    }

    public static TransactionVM GetTransactionListViewModelFromTransaction(Transaction transaction)
    {
        TransactionVM transactionListViewModel = new()
        {
            Id = transaction.Id,
            Description = transaction.Description,
            Title = transaction.Title,
            Value = transaction.Value,
            IsArchived = transaction.IsArchived,
            Date = transaction.Date
        };
        return transactionListViewModel;
    }

    public static Transaction GetTransactionFromTransactionListViewModel(TransactionVM transactionVM)
    {
        Transaction transaction = new()
        {
            Id = transactionVM.Id,
            Description = transactionVM.Description,
            Title = transactionVM.Title,
            Value = transactionVM.Value,
            IsArchived = transactionVM.IsArchived,
            Date = transactionVM.Date
        };
        return transaction;
    }
}