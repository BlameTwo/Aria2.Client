using System.Net.WebSockets;

namespace Aria2.Net.Models.Handler;

public delegate void Aria2ConnectStateChangedDelegate(object source,WebSocketState state);

public delegate void Aria2WebSocketMessageDelegate(object source, WebSocketResultCode result);


