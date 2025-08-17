using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using S.Player.Options;
using S.Player.Utils.Options;
using S.Player.ViewModels.Pages;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Exceptions;

namespace S.Player.Models;

public partial class DownloadLinkModel(string link) : ObservableRecipient
{
    private readonly string? _downloadPath =
        App.GetRequiredService<IWritableOptions<Configuration>>().Value.DownloadPath;

    private readonly string? _ffmpegPath = App.GetRequiredService<IWritableOptions<Configuration>>().Value.FFMpegPath;
    private string? _link = link;
    private Uri? _poster;
    private double? _progress;
    private TimeSpan? _time;
    private string? _title;
    private YoutubeClient? _youtube;

    public string? Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public TimeSpan? Time
    {
        get => _time;
        set => SetProperty(ref _time, value);
    }

    public string? Link
    {
        get => _link;
        set => SetProperty(ref _link, value);
    }

    public double? Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }

    public Uri? Poster
    {
        get => _poster;
        set => SetProperty(ref _poster, value);
    }

    private void OnProgress(double obj)
    {
        Progress = obj * 100;
    }

    public async Task<Task> DownloadLink(CancellationToken token)
    {
        try
        {
            Progress = 0;

            _youtube = new YoutubeClient();

            await _youtube.Videos.DownloadAsync(Link!, $"{_downloadPath}/{Title}.mp3", o => o
                    .SetContainer("mp3")
                    .SetFFmpegPath(_ffmpegPath!)
                , new Progress<double>(OnProgress), token);
        }
        catch (YoutubeExplodeException e)
        {
            return Task.FromException(e);
        }

        Progress = 100;
        return Task.CompletedTask;
    }

    [RelayCommand]
    public async Task Delete()
    {
        await Task.Run(() => { File.Delete($"{_downloadPath}/{Title}.mp3"); });
        App.GetRequiredService<DownloaderPageViewModel>().LinkList.Remove(this);
    }
}