using System.IO;
using System.Windows;
using System.Windows.Media;
using iNKORE.UI.WPF.Modern;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using S.Player.Options;
using S.Player.Pages;
using S.Player.Services.InfoBar;
using S.Player.Utils.Extensions;
using S.Player.Utils.Helpers;
using S.Player.Utils.Options;
using S.Player.ViewModels.Pages;
using S.Player.ViewModels.Windows;
using Serilog;

namespace S.Player;

public partial class App
{
    private static readonly IHost _host = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(config => { _ = config.AddJsonFile("appsettings.json", false, true); })
        .UseSerilog((context, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        })
        .ConfigureServices((context, services) =>
            {
                _ = services.ConfigureWritable<Configuration>(context.Configuration.GetSection("Configuration"));

                _ = services.AddSingleton<IInfoBarService, InfoBarService>();

                _ = services.AddSingleton<MainWindow>();
                _ = services.AddSingleton<MainWindowViewModel>();

                _ = services.AddSingleton<ErrorPage>();
                _ = services.AddSingleton<ErrorPageViewModel>();

                _ = services.AddSingleton<HomePage>();
                _ = services.AddSingleton<HomePageViewModel>();

                _ = services.AddSingleton<DownloaderPage>();
                _ = services.AddSingleton<DownloaderPageViewModel>();

                _ = services.AddSingleton<SettingsPage>();
                _ = services.AddSingleton<SettingsPageViewModel>();

                _ = services.AddSingleton<AboutPage>();
                _ = services.AddSingleton<AboutPageViewModel>();
            }
        )
        .Build();

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            await _host.StartAsync();

            var configuration = GetRequiredService<IWritableOptions<Configuration>>();

            if (!string.IsNullOrEmpty(configuration.Value.Language))
            {
                LocalizerHelper.ConfigureLocalizer(configuration.Value.Language);
            }
            else
            {
                LocalizerHelper.ConfigureLocalizer("en");
                configuration.Update(opt => { opt.Language = "en"; });
            }

            var _window = _host.Services.GetRequiredService<MainWindow>();
            _window.Show();

            var infosBarManager = GetRequiredService<IInfoBarService>();

            var downloadPath = configuration.Value.DownloadPath;
            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath!);
                infosBarManager.ShowInfo($"Le dossier \"'{downloadPath}\" a ete crée!");
                configuration.Update(opt => { opt.DownloadPath = downloadPath; });
            }

            /*var ffmpegPath = configuration.CurrentValue.FFMpegPath;
         if (!File.Exists(ffmpegPath))
         {
             infosBarManager.ShowInfo(
                 "Le dossier FfMpeg est introuvable. Vous pouvez éditer le fichier config \"appsettings.json\" avec le bon chemin.");
         }*/
            //TODO TAOST FFMPEGPATH

            ThemeManager.Current.ApplicationTheme = configuration.Value.Theme;


            if (!string.IsNullOrEmpty(configuration.Value.Accent))
            {
                ThemeManager.Current.AccentColor =
                    (Color)ColorConverter.ConvertFromString(configuration.Value.Accent);
            }

            base.OnStartup(e);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Unhandled exception on startup");
            throw;
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        try
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
            _host.Dispose();

            base.OnExit(e);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Unhandled exception on exit");
            throw;
        }
    }

    public static T GetRequiredService<T>()
        where T : class
    {
        return _host.Services.GetRequiredService<T>();
    }

    public static Page GetRequiredPage(Type? type)
    {
        return (Page)_host.Services.GetRequiredService(type!);
    }
}