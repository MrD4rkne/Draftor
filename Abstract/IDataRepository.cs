using Draftor.Models;
using SQLite;

namespace Draftor.Abstract;

public interface IDataRepository
{
    Task Init();

    AsyncTableQuery<Person> GetAllPeople();

    Task<Person> GetPersonAsync(int personId);

    Task<bool> DoesPersonExistsAsync(int personId);

    Task<bool> InsertPersonAsync(Person person);

    Task<bool> UpdatePersonAsync(Person person);

    Task<bool> DeletePersonAsync(int userId);

    Task<bool> AddTransactionAsync(Transaction transaction);

    AsyncTableQuery<Transaction> GetAllTransactions(int personId);

    Task<bool> DeleteTransactionAsync(int id);

    AsyncTableQuery<Group> GetGroups();

    AsyncTableQuery<Group> GetGroupsForUser(int userId);

    Task<bool> CreateGroupAsync(Group group);

    Task<bool> DeleteGroupAsync(Group group);

    Task<Group> GetGroupAsync(int groupId);

    Task<bool> DeleteGroupAsync(int id);

    Task<bool> DoesGroupExistAsync(int id);

    Task<bool> UpdateGroupAsync(Group group);

    Task<int> GetGroupMembersCount(int groupId);

    Task<bool> AddMembership(Membership membership);

    Task<bool> DeleteMembership(Membership membership);

    Task DeleteMembershipsOfUser(int userId);

    Task<Transaction> GetTransactionAsync(int transactionId);

    Task<bool> UpdateTransactionAsync(Transaction transactionToBeArchived);

    Task<bool> DoesTransactionExistAsync(int transactionId);

    AsyncTableQuery<Transaction> GetTransactionsForPerson(int id);

    Task<double> GetSumForPerson(int id);
}