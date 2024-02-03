using Draftor.Models;

namespace Draftor.Abstract;

public interface IPreferencesStore
{
    Theme GetPreferedTheme();

    void SetPreferedTheme(Theme preferedTheme);
}