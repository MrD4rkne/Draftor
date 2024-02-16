using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Draftor.Core.Interfaces;
using Draftor.Core.ViewModels;

namespace Draftor.BindingContexts;

public class AddPersonDataContext : ObservableObject
{
    private readonly IPersonService _dataService;

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

    public IAsyncRelayCommand AddCommand { get; set; }

    public AddPersonDataContext(IPersonService dataService)
    {
        _dataService = dataService;
        AddCommand = new AsyncRelayCommand(AddTransaction, CanAddTransaction);
    }

    private async Task AddTransaction()
    {
        PersonVM editedVM = new() { Name = Name, Description = Description };
        await _dataService.AddPerson(editedVM);
        await Shell.Current.GoToAsync("..");
    }

    private bool CanAddTransaction()
    {
        return !string.IsNullOrEmpty(Name);
    }
}