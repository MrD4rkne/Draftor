using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using Draftor.Abstract;
using Draftor.Core.Interfaces;
using Draftor.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace Draftor.BindingContexts;

public class MainDataContext : ObservableObject
{
    private readonly IUserDialogs _userDialogs;
    private readonly IPersonService _dataService;
    private readonly ILogger<MainDataContext> _logger;

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
                UpdateBalance();
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

    public MainDataContext(IPersonService dataService, IUserDialogs userDialogs, IThemeManager themeManager, ILogger<MainDataContext> logger)
    {
        _userDialogs = userDialogs;
        _dataService = dataService;
        _logger = logger;
        themeManager.SetupAppApperance();
        RefreshCommand = new AsyncRelayCommand(RefreshExecute, RefreshCanExecute);
        AddPersonCommand = new AsyncRelayCommand(AddPersonExecute, AddPersonCanExecute);
        AddTransactionCommand = new AsyncRelayCommand(AddTransactionExecute, AddTransactionCanExecute);
        DeletePersonCommand = new AsyncRelayCommand<int>(DeletePersonExecute);
        NavigateToPersonDetailsCommand = new AsyncRelayCommand<int>(NavigateToPersonDetailsCommand_Execute);
        _people = [];
    }

    public async Task Refresh()
    {
        var people = await _dataService.GetPeopleVMAsync();
        bool isLeft = true;
        People.Clear();
        foreach (var person in people)
        {
            person.IsLeft = isLeft;
            isLeft = !isLeft;
            People.Add(person);
        }
        UpdateBalance();
    }

    private void UpdateBalance()
    {
        double currTotal = People.Sum(x => x.Total);
        Total = currTotal;
    }

    private async Task AddPersonExecute()
    {
        await Shell.Current.GoToAsync(nameof(Views.Actions.AddPersonView));
    }

    private bool AddPersonCanExecute() => !IsRefreshing;

    private async Task AddTransactionExecute()
    {
        await Shell.Current.GoToAsync(nameof(Views.Actions.AddTransactionView));
    }

    private bool AddTransactionCanExecute()
    {
        bool canAddTransaction = People is not null && People.Count > 0;
        canAddTransaction = canAddTransaction && !IsRefreshing;
        return canAddTransaction;
    }

    private async Task DeletePersonExecute(int id)
    {
        PersonMainVM? personToDelete = People.Where(x => x.Id == id).FirstOrDefault();
        ArgumentNullException.ThrowIfNull(personToDelete, nameof(personToDelete));
        bool shouldProceesWithDeletion = await _userDialogs.ConfirmAsync("Confirmation", $"Do you want to remove person named {personToDelete.Name} with total balance of {personToDelete.Total}? The data will be lost.", "Yes", "No");
        if (!shouldProceesWithDeletion)
            return;
        try
        {
            await _dataService.DeletePersonAsync(personToDelete.Id);
        }
        catch (Exception ex)
        {
            _userDialogs.ShowToast("Person hasn't been deleted because of the error.");
            _logger.LogError(ex, "Error while deleting person.");
            return;
        }
        finally
        {
            People.Remove(personToDelete);
            UpdateBalance();
            _userDialogs.ShowToast("Person has been successfully deleted.");
        }
    }

    private async Task RefreshExecute()
    {
        IsRefreshing = true;
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