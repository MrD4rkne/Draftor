using System.Globalization;
using CommunityToolkit.Maui;
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
            .RegisterViews()
            .RegisterViewModels()
            .RegisterServices()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void SetupCultureInfo()
    {
        CultureInfo customCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
    }
}
