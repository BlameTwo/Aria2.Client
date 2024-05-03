using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Aria2.Client.ViewModels.FirstLaunchViewModel;

public sealed partial class HelloAria2ViewModel:ObservableRecipient
{
    public HelloAria2ViewModel()
    {
        
    }

    [ObservableProperty]
    ObservableCollection<string> _NavigateHeader = new()
    {
        "信息",
        "文件设置",
        "主题设置"
    };
}