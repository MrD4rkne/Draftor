﻿using Draftor.Abstract;
using Draftor.Models;
using Draftor.Resources.Styles;

namespace Draftor.Device;

public class ThemesManager : IThemeManager
{
    private readonly IPreferencesStore _preferencesStore;

    private Theme CurrentTheme { get; set; }

    public ThemesManager(IPreferencesStore preferencesStore)
    {
        _preferencesStore = preferencesStore;
    }

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
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current!.Resources.MergedDictionaries;
        mergedDictionaries.Clear();
        mergedDictionaries.Add(new MainTheme());
        switch (CurrentTheme)
        {
            case Theme.Dark:
                App.Current.UserAppTheme = AppTheme.Dark;
                mergedDictionaries.Add(new DarkTheme());
                break;

            case Theme.Light:
            default:
                App.Current.UserAppTheme = AppTheme.Light;
                mergedDictionaries.Add(new LightTheme());
                break;
        }
    }
}