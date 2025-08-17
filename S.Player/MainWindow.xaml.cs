using CommunityToolkit.Mvvm.Messaging;
using iNKORE.UI.WPF.Modern.Controls;
using S.Player.Pages;
using S.Player.Services.InfoBar;

namespace S.Player;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<InfoBarMessage>(this, (_, msg) =>
        {
            InfoBar.Title = msg.Severity == InfoBarSeverity.Error ? "ERROR" : "INFO";
            InfoBar.Message = msg.Message;
            InfoBar.Severity = msg.Severity;
            InfoBar.IsOpen = true;
            InfoBar.IsClosable = true;
        });

        Loaded += (_, _) => { NavigationView.SelectedItem = HomePage; };

        // await InputBox.ShowAsync("title", "msg", "test");
    }

    private void OnSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.IsSettingsSelected)
        {
            if (NavigationFrame.CurrentSourcePageType != typeof(SettingsPage))
            {
                NavigationFrame.Navigate(typeof(SettingsPage));
            }
        }
        else
        {
            var pageType = args.SelectedItemContainer?.Tag as Type ?? typeof(ErrorPage);
            var page = App.GetRequiredPage(pageType);
            NavigationFrame.Navigate(page);
        }
    }

    private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
    {
        if (NavigationFrame.CanGoBack)
        {
            NavigationFrame.GoBack();
        }
    }
}