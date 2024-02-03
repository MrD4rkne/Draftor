using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using Draftor.Abstract;
using Draftor.ViewModels;

namespace Draftor.BindingContexts;

public class MainDataContext : Core.ObservableObject
{
    private readonly IDataService _dataService;

    private double _total;

    public double Total
    {
        get { return _total; }
        set { _total = value; OnPropertyChanged(nameof(Total)); }
    }

    private ObservableCollection<PersonMainVM> _people;

    public ObservableCollection<PersonMainVM> People
    {
        get
        {
            return _people;
        }
        set
        {
            _people = value;
            OnPropertyChanged(nameof(People));
        }
    }

    public PersonMainVM SelectedPerson
    {
        set
        {
            OnPropertyChanged(nameof(SelectedPerson));
            if (value != null)
                Shell.Current.GoToAsync($"{nameof(Views.Actions.PersonView)}?id={value.Id}");
        }
        get
        {
            return null;
        }
    }

    private bool _isRefreshing = false;

    public bool IsRefreshing
    {
        get
        {
            return _isRefreshing;
        }
        set
        {
            _isRefreshing = value;
            OnPropertyChanged(nameof(IsRefreshing));
            AddPersonCommand.ChangeCanExecute();
            AddTransactionCommand.ChangeCanExecute();
        }
    }

    public Command AddPersonCommand { get; private set; }

    public Command AddTransactionCommand { get; private set; }

    public Command DeletePersonCommand { get; private set; }

    public Command RefreshCommand { get; private set; }

    public MainDataContext(IDataService dataService, IThemeManager themeManager)
    {
        _dataService = dataService;
        themeManager.SetupAppApperance();
        People = [];
        BindCommands();
    }

    public void BindCommands()
    {
        RefreshCommand = new Command(RefreshExecute, RefreshCanExecute);
        AddPersonCommand = new Command(AddPersonExecute, AddPersonCanExecute);
        AddTransactionCommand = new Command(AddTransactionExecute, AddTransactionCanExecute);
        DeletePersonCommand = new Command(DeletePersonExecute);
    }

    public async Task Refresh()
    {
        IsRefreshing = true;
        People.Clear();
        var people = await _dataService.GetPeopleVMAsync();
        foreach (var person in people)
        {
            People.Add(person);
        }
        UpdateBalance();
        await Task.Delay(100);
        IsRefreshing = false;
    }

    private void LoadData()
    {
        Task.Run(Refresh);
    }

    private void UpdateBalance()
    {
        Total = People.Sum(x => x.Total);
    }

    private async void AddPersonExecute()
    {
        await Shell.Current.GoToAsync(nameof(Views.Actions.PersonView));
    }

    private bool AddPersonCanExecute()
    {
        return !IsRefreshing;
    }

    private async void AddTransactionExecute()
    {
        await Shell.Current.GoToAsync(nameof(Views.Actions.TransactionView));
    }

    private bool AddTransactionCanExecute()
    {
        bool canAddTransaction = People is not null && People.Count > 0;
        canAddTransaction = canAddTransaction && !IsRefreshing;
        return canAddTransaction;
    }

    private async void DeletePersonExecute(object o)
    {
        if (o is not int id)
            return;
        PersonMainVM personToDelete = People.Where(x => x.Id == id).FirstOrDefault();
        ArgumentNullException.ThrowIfNull(personToDelete, nameof(personToDelete));
        bool shouldProceesWithDeletion = await App.Current.MainPage.DisplayAlert("Confirmation", $"Do you want to remove person named {personToDelete.Name} with total balance of {personToDelete.Total}? The data will be lost.", "Yes", "No");
        if (!shouldProceesWithDeletion)
            return;
        bool wasSuccess = await _dataService.DeletePersonAsync(personToDelete.Id);
        if (wasSuccess)
        {
            People.Remove(personToDelete);
            UpdateBalance();
            var toast = Toast.Make("Person has been successfully deleted.");
            await toast.Show();
        }
        else
        {
            var toast = Toast.Make("Person hasn't been deleted because of the error.");
            await toast.Show();
        }
    }

    private async void RefreshExecute()
    {
        await Refresh();
        IsRefreshing = false;
    }

    private bool RefreshCanExecute()
    {
        return !IsRefreshing;
    }
}