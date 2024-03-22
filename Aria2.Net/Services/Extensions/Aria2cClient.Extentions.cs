using Aria2.Net.Models.Handler;

namespace Aria2.Net.Services;

partial class Aria2cClient
{
    #region Aria2 WebSocket Event
    internal Aria2DownloadStartDelegate aria2DownloadStartHandler;
    internal Aria2DownloadCompleteDelegate aria2DownloadCompleteHandler;
    internal Aria2DownloadStopDelegate aria2DownloadStopHandler;
    internal Aria2DownloadPauseDelegate aria2DownloadPauseHandler;
    internal Aria2DownloadErrorDelegate aria2DownloadErrorHandler;
    internal Aria2DownloadBtCompleteDelegate aria2DownloadBtCompleteHandler;

    public event Aria2DownloadStartDelegate DownloadStartEvent
    {
        add => aria2DownloadStartHandler += value;
        remove => aria2DownloadStartHandler -= value;
    }

    public event Aria2DownloadCompleteDelegate DownloadCompleteEvent
    {
        add => aria2DownloadCompleteHandler += value;
        remove => aria2DownloadCompleteHandler -= value;
    }

    public event Aria2DownloadStopDelegate DownloadStopEvent
    {
        add => aria2DownloadStopHandler += value;
        remove => aria2DownloadStopHandler -= value;
    }

    public event Aria2DownloadPauseDelegate DownloadCompleteStopEvent
    {
        add => aria2DownloadPauseHandler += value;
        remove => aria2DownloadPauseHandler -= value;
    }

    public event Aria2DownloadErrorDelegate DownloadErrorEvent
    {
        add => aria2DownloadErrorHandler += value;
        remove => aria2DownloadErrorHandler -= value;
    }

    public event Aria2DownloadBtCompleteDelegate DownloadBtCompleteEvent
    {
        add => aria2DownloadBtCompleteHandler += value;
        remove => aria2DownloadBtCompleteHandler -= value;
    }
    #endregion
}
