using S.Player.ViewModels.Pages;
using S.Player.ViewModels.Windows;

namespace S.Player.ViewModels;

public class ViewModelLocator
{
    public MainWindowViewModel MainWindow => App.GetRequiredService<MainWindowViewModel>();

    public AboutPageViewModel AboutPage => App.GetRequiredService<AboutPageViewModel>();

    public DownloaderPageViewModel DownloaderPage => App.GetRequiredService<DownloaderPageViewModel>();

    public ErrorPageViewModel ErrorPage => App.GetRequiredService<ErrorPageViewModel>();

    public HomePageViewModel HomePage => App.GetRequiredService<HomePageViewModel>();

    public SettingsPageViewModel SettingsPage => App.GetRequiredService<SettingsPageViewModel>();
}