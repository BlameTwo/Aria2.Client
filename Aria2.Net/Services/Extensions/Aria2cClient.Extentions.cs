using Aria2.Net.Models.Handler;

namespace Aria2.Net.Services;

partial class Aria2cClient
{
    #region Aria2 WebSocket Event
    internal Aria2DownloadStateChangedDelegate aria2DownloadStateChangedHandler;

    public event Aria2DownloadStateChangedDelegate Aria2DownloadStateEvent
    {
        add => aria2DownloadStateChangedHandler += value;
        remove => aria2DownloadStateChangedHandler -= value;
    }

    #endregion
}
