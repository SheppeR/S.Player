using System.ComponentModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using iNKORE.UI.WPF.Helpers;
using iNKORE.UI.WPF.Modern;
using S.Player.Options;
using S.Player.Utils.Options;

namespace S.Player.ViewModels.Pages;

public partial class SettingsPageViewModel : ObservableRecipient
{
    private readonly IWritableOptions<Configuration> _options;
    private Color? _accentColor;

    private Color _actualAccentColor;

    private ApplicationTheme _actualApplicationTheme;

    private ApplicationTheme? _applicationTheme;
    private bool _isLight;
    private bool _updatingAccentColor;
    private bool _updatingApplicationTheme;

    public SettingsPageViewModel(IWritableOptions<Configuration> options)
    {
        _options = options;
        IsLight = ThemeManager.Current.ApplicationTheme != ApplicationTheme.Dark;
        DispatcherHelper.RunOnMainThread(() =>
        {
            DependencyPropertyDescriptor.FromProperty(ThemeManager.ApplicationThemeProperty, typeof(ThemeManager))
                .AddValueChanged(ThemeManager.Current, delegate { UpdateApplicationTheme(); });

            DependencyPropertyDescriptor
                .FromProperty(ThemeManager.ActualApplicationThemeProperty, typeof(ThemeManager))
                .AddValueChanged(ThemeManager.Current, delegate { UpdateActualApplicationTheme(); });

            DependencyPropertyDescriptor.FromProperty(ThemeManager.AccentColorProperty, typeof(ThemeManager))
                .AddValueChanged(ThemeManager.Current, delegate { UpdateAccentColor(); });

            DependencyPropertyDescriptor.FromProperty(ThemeManager.ActualAccentColorProperty, typeof(ThemeManager))
                .AddValueChanged(ThemeManager.Current, delegate { UpdateActualAccentColor(); });

            UpdateApplicationTheme();
            UpdateActualApplicationTheme();
            UpdateAccentColor();
            UpdateActualAccentColor();
        });
    }

    public bool IsLight
    {
        get => _isLight;
        set
        {
            SetProperty(ref _isLight, value);
            ToggleTheme(_isLight);
        }
    }

    public ApplicationTheme? Theme
    {
        get => _applicationTheme;
        set
        {
            if (_applicationTheme != value)
            {
                SetProperty(ref _applicationTheme, value);

                if (!_updatingApplicationTheme)
                {
                    DispatcherHelper.RunOnMainThread(() =>
                    {
                        ThemeManager.Current.ApplicationTheme = _applicationTheme;
                    });
                }
            }
        }
    }

    public ApplicationTheme ActualApplicationTheme
    {
        get => _actualApplicationTheme;
        private set => SetProperty(ref _actualApplicationTheme, value);
    }

    public Color? AccentColor
    {
        get => _accentColor;
        set
        {
            if (_accentColor != value)
            {
                SetProperty(ref _accentColor, value);

                if (!_updatingAccentColor)
                {
                    DispatcherHelper.RunOnMainThread(() =>
                    {
                        ThemeManager.Current.AccentColor = _accentColor;
                        _options.Update(opt =>
                        {
                            if (ThemeManager.Current.AccentColor != null)
                            {
                                opt.Accent = ThemeManager.Current.AccentColor.Value.ToString();
                            }
                        });
                    });
                }
            }
        }
    }

    public Color ActualAccentColor
    {
        get => _actualAccentColor;
        private set => SetProperty(ref _actualAccentColor, value);
    }

    private void ToggleTheme(bool isLight)
    {
        ThemeManager.Current.ApplicationTheme = isLight ? ApplicationTheme.Light : ApplicationTheme.Dark;
        _options.Update(opt =>
        {
            if (ThemeManager.Current.ApplicationTheme != null)
            {
                opt.Theme = ThemeManager.Current.ApplicationTheme.Value;
            }
        });
    }

    private void UpdateApplicationTheme()
    {
        _updatingApplicationTheme = true;
        Theme = ThemeManager.Current.ApplicationTheme;
        _updatingApplicationTheme = false;
    }

    private void UpdateActualApplicationTheme()
    {
        ActualApplicationTheme = ThemeManager.Current.ActualApplicationTheme;
    }

    private void UpdateAccentColor()
    {
        _updatingAccentColor = true;
        AccentColor = ThemeManager.Current.AccentColor;
        _updatingAccentColor = false;
    }

    private void UpdateActualAccentColor()
    {
        ActualAccentColor = ThemeManager.Current.ActualAccentColor;
    }
}