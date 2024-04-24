using Aria2.Client.Common;
using Aria2.Client.Models;
using Aria2.Client.Services.Contracts;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;

namespace Aria2.Client.Services;


public sealed class ThemeService<T> : IThemeService<T>
    where T : ClientApplication
{
    public ElementTheme Theme
    {
        get => _theme;
        set => _theme = value;
    }

    private T _app;
    private ElementTheme _theme;

    public ILocalSettingsService LocalSetting { get; }
    public string ServiceID { get; set; } = "主体服务";

    public ThemeService(ILocalSettingsService localSetting)
    {
        LocalSetting = localSetting;
    }

    public async Task InitializeAsync(T app)
    {
        try
        {
            var themestr =
                (await LocalSetting.ReadConfig(AppSettingKey.ThemeColor)).ToString()
                ?? ElementTheme.Default.ToString();
            if (!string.IsNullOrWhiteSpace(themestr.ToString()))
            {
                Theme = (ElementTheme)Enum.Parse(typeof(ElementTheme), themestr.ToString());
            }
        }
        catch (Exception)
        {
            Theme = ElementTheme.Default;
        }
        _app = app;

        await Task.CompletedTask;
    }


    public async Task SetRequestedThemeAsync()
    {
        if (_app.MainWindow.Content is FrameworkElement rootelemet)
        {
            rootelemet.RequestedTheme = Theme;
        }

        await Task.CompletedTask;
    }

    private async Task SaveThemeSetting(ElementTheme theme)
    {
        await LocalSetting.SaveConfig("AppTheme", theme.ToString());
    }

    public async Task SetThemeAsync(ElementTheme theme)
    {
        Theme = theme;
        await SetRequestedThemeAsync();
        await SaveThemeSetting(theme);
    }
}
