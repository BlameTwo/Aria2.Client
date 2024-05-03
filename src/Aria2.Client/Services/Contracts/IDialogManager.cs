using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;

namespace Aria2.Client.Services.Contracts;

public interface IDialogManager
{
    public XamlRoot Root { get; }

    public void RegisterRoot(XamlRoot root);

    public Task ShowAddUriAsync();

    public Task ShowAddTorrentAsync();

    public Task ShowAddUriAsync(string url);



    public void CloseDialog();

    public Task<ContentDialogResult> ExitApp();
}