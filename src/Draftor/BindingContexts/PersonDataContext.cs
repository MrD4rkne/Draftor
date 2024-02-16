using System.Collections.ObjectModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using Draftor.BindableViewModels;
using Draftor.Core.Interfaces;
using Draftor.Core.ViewModels;

namespace Draftor.BindingContexts;

[QueryProperty(nameof(PersonId), "id")]
public class PersonDataContext : ObservableObject
{
    private readonly IUserDialogs _userDialogs;
    private readonly IPersonService _dataService;

    private bool _isDataBeingLoaded;

    public bool IsDataBeingLoaded
    {
        get => _isDataBeingLoaded;
        set => SetProperty(ref _isDataBeingLoaded, value);
    }

    private int _personId;
    public int PersonId
    { get => _personId; set { _personId = value; } }

    private ObservableCollection<TransactionBindableVM> _transactions;

    public ObservableCollection<TransactionBindableVM> Transactions
    {
        get => _transactions;
        set => SetProperty(ref _transactions, value);
    }

    private readonly List<int> _transactions_to_remove = [];

    private PersonVM _person;

    private PersonVM Person
    {
        get => _person;
        set
        {
            if (SetProperty(ref _person, value))
            {
                EditCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private string _name = "";

    public string Name
    {
        get => _name;
        set
        {
            if (SetProperty(ref _name, value))
            {
                EditCommand.NotifyCanExecuteChanged();
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
                EditCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private double _total;

    public double Total
    {
        get => _total;
        set => SetProperty(ref _total, value);
    }

    public IAsyncRelayCommand EditCommand { get; set; }

    public IAsyncRelayCommand DeleteTransactionCommand { get; private set; }

    public IAsyncRelayCommand DisplayTransactionCommand { get; private set; }

    public IAsyncRelayCommand LoadDataCommand { get; private set; }

    public PersonDataContext(IPersonService dataService, IUserDialogs userDialogs)
    {
        _userDialogs = userDialogs;
        _transactions = [];
        EditCommand = new AsyncRelayCommand(EditPerson, CanSaveChanges);
        DeleteTransactionCommand = new AsyncRelayCommand<int>(DeleteTransaction);
        LoadDataCommand = new AsyncRelayCommand(LoadDataForEdit);
        DisplayTransactionCommand = new AsyncRelayCommand<TransactionBindableVM>(ShowDetailsForTransaction);
        _dataService = dataService;
    }

    private async Task LoadDataForEdit()
    {
        if (PersonId == 0)
            return;
        IsDataBeingLoaded = true;
        var personToEdit = await _dataService.GetPersonAsync(PersonId);
        if (personToEdit is null)
        {
            IsDataBeingLoaded = false;
            await _userDialogs.AlertAsync("Error", "Person not found", "Ok");
            await Shell.Current.GoToAsync("..");
            return;
        }

        var personTransactions = (await _dataService.GetAllTransactionsForPerson(PersonId));
        var bindableTransactions = GetBindableTransactions(personTransactions);

        Transactions = new(bindableTransactions);
        Name = personToEdit.Name;
        Description = personToEdit.Description;
        Person = personToEdit;
        IsDataBeingLoaded = false;
        CountBalance();
    }

    private List<TransactionBindableVM> GetBindableTransactions(IEnumerable<TransactionVM> transactionVMs)
    {
        List<TransactionBindableVM> transactionBindableVMs = [];
        foreach (var transaction in transactionVMs)
        {
            var bindableTransaction = BindableMappings.GetTransactionBindableFromTransactionVM(transaction);
            transactionBindableVMs.Add(bindableTransaction);
        }
        return transactionBindableVMs;
    }

    private void CountBalance()
    {
        Total = Transactions.Where(y => !y.IsToRemove).Sum(x => x.Value);
    }

    private async Task EditPerson()
    {
        PersonVM editedVM = Person with { Name = Name, Description = Description };
        await _dataService.UpdatePerson(editedVM);
        await _dataService.RemoveTransactions(_transactions_to_remove);
        await Shell.Current.GoToAsync("..");
    }

    private bool CanSaveChanges()
    {
        if (Person == null)
            return !string.IsNullOrEmpty(Name);
        return !string.IsNullOrEmpty(Name) && (Name != Person.Name || Description != Person.Description || _transactions_to_remove.Count > 0);
    }

    private async Task DeleteTransaction(int id)
    {
        TransactionBindableVM? transactionToDelete = Transactions.Where(x => x.Id == id).FirstOrDefault();
        if (transactionToDelete is null)
            return;
        bool confirmation = await _userDialogs.ConfirmAsync($"Do you want to remove transaction titled {transactionToDelete.Title} with a value of of {transactionToDelete.Value}? The data will be lost after saving.", "Confirmation", "Yes", "No");
        if (!confirmation)
        {
            return;
        }
        _transactions_to_remove.Add(transactionToDelete.Id);
        EditCommand.NotifyCanExecuteChanged();
        transactionToDelete.IsToRemove = true;
        _userDialogs.ShowToast("Transaction will be erased after saving data.");
        CountBalance();
    }

    private async Task ShowDetailsForTransaction(TransactionBindableVM? transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        StringBuilder detailsBuilder = new();
        detailsBuilder.AppendLine($"Title: {transaction.Title}");
        detailsBuilder.AppendLine($"Value: {transaction.Value}");
        detailsBuilder.AppendLine($"Date: {transaction.Date}");
        detailsBuilder.AppendLine($"Description: {transaction.Description}");
        await _userDialogs.AlertAsync(message: detailsBuilder.ToString(), title: "Details", okText: "Ok");
    }
}