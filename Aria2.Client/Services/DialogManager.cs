using Aria2.Client.Services.Contracts;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;

namespace Aria2.Client.Services;

public class DialogManager : IDialogManager
{
    public XamlRoot Root { get; private set; }

    public ContentDialog _dialog = null;

    public void CloseDialog()
    {
        if (_dialog != null)
        {
            _dialog.Hide();
        }
    }

    public void RegisterRoot(XamlRoot root)
    {
        this.Root = root;
    }

    public void ShowAddTorrent()
    {

    }

    public void ShowAddUri()
    {

    }
}
