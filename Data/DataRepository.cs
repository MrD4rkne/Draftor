using Draftor.Abstract;
using Draftor.Exceptions;
using Draftor.Models;
using SQLite;

namespace Draftor.Services;

public class DataRepository : IDataRepository
{
    private readonly SQLiteAsyncConnection _database;

    private SQLiteAsyncConnection Database
    {
        get
        {
            CheckInit();
            return _database;
        }
        init
        {
            _database = value;
        }
    }

    private bool _shouldInit = true;

    public DataRepository(IConstantsProvider constantsProvider)
    {
        string databasePath = constantsProvider.GetDatabasePath();
        var flags = constantsProvider.GetDatabaseFlags();
        Database = new SQLiteAsyncConnection(databasePath, flags);
    }

    public async Task Init()
    {
        if (!_shouldInit)
            return;
        _shouldInit = false;

        _ = await Database.CreateTableAsync<Models.Person>();
        _ = await Database.CreateTableAsync<Models.Transaction>();
        _ = await Database.CreateTableAsync<Models.Group>();
        _ = await Database.CreateTableAsync<Models.Membership>();
    }

    /// <summary>
    /// Whether database connection was established by calling Init(). Throws error if not.
    /// </summary>
    /// <exception cref="RepositoryWasNotInitedException"></exception>
    private void CheckInit()
    {
        if (_shouldInit)
            throw new RepositoryWasNotInitedException();
    }

    /// <summary>
    /// Get all people
    /// </summary>
    /// <returns>List of people</returns>
    public AsyncTableQuery<Person> GetAllPeople()
    {
        return Database.Table<Person>();
    }

    /// <summary>
    /// Get person by id
    /// </summary>
    /// <param name="personId">Person's id</param>
    /// <returns>Person's model or null</returns>
    public async Task<Person> GetPersonAsync(int personId)
    {
        return await Database.FindAsync<Person>(personId);
    }

    /// <summary>
    /// Does person exist
    /// </summary>
    /// <param name="personId"></param>
    /// <returns>True if exists, otherwise false</returns>
    public async Task<bool> DoesPersonExistsAsync(int personId)
    {
        return (await GetPersonAsync(personId)) != null;
    }

    /// <summary>
    /// Add person to database
    /// </summary>
    /// <param name="person">Person's model to add</param>
    /// <returns>If adding was a success</returns>
    public async Task<bool> InsertPersonAsync(Person person)
    {
        return (await Database.InsertAsync(person)) == 1;
    }

    /// <summary>
    /// Update person's model info
    /// </summary>
    /// <param name="person">Person's model to update</param>
    /// <returns>If changes were made</returns>
    public async Task<bool> UpdatePersonAsync(Person person)
    {
        ArgumentNullException.ThrowIfNull(person);
        if (!await DoesPersonExistsAsync(person.Id))
            throw new EntityDoesNotExistException();
        return (await Database.UpdateAsync(person)) == 1;
    }

    /// <summary>
    /// Delete person (entirely)
    /// </summary>
    /// <param name="userId">Person's id</param>
    /// <returns>If operation was successful</returns>
    public async Task<bool> DeletePersonAsync(int userId)
    {
        bool deletedPerson = await Database.DeleteAsync<Person>(userId) == 1;
        await DeleteMembershipsOfUser(userId);
        return deletedPerson;
    }

    /// <summary>
    /// Add transaction to database
    /// </summary>
    /// <param name="transaction">Transaction model</param>
    /// <returns>If adding was successfull</returns>
    public async Task<bool> AddTransactionAsync(Transaction transaction)
    {
        if (transaction == null || !await DoesPersonExistsAsync(transaction.PersonId))
            return false;
        return (await Database.InsertAsync(transaction)) == 1;
    }

    /// <summary>
    /// Get all transactions for Person
    /// </summary>
    /// <param name="personId">Person's id</param>
    /// <returns>List of transactions</returns>
    public AsyncTableQuery<Transaction> GetAllTransactions(int personId)
    {
        return Database.Table<Transaction>()
            .Where(x => x.PersonId == personId);
    }

    /// <summary>
    /// Delete transaction
    /// </summary>
    /// <param name="id">Transaction's id </param>
    /// <returns>If deleting was successfull</returns>
    public async Task<bool> DeleteTransactionAsync(int id)
    {
        return await Database.DeleteAsync<Transaction>(id) == 1;
    }

    /// <summary>
    /// Get all groups
    /// </summary>
    /// <returns>List of groups</returns>
    public AsyncTableQuery<Group> GetGroups()
    {
        return Database.Table<Group>();
    }

    /// <summary>
    /// Get groups which user is a part of
    /// </summary>
    /// <param name="userId">Person's id</param>
    /// <returns>List of groups</returns>
    public AsyncTableQuery<Group> GetGroupsForUser(int userId)
    {
        return Database.Table<Group>()
            .Where(y => y.Members.Any(person => person.Id == userId));
    }

    /// <summary>
    /// Ad group
    /// </summary>
    /// <param name="group">Group's model</param>
    /// <returns>If adding was successfull</returns>
    public async Task<bool> CreateGroupAsync(Group group)
    {
        ArgumentNullException.ThrowIfNull(group, nameof(group));
        if (await DoesExistGroupWithTitleAsync(group.Title))
            throw new GroupTitleOverlapException();
        return await Database.InsertAsync(group) == 1;
    }

