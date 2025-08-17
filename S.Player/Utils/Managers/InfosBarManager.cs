using iNKORE.UI.WPF.Modern.Controls;

namespace S.Player.Utils.Managers;

public class InfosBarManager(MainWindow mainWindow)
{
    public void ShowError(string message)
    {
        var infoBar = mainWindow.InfoBar;
        infoBar.Title = "ERROR";
        infoBar.Message = message;
        infoBar.Severity = InfoBarSeverity.Error;
        infoBar.IsOpen = true;
        infoBar.IsClosable = true;
    }

    public void ShowInfo(string message)
    {
        var infoBar = mainWindow.InfoBar;
        infoBar.Title = "INFOS";
        infoBar.Message = message;
        infoBar.Severity = InfoBarSeverity.Success;
        infoBar.IsOpen = true;
        infoBar.IsClosable = true;
    }
}