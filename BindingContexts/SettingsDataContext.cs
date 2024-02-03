using Draftor.Abstract;
using Draftor.Models;

namespace Draftor.BindingContexts;

public class SettingsDataContext : Core.ObservableObject
{
    private readonly IThemeManager _themeManager;

    private bool _isDarkModeEnabled;
    public bool IsDarkModeEnabled
    {
        get
        {
            return _isDarkModeEnabled;
        }
        set
        {
            _isDarkModeEnabled = value;
            OnPropertyChanged(nameof(IsDarkModeEnabled));
        }
    }

    public Command UpdateAppThemeCommand { get; private set; }

    public SettingsDataContext(IThemeManager themeManager)
    {
        _themeManager = themeManager;
        GetUserPreferences();
        BindCommands();
    }

    private void BindCommands()
    {
        UpdateAppThemeCommand = new Command(UpdateAppTheme);
    }

    private void GetUserPreferences()
    {
        _themeManager.SetupAppApperance();
        _isDarkModeEnabled = _themeManager.GetCurrentTheme() == Models.Theme.Dark;
    }

    private void UpdateAppTheme(object o)
    {
        if (o is bool isDarkMode)
        {
            Theme preferedTheme = isDarkMode ? Theme.Dark : Theme.Light;
            _themeManager.SetAppTheme(preferedTheme);
        }
    }
}