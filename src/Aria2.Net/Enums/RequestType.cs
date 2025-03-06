namespace Aria2.Net.Enums;

public enum RequestType
{
    /// <summary>
    /// WebSocket
    /// </summary>
    WebSocket,

    /// <summary>
    /// Http
    /// </summary>
    Http
}

public enum WebSocketEventType
{
    Start,
    Stop,
    Pause,
    Complete,
    BtComplete,
    Error
}
