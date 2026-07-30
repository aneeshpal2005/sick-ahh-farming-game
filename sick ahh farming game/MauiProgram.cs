using Microsoft.Extensions.Logging;
using sick_ahh_farming_game.Services;

namespace sick_ahh_farming_game
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<GameService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }

    public static class ServiceHelper
    {
        public static IServiceProvider? Provider { get; set; }

        public static T GetService<T>() where T : notnull
        {
            if (Provider == null)
                throw new InvalidOperationException("Service provider has not been set.");

            return Provider.GetRequiredService<T>();
        }
    }
}
