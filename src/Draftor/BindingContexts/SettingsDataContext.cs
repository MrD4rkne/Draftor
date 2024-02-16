using System.Diagnostics.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using Draftor.Abstract;
using Draftor.Models;

namespace Draftor.BindingContexts;

public class SettingsDataContext : ObservableObject
{
    private const string LICENSES = @"This app uses third-party icons. Here are the licenses for them:
- Home icons created by Aswell Studio - Flaticon - https://www.flaticon.com/free-icons/home
- Settings icons created by Gregor Cresnar Premium - Flaticon - https://www.flaticon.com/free-icons/settings
- Add icons created by Freepik - Flaticon - https://www.flaticon.com/free-icons/add
- People icons created by alkhalifi design - Flaticon - https://www.flaticon.com/free-icons/people
- Balance icons created by Xinh Studio - Flaticon - https://www.flaticon.com/free-icons/balance";

    private const string AUTHORS_PAGE = "https://github.com/MrD4rkne";

    private readonly IUserDialogs _userDialogs;
    private readonly IThemeManager _themeManager;

    private bool _isDarkModeEnabled;

    public bool IsDarkModeEnabled
    {
        get => _isDarkModeEnabled;
        set => SetProperty(ref _isDarkModeEnabled, value);
    }

    public IRelayCommand UpdateAppThemeCommand { get; private set; }

    public IAsyncRelayCommand ShowLicensesCommand { get; private set; }

    public IAsyncRelayCommand NavigateToAuthorsPageCommand { get; private set; }

    public SettingsDataContext(IThemeManager themeManager, IUserDialogs userDialogs)
    {
        _themeManager = themeManager;
        _userDialogs = userDialogs;
        GetUserPreferences();
        UpdateAppThemeCommand = new RelayCommand<bool>(UpdateAppTheme);
        ShowLicensesCommand = new AsyncRelayCommand(ShowLicenses);
        NavigateToAuthorsPageCommand = new AsyncRelayCommand(NavigateToAuthorsPage);
    }

    private void GetUserPreferences()
    {
        _isDarkModeEnabled = _themeManager.GetCurrentTheme() == Models.Theme.Dark;
    }

    private void UpdateAppTheme(bool isDarkMode)
    {
        Theme preferedTheme = isDarkMode ? Theme.Dark : Theme.Light;
        _themeManager.SetAppTheme(preferedTheme);
    }

    private async Task ShowLicenses()
    {
        await _userDialogs.AlertAsync(LICENSES, "Licenses");
    }
    
    private async Task NavigateToAuthorsPage()
    {
        Uri uri = new Uri(AUTHORS_PAGE);
        await Browser.Default.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
    }
}