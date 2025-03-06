using Aria2.Net.Models.Handler;

namespace Aria2.Net.Services;

partial class Aria2cClient
{
    #region Aria2 WebSocket Event

    public event Aria2DownloadStateChangedEventHandler? Aria2DownloadStateEvent;

    #endregion
}
