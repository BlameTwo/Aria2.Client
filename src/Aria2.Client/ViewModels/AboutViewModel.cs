using Aria2.Client.Common.ViewModelBase;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Aria2.Client.ViewModels;

public sealed partial class AboutViewModel : PageViewModelBase
{
    public AboutViewModel() : base("关于")
    {
        this.Licenses = Aria2.Client.Models.Licenses.GetLicenses();
    }

    [ObservableProperty]
    ObservableCollection<Aria2.Client.Models.Licenses> _Licenses;
}
