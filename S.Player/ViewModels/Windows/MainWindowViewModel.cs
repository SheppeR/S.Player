using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace S.Player.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableRecipient
{
    private string? _clockTime = DateTime.Now.ToString("HH:mm").ToUpper();
    private string? _dateDay = DateTime.Now.ToString("dddd").ToUpper();
    private string? _dateMonth = DateTime.Now.ToString("MMMM").ToUpper();
    private string? _dateNumber = DateTime.Today.Day.ToString().ToUpper();

    public MainWindowViewModel()
    {
        var timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        timer.Tick += OnTick;
        timer.Start();
    }

    public string? DateNumber
    {
        get => _dateNumber;
        set => SetProperty(ref _dateNumber, value);
    }

    public string? DateMonth
    {
        get => _dateMonth;
        set => SetProperty(ref _dateMonth, value);
    }

    public string? DateDay
    {
        get => _dateDay;
        set => SetProperty(ref _dateDay, value);
    }

    public string? ClockTime
    {
        get => _clockTime;
        set => SetProperty(ref _clockTime, value);
    }

    private void OnTick(object? sender, EventArgs e)
    {
        DateDay = DateTime.Now.ToString("dddd").ToUpper();
        DateMonth = DateTime.Now.ToString("MMMM").ToUpper();
        DateNumber = DateTime.Today.Day.ToString().ToUpper();
        ClockTime = DateTime.Now.ToString("HH:mm").ToUpper();
    }
}