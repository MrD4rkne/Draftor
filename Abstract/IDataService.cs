using Draftor.Models;
using Draftor.ViewModels;

namespace Draftor.Abstract;

public interface IDataService
{
    Task<List<PersonForListVM>> GetPeopleForListAsync();

    Task<List<PersonMainVM>> GetPeopleVMAsync();

    Task<bool> ArchiveTransactionAsync(int transactionId);

    Task<bool> DeleteGroupAsync(GroupVM group);

    Task<Group> GetGroupAsync(int groupId);

    Task<bool> AddTransactionAsync(TransactionVM transaction);

    Task<int> AddTransactionBulk(double ammount, string title, string description, IEnumerable<int> peopleChecked);

    Task<IEnumerable<GroupVM>> GetGroupsListAsync();

    Task<bool> DeletePersonAsync(int id);

    Task<PersonVM> GetPersonAsync(int id);

    Task<IEnumerable<TransactionListViewModel>> GetAllTransactionsForPerson(int id);

    Task UpdatePerson(PersonVM editedVM);

    Task AddPerson(PersonVM person);

    Task RemoveTransactions(List<TransactionListViewModel> transactions_to_remove);
}