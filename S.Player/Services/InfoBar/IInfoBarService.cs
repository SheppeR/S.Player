namespace S.Player.Services.InfoBar;

public interface IInfoBarService
{
    void ShowError(string message);
    void ShowInfo(string message);
}