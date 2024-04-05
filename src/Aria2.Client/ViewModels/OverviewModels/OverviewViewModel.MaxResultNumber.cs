using CommunityToolkit.Mvvm.ComponentModel;

namespace Aria2.Client.ViewModels;

partial class OverviewViewModel
{
    [ObservableProperty]
    int _MaxResult;

    partial void OnMaxResultChanged(int oldValue, int newValue)
    {

    }
}
