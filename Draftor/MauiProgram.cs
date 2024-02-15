using System.Globalization;
using CommunityToolkit.Maui;
using Controls.UserDialogs.Maui;
using Microsoft.Extensions.Logging;

namespace Draftor;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        SetupCultureInfo();
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiCommunityToolkit()
            .UseUserDialogs(registerInterface: true, () => { })
            .RegisterViews()
            .RegisterViewModels()
            .RegisterClientApi()
            .RegisterCore()
            .RegisterInfrastructure()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        app.RegisterMappings();
        return app;
    }

    private static void SetupCultureInfo()
    {
        CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
    }
}
