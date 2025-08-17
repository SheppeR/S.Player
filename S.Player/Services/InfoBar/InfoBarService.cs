using CommunityToolkit.Mvvm.Messaging;
using iNKORE.UI.WPF.Modern.Controls;

namespace S.Player.Services.InfoBar;

public record InfoBarMessage(string Message, InfoBarSeverity Severity);

public class InfoBarService : IInfoBarService
{
    public void ShowError(string message)
    {
        WeakReferenceMessenger.Default.Send(new InfoBarMessage(message, InfoBarSeverity.Error));
    }

    public void ShowInfo(string message)
    {
        WeakReferenceMessenger.Default.Send(new InfoBarMessage(message, InfoBarSeverity.Success));
    }
}