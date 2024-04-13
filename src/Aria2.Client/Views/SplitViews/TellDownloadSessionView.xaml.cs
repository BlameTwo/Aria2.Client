using Aria2.Client.ViewModels.SplitViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.SplitViews;

public sealed partial class TellDownloadSessionView : UserControl
{
    public TellDownloadSessionView()
    {
        this.InitializeComponent();
    }

    public TellDownloadSessionViewModel ViewModel
    {
        get { return (TellDownloadSessionViewModel)GetValue(ViewModelProperty); }
        set { SetValue(ViewModelProperty, value); }
    }

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        "ViewModel",
        typeof(TellDownloadSessionViewModel),
        typeof(TellDownloadSessionView),
        new PropertyMetadata(null)
    );
}
