using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using Draftor.Abstract;
using Draftor.ViewModels;

namespace Draftor.BindingContexts;

public class GroupsDataContext : ObservableObject
{
    private readonly IUserDialogs _userDialogs;
    private readonly IDataService _dataService;
    public ObservableCollection<GroupVM> Groups { get; set; }

    private bool _isRefreshing = false;
    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            if (SetProperty(ref _isRefreshing, value))
            {
                RefreshCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public IAsyncRelayCommand RefreshCommand { get; private set; }

    public IAsyncRelayCommand DeleteGroupCommand { get; private set; }

    public IAsyncRelayCommand ClickedGroupCommand { get; private set; }

    public IAsyncRelayCommand CreateGroupCommand { get; private set; }

    public GroupsDataContext(IDataService dataService, IUserDialogs userDialogs)
    {
        _userDialogs = userDialogs;
        _dataService = dataService;
        Groups = [];
        RefreshCommand = new AsyncRelayCommand(RefreshExecute, RefreshCanExecute);
        DeleteGroupCommand = new AsyncRelayCommand<GroupVM>(DeleteGroupExecute);
        CreateGroupCommand = new AsyncRelayCommand(CreateGroupCommnand_Execute);
        ClickedGroupCommand = new AsyncRelayCommand(ClickedGroupExecute);
        Task.Run(Refresh);
    }

    public async Task Refresh()
    {
        IsRefreshing = true;
        Groups.Clear();
        var groups = await _dataService.GetGroupsListAsync();
        foreach (var group in groups)
        {
            Groups.Add(group);
        }
        await Task.Delay(100);
        IsRefreshing = false;
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

    private async Task DeleteGroupExecute(GroupVM? groupToDelete)
    {
        ArgumentNullException.ThrowIfNull(groupToDelete);
        
        bool confirmation = await _userDialogs.ConfirmAsync("Confirmation", $"Do you want to remove group titled {groupToDelete.Title}. The data will be lost.", "Yes", "No");
        if (!confirmation)
        {
            return;
        }

        bool isSuccess = await _dataService.DeleteGroupAsync(groupToDelete);
        if (isSuccess)
        {
            Groups.Remove(groupToDelete);
            _userDialogs.ShowToast("Group has been successfully deleted.");
        }
        else
        {
            _userDialogs.ShowToast("Group hasn't been deleted because of the error.");
        }
    }

    private Task ClickedGroupExecute()
    {
        throw new NotImplementedException();
    }

    private Task CreateGroupCommnand_Execute()
    {
        //await Shell.Current.GoToAsync();
        throw new NotImplementedException();
    }
}