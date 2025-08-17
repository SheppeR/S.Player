using iNKORE.UI.WPF.Modern.Controls;
using S.Player.Pages;

namespace S.Player;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
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
            var item = sender.SelectedItem as NavigationViewItem;
            var type = Type.GetType($"S.Player.Pages.{item?.Name}, S.Player");
            var page = App.GetRequiredPage(type ?? typeof(ErrorPage));

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