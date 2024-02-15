using Draftor.Abstract;
using Draftor.Models;

namespace Draftor.Core;

public class PreferencesStore : IPreferencesStore
{
    private const string THEME_KEY = "prefered_theme";

    public Theme GetPreferedTheme()
    {
        Theme userAppTheme = GetUserAppTheme();
        int themePreference = Preferences.Get(THEME_KEY, (int)userAppTheme);
        if (Enum.IsDefined(typeof(Theme), themePreference))
        {
            return (Theme)themePreference;
        }
        return userAppTheme;
    }

    public void SetPreferedTheme(Theme preferedTheme)
    {
        Preferences.Set(THEME_KEY, (int)preferedTheme);
    }

    private Theme GetUserAppTheme() => App.Current!.UserAppTheme switch
    {
        AppTheme.Dark => Theme.Dark,
        AppTheme.Light => Theme.Light,
        _ => Theme.Light
    };
}