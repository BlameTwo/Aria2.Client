using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using LiveChartsCore.Kernel.Sketches;
using Aria2.Net.Models.ClientModel;

namespace Aria2.Client.ViewModels;

partial class OverviewViewModel
{

    private readonly ObservableCollection<ObservableValue> _downloadSpeed;
    private readonly ObservableCollection<ObservableValue> _uploadSpeed;
    public DispatcherTimer Timer { get; private set; }

    [ObservableProperty]
    IEnumerable<ICartesianAxis> _SpeedY = new Axis[]
    {
        new Axis()
        {
            IsVisible = true,
            
        }
    };

    [ObservableProperty]
    IEnumerable<ICartesianAxis> _SpeedX = new Axis[]
    {
        new TimeSpanAxis(TimeSpan.FromDays(1), date => $"{date.Hours}:{date.Minutes}:{date.Seconds}")
        {
            MaxLimit = 100,
            IsVisible = false
        }
    };

    [ObservableProperty]
    AllTellStatus _Space;

    partial void OnSpaceChanged(AllTellStatus value)
    {
        _downloadSpeed.Add(new(double.Parse(value.DownloadSpeed)));
        if (_downloadSpeed.Count > 100)
        {
            _downloadSpeed.RemoveAt(0);
        }
        _uploadSpeed.Add(new(double.Parse(value.UploadSpeed)));
        if (_uploadSpeed.Count > 100)
        {
            _uploadSpeed.RemoveAt(0);
        }
    }

    void InitChart()
    {
        Timer = new DispatcherTimer();
        Timer.Interval = TimeSpan.FromSeconds(1);
        Timer.Tick += Timer_Tick;
        Timer.Start();
        Series = new ObservableCollection<ISeries>()
        {
            new LineSeries<ObservableValue>() { Values = _downloadSpeed,Fill = null,GeometrySize = 0,MiniatureShapeSize=5 },
            new LineSeries<ObservableValue>() { Values = _uploadSpeed,Fill = null,GeometrySize = 0 ,MiniatureShapeSize=5,}
        };
    }

    private async void Timer_Tick(object sender, object e)
    {
        var tell = await Aria2CClient.GetAllTellStatus();
        Space = tell.Result;
    }
}
