using Draftor.Abstract;
using Draftor.Core.Interfaces;
using Draftor.BindableViewModels;
using Draftor.BindingContexts;
using Draftor.Core;
using Draftor.Device;
using Draftor.Domain.Interfaces;
using Draftor.Views.Actions;
using Draftor.Views.Main;
using Draftor.Core.Services;
using Draftor.Core.Mapping;
using Draftor.Infrastructure;

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
            .AddTransient<TransactionDataContext>()
            .AddTransient<AddPersonDataContext>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddSingleton<IPreferencesStore, PreferencesStore>()
            .AddSingleton<IThemeManager, ThemesManager>()
            .AddSingleton<IPersonService, PersonService>()
            .AddSingleton<IGroupService, GroupService>()
            .AddSingleton<IDataRepository, DataRepository>()
            .AddSingleton<IConstantsProvider, ConstantsProvider>()
            .AddSingleton<IMapper, Mapper>();
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

    public static MauiApp RegisterMappings(this MauiApp mauiApp)
    {
        IMapper? mapper = mauiApp.Services.GetService<IMapper>() ?? throw new ArgumentNullException(nameof(mauiApp), "IMapper instance was not registered.");
        mapper.RegisterMappings();
        return mauiApp;
    }
}