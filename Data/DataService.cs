using System.Diagnostics;
using Draftor.Abstract;
using Draftor.Exceptions;
using Draftor.Models;
using Draftor.ViewModels;

namespace Draftor.Services;

public class DataService : IDataService

{
    private readonly IDataRepository _dataRepository;
    private readonly IMapper _mapper;

    private bool _wasCalledInit = false;

    public DataService(IDataRepository dataRepository, IMapper mapper)
    {
        _dataRepository = dataRepository;
        _mapper = mapper;
    }

    private async Task<IDataRepository> GetDataRepository()
    {
        if (!_wasCalledInit)
            await _dataRepository.Init();
        return _dataRepository;
    }

    /// <summary>
    /// Get people's stripped models (id, name)
    /// </summary>
    /// <returns>List of people's stripped models</returns>
    public async Task<List<PersonForListVM>> GetPeopleForListAsync()
    {
        var people = await (await GetDataRepository())
            .GetAllPeople()
            .ToListAsync();
        var peopleForList = people.Select(
            person => _mapper.MapTo<PersonForListVM>(person));
        return peopleForList.ToList();
    }

    /// <summary>
    /// Get people's viewmodel model (id, name, total)
    /// </summary>
    /// <returns>List of people's viewmodels</returns>
    public async Task<List<PersonMainVM>> GetPeopleVMAsync()
    {
        var people = await (await GetDataRepository())
            .GetAllPeople()
            .ToListAsync();

        var peopleForMainViewList = new List<PersonMainVM>();
        await Parallel.ForEachAsync(people, async (person, cts) =>
        {
            double total = await (await GetDataRepository()).GetSumForPerson(person.Id);
            PersonMainVM personMainVM = new()
            {
                Id = person.Id,
                Name = person.Name,
                Total = total
            };
            peopleForMainViewList.Add(personMainVM);
        });

        return peopleForMainViewList.OrderBy(person => person.Id)
            .ToList();
    }

    /// <summary>
    /// Archive transaction
    /// </summary>
    /// <param name="transactionId">Transaction's id ot be archived</param>
    /// <returns>If archiving was successfull</returns>
    public async Task<bool> ArchiveTransactionAsync(int transactionId)
    {
        var transactionToBeArchived = await (await GetDataRepository()).GetTransactionAsync(transactionId) ?? throw new EntityDoesNotExistException();
        transactionToBeArchived.IsArchived = true;
        return await (await GetDataRepository()).UpdateTransactionAsync(transactionToBeArchived);
    }

    /// <summary>
    /// Delete group
    /// </summary>
    /// <param name="group">Group to delete</param>
    /// <returns>If deleting was successfull</returns>
    public async Task<bool> DeleteGroupAsync(Group group)
    {
        ArgumentNullException.ThrowIfNull(group);

        return await (await GetDataRepository()).DeleteGroupAsync(group);
    }

    /// <summary>
    /// Get group by id
    /// </summary>
    /// <param name="groupId">Group's id</param>
    /// <returns>Group model or null</returns>
    public async Task<Group> GetGroupAsync(int groupId)
    {
        return await (await GetDataRepository()).GetGroupAsync(groupId);
    }

    public async Task<bool> DeleteGroupAsync(GroupVM group)
    {
        return await (await GetDataRepository()).DeleteGroupAsync(group.Id);
    }

    public async Task<bool> AddTransactionAsync(TransactionVM transaction)
    {
        var transcation = _mapper.MapTo<Transaction>(transaction);
        return await (await GetDataRepository()).AddTransactionAsync(transcation);
    }

    public async Task<int> AddTransactionBulk(double ammount, string title, string description, IEnumerable<int> peopleChecked)
    {
        int peopleAffectedCount = 0;
        await Parallel.ForEachAsync(peopleChecked, async (personId, cts) =>
        {
            Transaction transaction = new()
            {
                Value = ammount,
                Title = title,
                Description = description,
                Date = DateTime.Now,
                IsArchived = false,
                PersonId = personId
            };
            if (await (await GetDataRepository()).AddTransactionAsync(transaction))
            {
                peopleAffectedCount++;
            }
            else
            {
                Debug.WriteLine($"Transaction for person {personId} was not added");
            }
        });
        return peopleAffectedCount;
    }

    public async Task<IEnumerable<GroupVM>> GetGroupsListAsync()
    {
        var groups = await (await GetDataRepository())
            .GetGroups()
            .ToListAsync();
        List<GroupVM> groupsVMs = [];
        foreach(var group in groups)
        {
            int membersCount = (int)await _dataRepository.GetGroupMembersCount(group.Id);
            var groupVM = new GroupVM(group, membersCount);
            groupsVMs.Add(groupVM);
        }
        return groupsVMs.AsEnumerable();
    }

    public async Task<bool> DeletePersonAsync(int id)
    {
        return await (await GetDataRepository()).DeletePersonAsync(id);
    }

    public async Task<PersonVM> GetPersonAsync(int id)
    {
        var person = await (await GetDataRepository()).GetPersonAsync(id);
        return _mapper.MapTo<PersonVM>(person);
    }

    public async Task<IEnumerable<TransactionListViewModel>> GetAllTransactionsForPerson(int id)
    {
        var personTranscations = await (await GetDataRepository()).GetTransactionsForPerson(id)
            .ToListAsync();
        List<TransactionListViewModel> transactionsVMs = [];
        foreach(var transaction in personTranscations)
        {
            var transactionVM = _mapper.MapTo<TransactionListViewModel>(transaction);
            transactionsVMs.Add(transactionVM);
        }
        return transactionsVMs.AsEnumerable();
    }

    public async Task UpdatePerson(PersonVM editedVM)
    {
        Person person = _mapper.MapTo<Person>(editedVM);
        await (await GetDataRepository()).UpdatePersonAsync(person);
    }

    public async Task AddPerson(PersonVM personVM)
    {
        var person = _mapper.MapTo<Person>(personVM);
        await (await GetDataRepository()).InsertPersonAsync(person);
    }

    public async Task RemoveTransactions(List<TransactionListViewModel> transactionsToRemove)
    {
        ArgumentNullException.ThrowIfNull(transactionsToRemove);

        var transactionsToRemoveIds = transactionsToRemove.Select(x => x.Id);
        await Parallel.ForEachAsync(transactionsToRemoveIds, async (id, cts) =>
        {
            await (await GetDataRepository()).DeleteTransactionAsync(id);
        });
    }
}