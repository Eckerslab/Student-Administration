using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using StudentAdministration.UI.ViewModel;
using StudentAdministration.UI.ViewModel.Interfaces;
using StudentAdministration.UI.Views;
using StudentAdministrationDatabase.Database;
using StudentAdministrationDatabase.Extensions;
using StudentAdministrationServices.Extensions;
using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;

namespace StudentAdministration.UI;

public static class MauiProgram
{
    /// <summary>
    ///     Creates and configures a new instance of the <see cref="MauiApp" /> class.
    /// </summary>
    /// <remarks>
    ///     This method sets up the application by configuring essential services, fonts, and Syncfusion components.
    ///     Additionally, it enables debug logging in debug builds.
    /// </remarks>
    /// <returns>A configured instance of <see cref="MauiApp" />.</returns>
    public static MauiApp CreateMauiApp()
    {
        string? syncfusionKey = ReadAppSettings("Syncfusion", "LicenseKey");
        if (!string.IsNullOrEmpty(syncfusionKey))
        {
            SyncfusionLicenseProvider.RegisterLicense(syncfusionKey);
        }

        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().
                UseMauiCommunityToolkit().
                ConfigureSyncfusionCore().
                ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

        string logPath = Path.Combine(FileSystem.Current.AppDataDirectory, "logs", "app-.log");

        Log.Logger = new LoggerConfiguration().MinimumLevel.Warning().
                                               WriteTo.File(
                                                   logPath,
                                                   rollingInterval: RollingInterval.Day,
                                                   retainedFileCountLimit: 7,
                                                   outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}").
                                               MinimumLevel.Override("Microsoft", LogEventLevel.Warning).
                                               MinimumLevel.Override(nameof(System), LogEventLevel.Warning).
                                               CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(Log.Logger);

#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddDbContext<StudentAdministrationDbContext>(options =>
        {
            string? connectionString = ReadAppSettings("ConnectionStrings", "DefaultConnection");
            options.UseSqlServer(connectionString);
            options.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
#if DEBUG
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
#endif
        });
        builder.Services.AddTransient<IMainPageViewModel, MainPageViewModel>();
        builder.Services.AddTransient<IAddEditStudentPageViewModel, AddEditStudentPageViewModel>();
        builder.Services.AddTransient<IAddGradePageViewModel, AddGradePageViewModel>();
        builder.Services.AddTransient<IDetailsPageViewModel, DetailsPageViewModel>();
        builder.Services.AddTransient<AddGradePage>();
        builder.Services.AddTransient<AddEditStudentPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<DetailsPage>();
        builder.Services.AddDataBaseRepositories();
        builder.Services.AddServices();
        MauiApp app = builder.Build();
        using (IServiceScope scope = app.Services.CreateScope())
        {
            StudentAdministrationDbContext db = scope.ServiceProvider.GetRequiredService<StudentAdministrationDbContext>();
            db.Database.Migrate();
            Debug.WriteLine($"DB Path: {FileSystem.AppDataDirectory}");
        }

        return app;

        static string? ReadAppSettings(string property, string key)
        {
            foreach (var n in Assembly.GetExecutingAssembly().GetManifestResourceNames())
                Debug.WriteLine(n);

            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream(
                "StudentAdministration.UI.appsettings.json");

            if (stream is null)
            {
                return null;
            }

            using JsonDocument doc = JsonDocument.Parse(stream);
            return doc.RootElement.GetProperty(property).GetProperty(key).GetString();
        }
    }
}