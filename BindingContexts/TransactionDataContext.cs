using System.Collections.ObjectModel;
using Draftor.Abstract;

namespace Draftor.BindingContexts;

public class TransactionDataContext : Core.ObservableObject
{
    private readonly IDataService _dataService;

    private char _sign = '+';
    public char Sign
    {
        get { return _sign; }
        set { _sign = value; OnPropertyChanged(nameof(Sign)); }
    }

    private string _title = string.Empty;
    public string Title
    {
        get { return _title; }
        set { _title = value; OnPropertyChanged(nameof(Title)); }
    }

    private string _description = string.Empty;
    public string Description
    {
        get { return _description; }
        set { _description = value; OnPropertyChanged(nameof(Description)); }
    }

    private double _ammount = 0.0;
    public double Ammount
    {
        get { return _ammount; }
        set { _ammount = value; OnPropertyChanged(nameof(Ammount)); AddTransactionCommand.ChangeCanExecute(); }
    }

    private bool _arePeopleLoading;
    public bool ArePeopleLoading
    {
        get { return _arePeopleLoading; }
        set { _arePeopleLoading = value; OnPropertyChanged(nameof(ArePeopleLoading)); }
    }

    private bool _isBusy = false;
    public bool IsBusy
    {
        get
        {
            return _isBusy;
        }
        set
        {
            _isBusy = value;
            AddTransactionCommand.ChangeCanExecute();
        }
    }

    public ObservableCollection<ViewModels.PersonForListVM> People { get; private set; }

    public Command AddTransactionCommand { get; private set; }

    public Command SwitchSignCommand { get; private set; }

    public Command PersonCheckedCommand { get; private set; }

    public TransactionDataContext(IDataService dataService)
    {
        _dataService = dataService;
        People = [];
        BindCommands();
        LoadData();
    }

    private void BindCommands()
    {
        AddTransactionCommand = new Command(AddTransactionCommand_Execute, AddTransaction_CanExecute);
        PersonCheckedCommand = new Command(PersonChecked_Execute);
        SwitchSignCommand = new Command(SwitchSignCommand_Execute);
    }

    public async void LoadData()
    {
        ArePeopleLoading = true;
        People.Clear();
        var people = await _dataService.GetPeopleForListAsync();
        foreach (var person in people)
        {
            People.Add(person);
        }
        ArePeopleLoading = false;
    }

    private async void AddTransactionCommand_Execute(object o)
    {
        IsBusy = true;
        var peopleChecked = People.Where(x => x.Checked)
            .Select(person => person.Id);
        Ammount *= (Sign == '+' ? 1 : -1);
        int peopleAffectedCount = await _dataService.AddTransactionBulk(Ammount, Title, Description, peopleChecked);
        await Shell.Current.GoToAsync("..");
        var transactionAddedToast = CommunityToolkit.Maui.Alerts.Toast.Make($"Added transaction for {peopleAffectedCount} {(peopleAffectedCount > 1 ? "people" : "person")}.");
        await transactionAddedToast.Show();
        IsBusy = false;
    }

    private bool AddTransaction_CanExecute(object o)
    {
        return !IsBusy && Ammount != 0 && People.Any(x => x.Checked);
    }

    private void SwitchSignCommand_Execute(object o)
    {
        if (Sign == '+')
            Sign = '-';
        else
            Sign = '+';
    }

    private void PersonChecked_Execute()
    {
        AddTransactionCommand.ChangeCanExecute();
    }
}