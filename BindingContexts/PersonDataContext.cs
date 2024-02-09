using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using Draftor.Abstract;
using Draftor.Models;
using Draftor.ViewModels;

namespace Draftor.BindingContexts;

[QueryProperty(nameof(PersonId), "id")]
public class PersonDataContext : ObservableObject
{
    private IUserDialogs _userDialogs;
    private readonly IDataService _dataService;

    private bool _isDataBeingLoaded;
    public bool IsDataBeingLoaded
    {
        get => _isDataBeingLoaded;
        set => SetProperty(ref _isDataBeingLoaded, value);
    }

    public string ActionButtonText => !IsAdding ? "Save" : "Add";

    public string WindowsTitle => IsAdding ? "Add person" : "Edit / view person";

    private int _personId;
    public int PersonId { get => _personId; set { _personId = value; IsAdding = false; } }

    private ObservableCollection<TransactionForListVM> _transactions;
    public ObservableCollection<TransactionForListVM> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }

    private readonly List<TransactionForListVM> _transactions_to_remove = [];

    private PersonVM _person;
    private PersonVM Person
    {
        get => _person;
        set
        {
            if (SetProperty(ref _person, value))
            {
                AddCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private bool _isAdding = true;
    public bool IsAdding
    {
        get => _isAdding;
        set
        {
            if (SetProperty(ref _isAdding, value))
            {
                OnPropertyChanged(nameof(IsEditting));
            }
        }
    }

    public bool IsEditting => !IsAdding;

    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
            {
                AddCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private string _description = "";
    public string Description
    {
        get => _description;
        set
        {
            if (SetProperty(ref _description, value))
            {
                AddCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private double _total;
    public double Total
    {
        get => _total;
        set => SetProperty(ref _total, value);
    }

    public IAsyncRelayCommand AddCommand { get; set; }

    public IAsyncRelayCommand DeleteTransactionCommand { get; private set; }

    public IAsyncRelayCommand DisplayTransactionCommand { get; private set; }

    public IAsyncRelayCommand LoadDataCommand { get; private set; }

    public PersonDataContext(IDataService dataService, IUserDialogs userDialogs)
    {
        _userDialogs = userDialogs;
        Transactions = [];
        AddCommand = new AsyncRelayCommand(AddTransaction, CanAddTransaction);
        DeleteTransactionCommand = new AsyncRelayCommand<int>(DeleteTransaction);
        LoadDataCommand = new AsyncRelayCommand(LoadDataForEdit);
        DisplayTransactionCommand = new AsyncRelayCommand<TransactionForListVM>(ShowDetailsForTransaction);
        _dataService = dataService;
    }

    private async Task LoadDataForEdit()
    {
        if (PersonId == 0)
            return;
        IsDataBeingLoaded = true;
        var personToEdit = await _dataService.GetPersonAsync(PersonId);
        var peopleTransactions = (await _dataService.GetAllTransactionsForPerson(PersonId));

        Transactions = new(peopleTransactions);
        IsDataBeingLoaded = false;
        Name = personToEdit.Name;
        Description = personToEdit.Description;
        Person = personToEdit;
        CountBalance();
    }

    private void CountBalance()
    {
        if (Transactions is null)
        {
            Total = 0;
            return;
        }
        Total = Transactions.Where(y => !y.ToRemove).Sum(x => x.Value);
    }

    private async Task AddTransaction()
    {
        if (Person == null)
        {
            PersonVM personToCreate = new()
            {
                Name = Name,
                Description = Description
            };
            await _dataService.AddPerson(personToCreate);
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            PersonVM editedVM = Person with { Name = Name, Description = Description };
            await _dataService.UpdatePerson(editedVM);
            await _dataService.RemoveTransactions(_transactions_to_remove);
            await Shell.Current.GoToAsync("..");
        }
    }

    private bool CanAddTransaction()
    {
        if (Person == null)
            return !string.IsNullOrEmpty(Name);
        return !string.IsNullOrEmpty(Name) && (Name != Person.Name || Description != Person.Description || _transactions_to_remove.Count > 0);
    }

    private async Task DeleteTransaction(int id)
    {
        TransactionForListVM? transactionToDelete = Transactions.Where(x => x.Id == id).FirstOrDefault();
        if (transactionToDelete is null)
            return;
        bool confirmation = await _userDialogs.ConfirmAsync("Confirmation", $"Do you want to remove transaction titled {transactionToDelete.Title} with a value of of {transactionToDelete.Value}? The data will be lost after saving.", "Yes", "No");
        if (confirmation)
        {
            _transactions_to_remove.Add(transactionToDelete);
            AddCommand.NotifyCanExecuteChanged();
            transactionToDelete.ToRemove = true;
            _userDialogs.ShowToast("Transaction will be erased after saving data.");
            CountBalance();
        }
    }

    private async Task ShowDetailsForTransaction(TransactionForListVM? transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        StringBuilder detailsBuilder = new();
        detailsBuilder.AppendLine($"Title: {transaction.Title}");
        detailsBuilder.AppendLine($"Value: {transaction.Value}");
        detailsBuilder.AppendLine($"Date: {transaction.Date}");
        detailsBuilder.AppendLine($"Description: {transaction.Description}");
        await _userDialogs.AlertAsync("Details",detailsBuilder.ToString(), "Ok");
    }
}