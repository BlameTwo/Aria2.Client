using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace Aria2.Client.Services.Contracts;

public interface IDialogManager
{
    public XamlRoot Root { get; }

    public void RegisterRoot(XamlRoot root);

    public Task ShowAddUriAsync();

    public Task ShowAddTorrentAsync();

    public void CloseDialog();
}