using Aria2.Client.Models.Bases;
using Aria2.Client.Models.Messagers;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;

namespace Aria2.Client.Models;

public partial class AppMessageItemData: ItemDownloadBase<AppNotifyMessager>
{
    [RelayCommand]
    void Close()
    {
        WeakReferenceMessenger.Default.Send<Tuple<bool, AppNotifyMessager>>(
            new(false, this.Data)
        );
    }
}
