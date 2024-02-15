using Draftor.Core.ViewModels;

namespace Draftor.Core.Interfaces;

public interface IPersonService
{
    Task<List<PersonForListVM>> GetPeopleForListAsync();

    Task<List<PersonMainVM>> GetPeopleVMAsync();

    Task ArchiveTransactionAsync(int transactionId);

    Task AddTransactionAsync(TransactionVM transaction);

    Task AddTransactionBulk(double ammount, string title, string description, IEnumerable<int> peopleChecked);

    Task DeletePersonAsync(int id);

    Task<PersonVM> GetPersonAsync(int id);

    Task<List<TransactionVM>> GetAllTransactionsForPerson(int id);

    Task UpdatePerson(PersonVM editedVM);

    Task AddPerson(PersonVM personVM);

    Task RemoveTransactions(List<int> transactionsToRemoveIds);
}