using Microsoft.UI.Xaml;

namespace Aria2.Client.Common;


public class TitleBarModeChangedArgs: RoutedEventArgs
{

}

public delegate void TitleBarModeChangedDelegate(object sender, TitleBarModeChangedArgs e);