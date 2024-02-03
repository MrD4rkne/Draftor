using Draftor.Models;

namespace Draftor.Abstract;

public interface IThemeManager
{
    void SetupAppApperance();

    Theme GetCurrentTheme();

    void SetAppTheme(Theme selectedTheme);
}