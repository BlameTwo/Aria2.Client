using Microsoft.UI.Xaml;

namespace Aria2.Client.Services.Contracts;

public interface IDialogManager
{
    public XamlRoot Root { get; }

    public void RegisterRoot(XamlRoot root);

    public void ShowAddUri();

    public void ShowAddTorrent();

    public void CloseDialog();
}