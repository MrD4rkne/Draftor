using System.Collections.ObjectModel;
using Draftor.Abstract;
using Draftor.ViewModels;

namespace Draftor.BindingContexts;

public class GroupsDataContext : Core.ObservableObject
{
    private readonly IDataService _dataService;
    public ObservableCollection<GroupVM> Groups { get; set; }

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
            RefreshCommand?.ChangeCanExecute();
        }
    }

    public Command RefreshCommand { get; private set; }

    public Command DeleteGroupCommand { get; private set; }

    public Command ClickedGroupCommand { get; private set; }

    public Command CreateGroupCommand { get; private set; }

    public GroupsDataContext(IDataService dataService)
    {
        _dataService = dataService;
        Groups = [];
        BindCommands();
        Task.Run(Refresh);
    }

    private void BindCommands()
    {
        RefreshCommand = new Command(RefreshExecute, RefreshCanExecute);
        DeleteGroupCommand = new Command(DeleteGroupExecute);
        CreateGroupCommand = new Command(CreateGroupCommnand_Execute);
        ClickedGroupCommand = new Command(ClickedGroupExecute);
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

    private async void RefreshExecute()
    {
        await Refresh();
        IsRefreshing = false;
    }

    private bool RefreshCanExecute()
    {
        return !IsRefreshing;
    }

    private async void DeleteGroupExecute(object o)
    {
        if (o is int id)
        {
            GroupVM groupToDelete = Groups.Where(x => x.Id == id).FirstOrDefault();
            if (groupToDelete != null)
            {
                bool confirmation = await Application.Current.MainPage.DisplayAlert("Confirmation", $"Do you want to remove group titled {groupToDelete.Title}. The data will be lost.", "Yes", "No");
                if (confirmation)
                {
                    bool isSuccess = await _dataService.DeleteGroupAsync(groupToDelete);
                    if (isSuccess)
                    {
                        Groups.Remove(groupToDelete);
                        var groupCreatedToast = CommunityToolkit.Maui.Alerts.Toast.Make("Group has been successfully deleted.");
                        await groupCreatedToast.Show();
                    }
                    else
                    {
                        var groupNotCreatedToast = CommunityToolkit.Maui.Alerts.Toast.Make("Group hasn't been deleted because of the error.");
                        await groupNotCreatedToast.Show();
                    }
                }
            }
        }
    }

    private void ClickedGroupExecute(object o)
    {
        throw new NotImplementedException();
    }

    private void CreateGroupCommnand_Execute(object o)
    {
        //await Shell.Current.GoToAsync();
        throw new NotImplementedException();
    }
}