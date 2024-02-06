using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Draftor.Abstract;
using Draftor.Models;

namespace Draftor.BindingContexts;

public class SettingsDataContext : ObservableObject
{
    private readonly IThemeManager _themeManager;

    private bool _isDarkModeEnabled;
    public bool IsDarkModeEnabled
    {
        get => _isDarkModeEnabled;
        set => SetProperty(ref _isDarkModeEnabled, value);
    }

    public IRelayCommand UpdateAppThemeCommand { get; private set; }

    public SettingsDataContext(IThemeManager themeManager)
    {
        _themeManager = themeManager;
        GetUserPreferences();
        BindCommands();
    }

    private void BindCommands()
    {
        UpdateAppThemeCommand = new RelayCommand<bool>(UpdateAppTheme);
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
}