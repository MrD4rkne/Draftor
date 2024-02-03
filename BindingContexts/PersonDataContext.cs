using System.Collections.ObjectModel;
using Draftor.Abstract;
using Draftor.Models;
using Draftor.ViewModels;

namespace Draftor.BindingContexts;

[QueryProperty(nameof(PersonId), "id")]
public class PersonDataContext : Core.ObservableObject
{
    private readonly IDataService _dataService;

    private bool _areTransactionsRefreshing;
    public bool AreTransactionsRefreshing
    {
        get { return _areTransactionsRefreshing; }
        set { _areTransactionsRefreshing = value; OnPropertyChanged(nameof(AreTransactionsRefreshing)); }
    }

    public string ActionButtonText => !IsAdding ? "Save" : "Add";

    public string WindowsTitle => IsAdding ? "Add person" : "Edit / view person";

    private int _personId;
    public int PersonId
    {
        set
        {
            _personId = value;
            IsAdding = false;
        }
        get
        {
            return _personId;
        }
    }

    public ObservableCollection<TransactionListViewModel> Transactions { get; set; }

    private readonly List<TransactionListViewModel> _transactions_to_remove = [];

    private PersonVM Person { get; set; } = null;

    private bool _isAdding = true;
    public bool IsAdding
    {
        get { return _isAdding; }
        set { _isAdding = value; OnPropertyChanged(nameof(IsAdding)); OnPropertyChanged(nameof(ActionButtonText)); OnPropertyChanged(nameof(IsEditting)); OnPropertyChanged(nameof(WindowsTitle)); }
    }

    public bool IsEditting => !IsAdding;

    private string _name = "";
    public string Name
    {
        get { return _name; }
        set { _name = value; OnPropertyChanged(nameof(Name)); AddCommand.ChangeCanExecute(); }
    }

    private string _description = "";
    public string Description
    {
        get { return _description; }
        set { _description = value; OnPropertyChanged(nameof(Description)); AddCommand.ChangeCanExecute(); }
    }

    private double _total;
    public double Total
    {
        get { return _total; }
        set { _total = value; OnPropertyChanged(nameof(Total)); }
    }

    public Command AddCommand { get; set; }

    public Command DeleteTransactionCommand { get; private set; }

    public Command LoadTransactionsCommand { get; private set; }

    public PersonDataContext(IDataService dataService)
    {
        Transactions = [];
        BindCommands();
        _dataService = dataService;
    }

    private void BindCommands()
    {
        AddCommand = new Command(Add_Execute, Add_CanExecute);
        DeleteTransactionCommand = new Command(DeleteTransactionExecute);
        LoadTransactionsCommand = new Command(LoadTransactionsExecute);
    }

    private async Task LoadData(int id)
    {
        Person = await _dataService.GetPersonAsync(id);
        IsAdding = false;
        Name = Person.Name;
        Description = Person.Description;
        var transactions = (await _dataService.GetAllTransactionsForPerson(id));

        if (transactions != null)
        {
            AreTransactionsRefreshing = true;
            foreach (var transaction in transactions)
            {
                Transactions.Add(transaction);
            }
            AreTransactionsRefreshing = false;
        }
        CountBalance();
    }

    private void CountBalance()
    {
        if (Transactions == null)
        {
            Total = 0;
            return;
        }
        Total = Transactions.Where(y => !y.ToRemove).Sum(x => x.Value);
    }

    private async void Add_Execute(object o)
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

    private bool Add_CanExecute(object o)
    {
        if (Person == null)
            return !string.IsNullOrEmpty(Name);
        // if anything is changed
        return !string.IsNullOrEmpty(Name) && (Name != Person.Name || Description != Person.Description || _transactions_to_remove.Count > 0);
    }

    private async void DeleteTransactionExecute(object o)
    {
        if (o is int id)
        {
            TransactionListViewModel transactionToDelete = Transactions.Where(x => x.Id == id).FirstOrDefault();
            if (transactionToDelete == null)
                return;
            bool confirmation = await App.Current.MainPage.DisplayAlert("Confirmation", $"Do you want to remove transaction titled {transactionToDelete.Title} with a value of of {transactionToDelete.Value}? The data will be lost after saving.", "Yes", "No");
            if (confirmation)
            {
                _transactions_to_remove.Add(transactionToDelete);
                AddCommand.ChangeCanExecute();
                // UI change transaction
                transactionToDelete.ToRemove = true;

                var deleteNotification = CommunityToolkit.Maui.Alerts.Toast.Make("Transaction will be erased after saving data.");
                await deleteNotification.Show();
                CountBalance();
            }
        }
    }

    private async void LoadTransactionsExecute(object o)
    {
        if(!IsEditting) return;
        await LoadData(PersonId);
    }
}