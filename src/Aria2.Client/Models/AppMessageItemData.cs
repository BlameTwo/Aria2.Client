using Aria2.Client.Models.Bases;
using Aria2.Client.Models.Messagers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using System;

namespace Aria2.Client.Models;

public partial class AppMessageItemData: ItemDownloadBase<AppNotifyMessager>
{
    public TimeSpan DelayTime { get; private set; }
    public TimeSpan NowTime { get; private set; }
    [RelayCommand]
    void Close()
    {
        WeakReferenceMessenger.Default.Send<Tuple<bool, AppNotifyMessager>>(
            new(false, this.Data)
        );
    }

    [ObservableProperty]
    TimeSpan _DelayValue;

    [ObservableProperty]
    Visibility _CloseButtonVisibility = Visibility.Visible;

    [ObservableProperty]
    Visibility _TimeSpanVisibility = Visibility.Collapsed;

    public override void SetData(AppNotifyMessager data)
    {
        base.SetData(data);
        if (Data.DelayTime == TimeSpan.Zero) return;
        this.DelayTime = Data.DelayTime;
        TimeSpanVisibility = Visibility.Visible;
        CloseButtonVisibility = Visibility.Collapsed;
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (sender, s) =>
        {
            NowTime = NowTime.Add(TimeSpan.FromSeconds(1));
            var value = DelayTime - NowTime;
            this.DelayValue = value;
            if(DelayValue == TimeSpan.Zero)
            {
                timer.Stop();
                Close();
            }
        };
        timer.Start();
    }
}
