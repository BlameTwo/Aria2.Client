using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aria2.Client.Models;

public class Licenses
{
    public string Name { get; set; }

    public Licenses(string name, string webOrg,string language)
    {
        Name = name;
        WebOrg = webOrg;
        Language = language;
    }

    public string WebOrg { get; set; }
    public string Language { get; }

    public static ObservableCollection<Licenses> GetLicenses()
    {
        return new ObservableCollection<Licenses>()
        {
            new("CommunityToolkit.Mvvm", "https://github.com/CommunityToolkit/dotnet","C#"),
            new(
                "CommunityToolkit.WinUI.Controls.SettingsControls",
                "https://github.com/CommunityToolkit/Windows","C#"
            ),
            new("CommunityToolkit.WinUI.Converters", "https://github.com/CommunityToolkit/Windows", "C#"),
            new("H.NotifyIcon.WinUI","https://github.com/HavenDV/H.NotifyIcon","C#"),
            new("LiveChartsCore.SkiaSharpView.WinUI", "https://github.com/beto-rodriguez/LiveCharts2", "C#"),
            new("Microsoft.Extensions.DependencyInjection","https://dot.net/","C#"),
            new("Microsoft.WindowsAppSDK","https://github.com/microsoft/windowsappsdk","C++"),
            new("Microsoft.Windows.SDK.BuildTools","https://www.nuget.org/packages/Microsoft.Windows.SDK.Contracts", "C#"),
            new("Microsoft.Xaml.Behaviors.WinUI.Managed","https://github.com/Microsoft/XamlBehaviors","C#"),
            new("System.Drawing.Common","https://github.com/dotnet/winforms","C#"),
            new("WinUIEx","https://github.com/dotMorten/WinUIEx", "C#")
        };
    }
}
