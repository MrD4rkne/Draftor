using Draftor.Domain.Models;

namespace Draftor.Domain.Interfaces;

public interface IPersonRepository
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

    Task<Transaction?> GetTransactionAsync(int transactionId);

    Task<Transaction> UpdateTransactionAsync(Transaction transaction);

    Task<bool> DoesTransactionExistAsync(int transactionId);

    IQueryable<Transaction> GetTransactionsForPerson(int personId);

    Task<double> GetSumForPerson(int personId);

    Task<Transaction> ArchiveTransactionAsync(int transactionId);

    Task AddTransactionsAsync(List<Transaction> transactionsToAdd);
}