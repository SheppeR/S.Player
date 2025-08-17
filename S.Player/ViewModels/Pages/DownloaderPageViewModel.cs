using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using S.Player.Models;
using S.Player.Utils.Extensions;
using S.Player.Utils.Managers;
using YoutubeExplode;
using YoutubeExplode.Exceptions;

namespace S.Player.ViewModels.Pages;

public partial class DownloaderPageViewModel(InfosBarManager infosBarManager) : ObservableRecipient
{
    private ObservableCollection<DownloadLinkModel> _linkList = [];
    private YoutubeClient _youtube = null!;
    private string? _youtubeLink;

    public string? YoutubeLink
    {
        get => _youtubeLink;
        set => SetProperty(ref _youtubeLink, value);
    }

    public ObservableCollection<DownloadLinkModel> LinkList
    {
        get => _linkList;
        set => SetProperty(ref _linkList, value);
    }

    [RelayCommand]
    public async Task LoadLink()
    {
        var cts = new CancellationTokenSource();
        try
        {
            if (_youtubeLink != null)
            {
                if (!_youtubeLink.StartsWith("http"))
                {
                    return;
                }

                _youtube = new YoutubeClient();

                var video = await _youtube.Videos.GetAsync(_youtubeLink, cts.Token);

                var title = video.Title.Validate();
                var time = video.Duration;
                var poster = video.Thumbnails.FirstOrDefault()?.Url;

                var uri = poster != null ? new Uri(poster) : new Uri("/Assets/nodata.png", UriKind.Relative);

                var item = new DownloadLinkModel(_youtubeLink)
                {
                    Title = title,
                    Poster = uri,
                    Time = time
                };

                _linkList.Add(item);
                _ = item.DownloadLink(cts.Token);
            }
        }
        catch (Exception ex) //( ex1 ||   ex2)
        {
            if (ex is YoutubeExplodeException || ex is ArgumentException)
            {
                infosBarManager.ShowError(ex.Message);
            }
        }

        YoutubeLink = string.Empty;
    }
}