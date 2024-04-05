using Microsoft.UI.Xaml.Controls;

namespace Aria2.Client.Services.Contracts;

public interface ITipShow
{
    void ShowMessage(string message, Symbol icon);

    Panel Owner { get; set; }
}
