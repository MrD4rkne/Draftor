using Draftor.Abstract;
using Draftor.BindingContexts;
using Draftor.Core;
using Draftor.Core.Interfaces;
using Draftor.Core.Mapping;
using Draftor.Core.Services;
using Draftor.Device;
using Draftor.Domain.Interfaces;
using Draftor.Infrastructure;
using Draftor.Views.Actions;
using Draftor.Views.Main;

namespace Draftor;

public static class DependencyInjection
{
    public static MauiAppBuilder RegisterInfrastructure(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddSingleton<DataContext>()
            .AddSingleton<IDataRepository, DataRepository>()
            .AddSingleton<IConstantsProvider, ConstantsProvider>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterCore(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddSingleton<IPersonService, PersonService>()
            .AddSingleton<IMapper, Mapper>();
        return mauiAppBuilder;
    }

    public static MauiApp RegisterMappings(this MauiApp mauiApp)
    {
        IMapper? mapper = mauiApp.Services.GetService<IMapper>() ?? throw new ArgumentNullException(nameof(mauiApp), "IMapper instance was not registered.");
        mapper.RegisterMappings();
        return mauiApp;
    }

    public static MauiAppBuilder RegisterClientApi(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddSingleton<IPreferencesStore, PreferencesStore>()
            .AddSingleton<IThemeManager, ThemesManager>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddTransient<MainDataContext>()
            .AddTransient<GroupsDataContext>()
            .AddTransient<SettingsDataContext>()
            .AddTransient<PersonDataContext>()
            .AddTransient<TransactionDataContext>()
            .AddTransient<AddPersonDataContext>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddTransient<MainView>()
            .AddTransient<GroupsView>()
            .AddTransient<SettingsView>()
            .AddTransient<PersonView>()
            .AddTransient<AddPersonView>()
            .AddTransient<AddTransactionView>();

        return mauiAppBuilder;
    }
}