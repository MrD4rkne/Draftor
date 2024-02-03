using Draftor.Abstract;
using Draftor.BindingContexts;
using Draftor.Common;
using Draftor.Core;
using Draftor.Data;
using Draftor.Device;
using Draftor.Models;
using Draftor.Services;
using Draftor.ViewModels;
using Draftor.Views.Actions;
using Draftor.Views.Main;

namespace Draftor;

public static class DependencyInjection
{
    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddTransient<MainDataContext>()
            .AddTransient<GroupsDataContext>()
            .AddTransient<SettingsDataContext>()
            .AddTransient<PersonDataContext>()
            .AddTransient<TransactionDataContext>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddSingleton<IPreferencesStore, PreferencesStore>()
            .AddSingleton<IThemeManager, ThemesManager>()
            .AddSingleton<IDataService, DataService>()
            .AddSingleton<IDataRepository, DataRepository>()
            .AddSingleton<IConstantsProvider, ConstantsProvider>()
            .AddSingleton<IMapper, Services.Mapper>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddTransient<MainView>()
            .AddTransient<GroupsView>()
            .AddTransient<SettingsView>()
            .AddTransient<PersonView>()
            .AddTransient<TransactionView>();

        return mauiAppBuilder;
    }

    public static MauiApp RegisterMappings(this MauiApp mauiApp)
    {
        IMapper mapper = mauiApp.Services.GetService<IMapper>();
        if (mapper is null)
        {
            throw new ArgumentNullException(nameof(mauiApp), "IMapper instance was not registered.");
        }

        mapper.RegisterMappings();
        return mauiApp;
    }
}