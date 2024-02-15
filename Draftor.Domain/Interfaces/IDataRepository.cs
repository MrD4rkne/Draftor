using Draftor.Domain.Models;

namespace Draftor.Domain.Interfaces;

public interface IDataRepository
{
    IQueryable<Person> GetAllPeople();

    Task<Person?> GetPersonAsync(int personId);

    Task<bool> DoesPersonExistsAsync(int personId);

    Task<Person> InsertPersonAsync(Person person);

    Task<Person> UpdatePersonAsync(Person person);

    Task DeletePersonAsync(int userId);

    Task<Transaction> AddTransactionAsync(Transaction transaction);

    IQueryable<Transaction> GetAllTransactionsForPerson(int personId);

    Task RemoveTransactionAsync(int id);

    IQueryable<Group> GetGroups();

    Task<Group> CreateGroupAsync(Group group);

    Task DeleteGroupAsync(Group group);

    Task<Group?> GetGroupAsync(int groupId);

    Task DeleteGroupAsync(int id);

    Task<bool> DoesGroupExistAsync(int id);

    Task<Group> UpdateGroupAsync(Group group);

    Task<int> GetGroupMembersCount(int groupId);

    Task<Membership> AddMembership(Membership membership);

    Task DeleteMembership(Membership membership);

    Task<Transaction?> GetTransactionAsync(int transactionId);

    Task<Transaction> UpdateTransactionAsync(Transaction transaction);

    Task<bool> DoesTransactionExistAsync(int transactionId);

    IQueryable<Transaction> GetTransactionsForPerson(int personId);

    Task<double> GetSumForPerson(int personId);

    Task<Transaction> ArchiveTransactionAsync(int transactionId);
    Task AddTransactionsAsync(List<Transaction> transactionsToAdd);
}