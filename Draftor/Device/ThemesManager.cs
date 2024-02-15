using Draftor.Abstract;
using Draftor.Models;

namespace Draftor.Device;

public class ThemesManager(IPreferencesStore preferencesStore) : IThemeManager
{
    private readonly IPreferencesStore _preferencesStore = preferencesStore;

    private Theme CurrentTheme { get; set; }

    public void SetupAppApperance()
    {
        LoadTheme();
    }

    public void SetAppTheme(Theme selectedTheme)
    {
        _preferencesStore.SetPreferedTheme(selectedTheme);
        LoadTheme();
    }

    public Theme GetCurrentTheme()
    {
        return CurrentTheme;
    }

    private void LoadTheme()
    {
        CurrentTheme = _preferencesStore.GetPreferedTheme();
        UpdateTheme();
    }

    private void UpdateTheme()
    {
        App.Current!.UserAppTheme = CurrentTheme switch
        {
            Theme.Dark => AppTheme.Dark,
            _ => AppTheme.Light,
        };
    }
}