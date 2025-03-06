using IBtSearch;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Views.DateTemplate;

public sealed partial class PluginItemTemplate : UserControl
{
    public PluginItemTemplate()
    {
        InitializeComponent();
    }

    public IBTSearchPlugin Data
    {
        get { return (IBTSearchPlugin)GetValue(DataProperty); }
        set { SetValue(DataProperty, value); }
    }

    public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
        "Data",
        typeof(IBTSearchPlugin),
        typeof(PluginItemTemplate),
        new PropertyMetadata(null,OnDataChanged)
    );

    private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {

    }

    private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {

    }
}
