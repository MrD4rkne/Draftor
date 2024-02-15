using System.Linq;
using Draftor.Domain.Exceptions;
using Draftor.Domain.Interfaces;
using Draftor.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Draftor.Infrastructure;

public class DataRepository : IDataRepository
{
    private readonly DataContext _context;
    private readonly ILogger<IDataRepository> _logger;
    public DataRepository(DataContext dataContext, ILogger<IDataRepository> logger)
    {
        _context = dataContext;
        _logger = logger;
    }

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
        if (!await DoesPersonExistsAsync(person.Id))
            throw new EntityDoesNotExistException();
        _context.Attach(person);
        _context.Entry(person).State = EntityState.Modified;
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

    public IQueryable<Group> GetGroups()
    {
        return _context.Groups;
    }

    public IQueryable<Group> GetGroupsForUser(int userId)
    {
        return _context.Groups
            .Where(group => group.Members!.Any(person => person.Id == userId));
    }

    public async Task<Group> CreateGroupAsync(Group group)
    {
        ArgumentNullException.ThrowIfNull(group, nameof(group));
        if (await DoesGroupWithTitleExistAsync(group.Title))
            throw new GroupTitleOverlapException();
        _context.Add(group);
        try
        {
            await _context.SaveChangesAsync();
            return group;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding group to database");
            throw new UnknownInfrastructureException("Error while adding group to database", ex);
        }
    }

    private async Task<bool> DoesGroupWithTitleExistAsync(string groupTitle)
    {
        return await _context.Groups
            .AnyAsync(x => x.Title == groupTitle);
    }

    public async Task DeleteGroupAsync(Group group)
    {
        ArgumentNullException.ThrowIfNull(group);
        _context.Remove(group);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting group from database");
            throw new UnknownInfrastructureException("Error while deleting group from database", ex);
        }
    }

    public async Task<Group?> GetGroupAsync(int groupId)
    {
        try
        {
            Group? group = await _context.Groups.FindAsync(groupId);
            return group;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting group from database");
            throw new UnknownInfrastructureException("Error while getting group from database", ex);
        }
    }

    public async Task DeleteGroupAsync(int id)
    {
        var groupToBeDeleted = await GetGroupAsync(id) ?? throw new EntityDoesNotExistException("Group of that id does not exist.");
        _context.Remove(groupToBeDeleted);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting group from database");
            throw new UnknownInfrastructureException("Error while deleting group from database", ex);
        }
    }

    public async Task<bool> DoesGroupExistAsync(int id)
    {
        return await GetGroupAsync(id) is not null;
    }

    public async Task<Group> UpdateGroupAsync(Group group)
    {
        ArgumentNullException.ThrowIfNull(group);
        if (!await DoesGroupExistAsync(group.Id))
            throw new EntityDoesNotExistException();
        _context.Attach(group);
        _context.Entry(group).State = EntityState.Modified;
        try
        {
            await _context.SaveChangesAsync();
            return group;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating group in database");
            throw new UnknownInfrastructureException("Error while updating group in database", ex);
        }
    }

    public Task<int> GetGroupMembersCount(int groupId)
    {
        return _context.Groups
            .Where(x => x.Id == groupId)
            .SelectMany(x => x.Members)
            .CountAsync();
    }

    public async Task<Membership> AddMembership(Membership membership)
    {
        ArgumentNullException.ThrowIfNull(membership);
        if (!await DoesGroupExistAsync(membership.GroupId))
        {
            throw new EntityDoesNotExistException("Group does not exist.");
        }
        if (!await DoesPersonExistsAsync(membership.PersonId))
        {
            throw new EntityDoesNotExistException("Person does not exist.");
        }
        _context.Add(membership);
        try
        {
            await _context.SaveChangesAsync();
            return membership;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding membership to database");
            throw new UnknownInfrastructureException("Error while adding membership to database", ex);
        }
    }

    public async Task DeleteMembership(Membership membership)
    {
        ArgumentNullException.ThrowIfNull(membership);
        _context.Remove(membership);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting membership from database");
            throw new UnknownInfrastructureException("Error while deleting membership from database", ex);
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
        if (!await DoesTransactionExistAsync(transaction.Id))
            throw new EntityDoesNotExistException();
        _context.Attach(transaction);
        _context.Entry(transaction).State = EntityState.Modified;
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

    public async Task<double> GetSumForPerson(int personId)
    {
        try
        {
            return await _context.Transactions
                               .Where(x => x.PersonId == personId)
                               .Where(tr => tr.IsArchived == false)
                               .SumAsync(x => x.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting sum for person from database");
            throw new UnknownInfrastructureException("Error while getting sum for person from database", ex);
        }
    }

    public async Task<Transaction> ArchiveTransactionAsync(int transactionId) { 
        Transaction? transactionToArchive = await GetTransactionAsync(transactionId);
        if(transactionToArchive is null)
        {
            throw new EntityDoesNotExistException("Transaction does not exist.");
        }

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

    public Task AddTransactionsAsync(List<Transaction> transactionsToAdd) { 
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