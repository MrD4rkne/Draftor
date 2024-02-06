using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Draftor.Abstract;
using Draftor.ViewModels;

namespace Draftor.BindingContexts;

public class MainDataContext : ObservableObject
{
    private readonly IDataService _dataService;

    private double _total;
    public double Total
    {
        get => _total;
        set => SetProperty(ref _total, value);
    }

    private ObservableCollection<PersonMainVM> _people;
    public ObservableCollection<PersonMainVM> People
    {
        get => _people;
        set
        {
            if (SetProperty(ref _people, value))
            {
                AddPersonCommand.NotifyCanExecuteChanged();
                AddTransactionCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private bool _isRefreshing = false;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            if (SetProperty(ref _isRefreshing, value))
            {
                AddPersonCommand.NotifyCanExecuteChanged();
                AddTransactionCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public IAsyncRelayCommand NavigateToPersonDetailsCommand { get; private set; }

    public IAsyncRelayCommand AddPersonCommand { get; private set; }

    public IAsyncRelayCommand AddTransactionCommand { get; private set; }

    public IAsyncRelayCommand DeletePersonCommand { get; private set; }

    public IAsyncRelayCommand RefreshCommand { get; private set; }

    public MainDataContext(IDataService dataService, IThemeManager themeManager)
    {
        _dataService = dataService;
        themeManager.SetupAppApperance();
        BindCommands();
        People = [];
    }

    public void BindCommands()
    {
        RefreshCommand = new AsyncRelayCommand(RefreshExecute, RefreshCanExecute);
        AddPersonCommand = new AsyncRelayCommand(AddPersonExecute, AddPersonCanExecute);
        AddTransactionCommand = new AsyncRelayCommand(AddTransactionExecute, AddTransactionCanExecute);
        DeletePersonCommand = new AsyncRelayCommand<int>(DeletePersonExecute);
        NavigateToPersonDetailsCommand = new AsyncRelayCommand<int>(NavigateToPersonDetailsCommand_Execute);
    }

    public async Task Refresh()
    {
        var people = await _dataService.GetPeopleVMAsync();
        People = new(people);
        UpdateBalance();
        IsRefreshing = false;
    }

    private void UpdateBalance()
    {
        Total = People.Sum(x => x.Total);
    }

    private async Task AddPersonExecute()
    {
        await Shell.Current.GoToAsync(nameof(Views.Actions.PersonView));
    }

    private bool AddPersonCanExecute() => !IsRefreshing;

    private async Task AddTransactionExecute()
    {
        await Shell.Current.GoToAsync(nameof(Views.Actions.TransactionView));
    }

    private bool AddTransactionCanExecute()
    {
        bool canAddTransaction = People is not null && People.Count > 0;
        canAddTransaction = canAddTransaction && !IsRefreshing;
        return canAddTransaction;
    }

    private async Task DeletePersonExecute(int id)
    {
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

    private async Task RefreshExecute()
    {
        await Refresh();
        IsRefreshing = false;
    }

    private bool RefreshCanExecute()
    {
        return !IsRefreshing;
    }

    private async Task NavigateToPersonDetailsCommand_Execute(int id)
    {
        await Shell.Current.GoToAsync($"{nameof(Views.Actions.PersonView)}?id={id}");
    }
}