    private async Task<bool> DoesExistGroupWithTitleAsync(string groupTitle)
    {
        return await Database.Table<Group>()
            .Where(x => x.Title == groupTitle)
            .CountAsync() > 0;
    }

    /// <summary>
    /// Delete group
    /// </summary>
    /// <param name="group">Group to delete</param>
    /// <returns>If deleting was successfull</returns>
    public async Task<bool> DeleteGroupAsync(Group group)
    {
        ArgumentNullException.ThrowIfNull(group);
        bool success = await Database.DeleteAsync<Group>(group.Id) > 0;

        if (!success)
            return false;
        if (group.Members != null)
        {
            foreach (Person ship in group.Members)
            {
                var memberShip = new Membership() { GroupId = group.Id, PersonId = ship.Id };
                await DeleteMembership(memberShip);
            }
        }

        return true;
    }

    /// <summary>
    /// Get group by id
    /// </summary>
    /// <param name="groupId">Group's id</param>
    /// <returns>Group model or null</returns>
    public async Task<Group> GetGroupAsync(int groupId)
    {
        return await Database.FindAsync<Group>(groupId);
    }

    /// <summary>
    /// Delete group
    /// </summary>
    /// <param name="id">Group's id</param>
    /// <returns>If deleting was successsfull</returns>
    public async Task<bool> DeleteGroupAsync(int id)
    {
        var groupToBeDeleted = await GetGroupAsync(id);
        return groupToBeDeleted is null ? throw new EntityDoesNotExistException() : await DeleteGroupAsync(groupToBeDeleted);
    }

    public async Task<bool> DoesGroupExistAsync(int id)
    {
        return await GetGroupAsync(id) != null;
    }

    /// <summary>
    /// Update group
    /// </summary>
    /// <param name="group">Group's model</param>
    /// <returns>If updating was successfull</returns>
    public async Task<bool> UpdateGroupAsync(Group group)
    {
        ArgumentNullException.ThrowIfNull(group);
        if (!await DoesGroupExistAsync(group.Id))
            throw new EntityDoesNotExistException();
        return await Database.UpdateAsync(group) > 0;
    }

    /// <summary>
    /// Get members' count of the group
    /// </summary>
    /// <param name="groupId">Group's id</param>
    /// <returns>Members' count or 0 when group doesn't exist (or doesn't have any members)</returns>
    public async Task<int> GetGroupMembersCount(int groupId)
    {
        return (int)await Database.Table<Membership>()
            .Where(x => x.GroupId == groupId)
            .CountAsync();
    }

    /// <summary>
    /// Ad membership
    /// </summary>
    /// <param name="membership">Membership' smodel</param>
    /// <returns>If adding membership was successfull</returns>
    public async Task<bool> AddMembership(Membership membership)
    {
        ArgumentNullException.ThrowIfNull(membership);

        if (await GetPersonAsync(membership.PersonId) == null || await GetGroupAsync(membership.GroupId) == null)
        {
            // user or group doesn't exist
            return false;
        }
        return await Database.InsertAsync(membership) == 1;
    }

    /// <summary>
    /// Delete membership
    /// </summary>
    /// <param name="membership"></param>
    /// <returns>If deleting was successfull</returns>
    public async Task<bool> DeleteMembership(Membership membership)
    {
        ArgumentNullException.ThrowIfNull(membership);
        return await Database.DeleteAsync(membership) == 1;
    }

    /// <summary>
    /// Delete all memberships of user
    /// </summary>
    /// <param name="userId">User's id</param>
    /// <returns>If deleting was successfull</returns>
    public async Task DeleteMembershipsOfUser(int userId)
    {
        var memberShips = (await Database.Table<Membership>().Where(x => x.PersonId == userId).ToListAsync()).Select(y => y.GroupId);
        await Database.Table<Membership>().Where(x => x.PersonId == userId).DeleteAsync();
    }

    /// <summary>
    /// Gte transaction by id
    /// </summary>
    /// <param name="transactionId">transaction's id</param>
    /// <returns>Demanded transaction or null in case it does not exist</returns>
    public Task<Transaction> GetTransactionAsync(int transactionId)
    {
        return Database.FindAsync<Transaction>(transactionId);
    }

    public async Task<bool> UpdateTransactionAsync(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        if (!await DoesTransactionExistAsync(transaction.Id))
            throw new EntityDoesNotExistException();
        return await Database.UpdateAsync(transaction) == 1;
    }

    public async Task<bool> DoesTransactionExistAsync(int transactionId)
    {
        return await GetTransactionAsync(transactionId) is not null;
    }

    public AsyncTableQuery<Transaction> GetTransactionsForPerson(int id)
    {
        return _database.Table<Transaction>()
            .Where(x => x.PersonId == id);
    }

    public async Task<double> GetSumForPerson(int personId)
    {
        string sqlQuery = @$"SELECT SUM([{nameof(Transaction.Value)}])
            FROM [transactions]
            WHERE [{nameof(Transaction.PersonId)}]={personId}";
        double total = await _database.ExecuteScalarAsync<double>(sqlQuery);
        return total;
    }
}