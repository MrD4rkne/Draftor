using Draftor.Abstract;
using Draftor.Models;

namespace Draftor.Core;

public class PreferencesStore : IPreferencesStore
{
    private const Theme DEFAULT_THEME = Theme.Light;
    private const string THEME_KEY = "prefered_theme";

    public Theme GetPreferedTheme()
    {
        int themePreference = Preferences.Get(THEME_KEY, (int)DEFAULT_THEME);
        if (Enum.IsDefined(typeof(Theme), themePreference))
        {
            return (Theme)themePreference;
        }
        return DEFAULT_THEME;
    }

    public void SetPreferedTheme(Theme preferedTheme)
    {
        Preferences.Set(THEME_KEY, (int)preferedTheme);
    }
}