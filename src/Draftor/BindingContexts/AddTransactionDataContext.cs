using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Draftor.Core.Interfaces;
using Draftor.Core.ViewModels;

namespace Draftor.BindingContexts;

public class TransactionDataContext : ObservableObject
{
    private readonly IPersonService _dataService;

    private char _sign = '+';

    public char Sign
    {
        get => _sign;
        set => SetProperty(ref _sign, value);
    }

    private string _title = string.Empty;

    public string Title
    {
        get => _title;
        set { 
            if (SetProperty(ref _title, value)) {
                AddTransactionCommand.NotifyCanExecuteChanged();
            } 
        }
    }

    private string _description = string.Empty;

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    private double _ammount = 0.0;

    public double Ammount
    {
        get => _ammount;
        set
        {
            if (SetProperty(ref _ammount, value))
            {
                AddTransactionCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private bool _isDataBeingLoaded;

    public bool IsDataBeingLoaded
    {
        get => _isDataBeingLoaded;
        set => SetProperty(ref _isDataBeingLoaded, value);
    }

    public ObservableCollection<PersonForListVM> People { get; private set; }

    public IAsyncRelayCommand AddTransactionCommand { get; private set; }

    public IRelayCommand SwitchSignCommand { get; private set; }

    public IRelayCommand PersonCheckedCommand { get; private set; }

    public IAsyncRelayCommand LoadDataCommand { get; private set; }

    public TransactionDataContext(IPersonService dataService)
    {
        _dataService = dataService;
        People = [];
        AddTransactionCommand = new AsyncRelayCommand(AddTransactionCommand_Execute, AddTransaction_CanExecute);
        PersonCheckedCommand = new RelayCommand(PersonChecked_Execute);
        SwitchSignCommand = new RelayCommand(SwitchSignCommand_Execute);
        LoadDataCommand = new AsyncRelayCommand(LoadData);
    }

    private async Task LoadData()
    {
        IsDataBeingLoaded = true;
        await LoadPeople();
        IsDataBeingLoaded = false;
    }

    private async Task LoadPeople()
    {
        var people = await _dataService.GetPeopleForListAsync();
        People.Clear();
        foreach (var person in people)
        {
            People.Add(person);
        }
    }

    private async Task AddTransactionCommand_Execute()
    {
        var peopleChecked = People.Where(x => x.Checked)
            .Select(person => person.Id);
        Ammount *= (Sign == '+' ? 1 : -1);
        await _dataService.AddTransactionBulk(Ammount, Title, Description, peopleChecked);
        await Shell.Current.GoToAsync("..");

        int peopleAffectedCount = peopleChecked.Count();
        var transactionAddedToast = CommunityToolkit.Maui.Alerts.Toast.Make($"Added transaction for {peopleAffectedCount} {(peopleAffectedCount > 1 ? "people" : "person")}.");
        await transactionAddedToast.Show();
    }

    private bool AddTransaction_CanExecute()
    {
        return !IsDataBeingLoaded && Ammount != 0 && People.Any(x => x.Checked) && !string.IsNullOrEmpty(Title);
    }

    private void SwitchSignCommand_Execute()
    {
        if (Sign == '+')
            Sign = '-';
        else
            Sign = '+';
    }

    private void PersonChecked_Execute()
    {
        AddTransactionCommand.NotifyCanExecuteChanged();
    }
}