using Aria2.Client.Common.ViewModelBase;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

public sealed partial class AboutViewModel : PageViewModelBase
{
    public AboutViewModel() : base("关于")
    {
        Licenses = Models.Licenses.GetLicenses();
        Init();
    }

    private async void Init()
    {
        MarkdownText = await GetMarkText();
    }

    [ObservableProperty]
    ObservableCollection<Models.Licenses> _Licenses;

    [ObservableProperty]
    string _MarkdownText;

    private static async Task<string> GetMarkText()
    {
        return await File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory + $"Assets\\About.md");
    }
}
