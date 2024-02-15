using Draftor.Core.Exceptions;
using Draftor.Core.Interfaces;
using Draftor.Core.Mapping;
using Draftor.Core.ViewModels;
using Draftor.Domain.Exceptions;
using Draftor.Domain.Interfaces;
using Draftor.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Draftor.Core.Services;

public class PersonService(IPersonRepository dataRepository, IMapper mapper, ILogger<IPersonService> logger) : IPersonService
{
    private readonly IPersonRepository _dataRepository = dataRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IPersonService> _logger = logger;

    public async Task<List<PersonForListVM>> GetPeopleForListAsync()
    {
        try
        {
            var people = _dataRepository.GetAllPeople();
            var peopleForList = people.ProjectTo<Person, PersonForListVM>(_mapper);
            return await peopleForList.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting people for list");
            throw new UnexpectedApplicationException("Infrastructure reported exception while getting people.", ex);
        }
    }

    public async Task<List<PersonMainVM>> GetPeopleVMAsync()
    {
        IQueryable<Person> people;
        try
        {
            people = _dataRepository.GetAllPeople();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting people");
            throw new UnexpectedApplicationException("Infrastructure reported exception while getting people.", ex);
        }
        List<PersonMainVM> peopleForMainViewList = [];
        foreach (var person in people)
        {
            double total = await _dataRepository.GetSumForPerson(person.Id);
            PersonMainVM personMainVM = new()
            {
                Id = person.Id,
                Name = person.Name,
                Total = total
            };
            peopleForMainViewList.Add(personMainVM);
        }
        return peopleForMainViewList
        .OrderBy(person => person.Id)
        .ToList();
    }

    public async Task ArchiveTransactionAsync(int transactionId)
    {
        try
        {
            await _dataRepository.ArchiveTransactionAsync(transactionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while archiving transaction");
            throw new UnexpectedApplicationException("Infrastructure reported exception while archiving transaction.", ex);
        }
    }

    public async Task AddTransactionAsync(TransactionVM transaction)
    {
        var transcation = _mapper.MapTo<Transaction>(transaction);
        try
        {
            await _dataRepository.AddTransactionAsync(transcation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding transaction");
            throw new UnexpectedApplicationException("Infrastructure reported exception while adding transaction.", ex);
        }
    }

    public async Task AddTransactionBulk(double ammount, string title, string description, IEnumerable<int> peopleChecked)
    {
        List<Transaction> transactionsToAdd = peopleChecked.Select(personId =>
        {
            return new Transaction
            {
                Value = ammount,
                Title = title,
                Description = description,
                PersonId = personId
            };
        }).ToList();
        try
        {
            await _dataRepository.AddTransactionsAsync(transactionsToAdd);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding transactions bulk");
            throw new UnexpectedApplicationException("Infrastructure reported exception while adding transactions bulk.", ex);
        }
    }

    public async Task DeletePersonAsync(int id)
    {
        try
        {
            await _dataRepository.DeletePersonAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while deleting person");
            throw new UnexpectedApplicationException("Infrastructure reported exception while deleting person.", ex);
        }
    }

    public async Task<PersonVM> GetPersonAsync(int id)
    {
        Person? personModel;
        try
        {
            personModel = await _dataRepository.GetPersonAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting person");
            throw new UnexpectedApplicationException("Infrastructure reported exception while getting person.", ex);
        }

        if (personModel is null)
        {
            throw new EntityDoesNotExistException();
        }
        return _mapper.MapTo<PersonVM>(personModel);
    }

    public async Task<List<TransactionVM>> GetAllTransactionsForPerson(int id)
    {
        IQueryable<Transaction> personTranscations;
        try
        {
            personTranscations = _dataRepository.GetTransactionsForPerson(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting transactions for person");
            throw new UnexpectedApplicationException("Infrastructure reported exception while getting transactions for person.", ex);
        }
        return await personTranscations.ProjectTo<Transaction, TransactionVM>(_mapper)
            .ToListAsync();
    }

    public async Task UpdatePerson(PersonVM editedVM)
    {
        Person person = _mapper.MapTo<Person>(editedVM);
        try
        {
            await _dataRepository.UpdatePersonAsync(person);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while updating person");
            throw new UnexpectedApplicationException("Infrastructure reported exception while updating person.", ex);
        }
    }

    public async Task AddPerson(PersonVM personVM)
    {
        var personToAdd = _mapper.MapTo<Person>(personVM);
        try
        {
            await _dataRepository.InsertPersonAsync(personToAdd);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding person");
            throw new UnexpectedApplicationException("Infrastructure reported exception while adding person.", ex);
        }
    }

    public async Task RemoveTransactions(List<int> transactionsToRemoveIds)
    {
        ArgumentNullException.ThrowIfNull(transactionsToRemoveIds);

        await Parallel.ForEachAsync(transactionsToRemoveIds, async (id, cts) =>
        {
            await _dataRepository.RemoveTransactionAsync(id);
        });
    }
}