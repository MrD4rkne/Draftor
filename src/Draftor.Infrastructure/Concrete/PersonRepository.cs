using Draftor.Domain.Exceptions;
using Draftor.Domain.Interfaces;
using Draftor.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Draftor.Infrastructure.Concrete;

public class PersonRepository(DataContext dataContext, ILogger<IPersonRepository> logger) : IPersonRepository
{
    private readonly DataContext _context = dataContext;
    private readonly ILogger<IPersonRepository> _logger = logger;

    public IQueryable<Person> GetAllPeople()
    {
        return _context.People;
    }

    public async Task<Person?> GetPersonAsync(int personId)
    {
        try
        {
            var person = await _context.People.FindAsync(personId);
            return person;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting person from database");
            throw new UnknownInfrastructureException("Error while getting person from database", ex);
        }
    }

    public async Task<bool> DoesPersonExistsAsync(int personId)
    {
        return await GetPersonAsync(personId) is not null;
    }

    public async Task<Person> InsertPersonAsync(Person person)
    {
        try
        {
            _context.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding person to database");
            throw new UnknownInfrastructureException("Error while adding person to database", ex);
        }
    }

    public async Task<Person> UpdatePersonAsync(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        var currPerson = await GetPersonAsync(person.Id) ?? throw new EntityDoesNotExistException();
        currPerson.Name = person.Name;
        currPerson.Description = person.Description;
        try
        {
            await _context.SaveChangesAsync();
            return person;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating person in database");
            throw new UnknownInfrastructureException("Error while updating person in database", ex);
        }
    }

    public async Task DeletePersonAsync(int userId)
    {
        Person? personToDelete = await GetPersonAsync(userId) ?? throw new EntityDoesNotExistException();
        _context.Remove(personToDelete);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting person from database");
            throw new UnknownInfrastructureException("Error while deleting person from database", ex);
        }
    }

    public async Task<Transaction> AddTransactionAsync(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        if (!await DoesPersonExistsAsync(transaction.PersonId))
        {
            throw new EntityDoesNotExistException("Recipient of transaction does not exist.");
        }
        await _context.Transactions.AddAsync(transaction);
        try
        {
            await _context.SaveChangesAsync();
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding transaction to database");
            throw new UnknownInfrastructureException("Error while adding transaction to database", ex);
        }
    }

    public IQueryable<Transaction> GetAllTransactionsForPerson(int personId)
    {
        return _context.Transactions
             .Where(x => x.PersonId == personId);
    }

    public async Task RemoveTransactionAsync(int id)
    {
        var transactionToRemove = await GetTransactionAsync(id) ?? throw new EntityDoesNotExistException();
        _context.Remove(transactionToRemove);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting transaction from database");
            throw new UnknownInfrastructureException("Error while deleting transaction from database", ex);
        }
    }

    public async Task<Transaction?> GetTransactionAsync(int transactionId)
    {
        try
        {
            Transaction? transaction = await _context.Transactions
                .FindAsync(transactionId);
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting transaction from database");
            throw new UnknownInfrastructureException("Error while getting transaction from database", ex);
        }
    }

    public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        Transaction transactionToEdit = await GetTransactionAsync(transaction.Id) ?? throw new EntityDoesNotExistException();
        transactionToEdit.Title = transaction.Title;
        transactionToEdit.Description = transaction.Description;
        transactionToEdit.Value = transaction.Value;
        transactionToEdit.PersonId = transaction.PersonId;
        try
        {
            await _context.SaveChangesAsync();
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating transaction in database");
            throw new UnknownInfrastructureException("Error while updating transaction in database", ex);
        }
    }

    public async Task<bool> DoesTransactionExistAsync(int transactionId)
    {
        return await GetTransactionAsync(transactionId) is not null;
    }

    public IQueryable<Transaction> GetTransactionsForPerson(int personId)
    {
        return _context.Transactions
            .Where(x => x.PersonId == personId);
    }

    public async Task<decimal> GetSumForPerson(int personId)
    {
        try
        {
            var rawSum = await _context.Transactions
                               .Where(x => x.PersonId == personId)
                               .Where(tr => tr.IsArchived == false)
                               .SumAsync(x => x.Value);
           double roundedSum = Math.Round(rawSum, 2);
           return Convert.ToDecimal(roundedSum);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting sum for person from database");
            throw new UnknownInfrastructureException("Error while getting sum for person from database", ex);
        }
    }

    public async Task<Transaction> ArchiveTransactionAsync(int transactionId)
    {
        Transaction? transactionToArchive = await GetTransactionAsync(transactionId) ?? throw new EntityDoesNotExistException("Transaction does not exist.");
        transactionToArchive.IsArchived = true;
        _context.Attach(transactionToArchive);
        _context.Entry(transactionToArchive).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
            return transactionToArchive;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while archiving transaction in database");
            throw new UnknownInfrastructureException("Error while archiving transaction in database", ex);
        }
    }

    public Task AddTransactionsAsync(List<Transaction> transactionsToAdd)
    {
        ArgumentNullException.ThrowIfNull(transactionsToAdd);
        _context.Transactions.AddRange(transactionsToAdd);
        try
        {
            return _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding transactions bulk to database");
            throw new UnknownInfrastructureException("Error while adding transactions bulk to database", ex);
        }
    }
}