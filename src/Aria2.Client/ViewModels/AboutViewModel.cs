using Aria2.Client.Common.ViewModelBase;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Aria2.Client.ViewModels;

public sealed partial class AboutViewModel : PageViewModelBase
{
    public AboutViewModel() : base("关于")
    {
        this.Licenses = Aria2.Client.Models.Licenses.GetLicenses();
        Init();
    }

    private async void Init()
    {
        this.MarkdownText = await GetMarkText();
    }

    [ObservableProperty]
    ObservableCollection<Aria2.Client.Models.Licenses> _Licenses;

    [ObservableProperty]
    string _MarkdownText;

    private static async Task<string> GetMarkText()
    {
        return await File.ReadAllTextAsync(AppDomain.CurrentDomain.BaseDirectory + $"Assets\\About.md");
    }
}
