using iNKORE.UI.WPF.Modern;

namespace S.Player.Options;

public class Configuration
{
    public string? DownloadPath { get; set; }

    public string? FFMpegPath { get; set; }

    public ApplicationTheme Theme { get; set; }

    public string? Accent { get; set; }
}