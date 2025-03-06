using Aria2.Net.Models;
using Aria2.Net.Models.ClientModel;
using Aria2.Net.Models.Enums;
using Aria2.Net.Models.Handler;
using Aria2.Net.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Aria2.Net.Common;
using System.IO;
using System.Net.Http;

namespace Aria2.Net.Services;

public partial class Aria2cClient : IAria2cClient
{
    public int ProcessId { get; private set; }

    public ClientWebSocket Socket { get; private set; }

    private Thread _socketThread;

    private bool _isReconnection;

    public event Aria2ConnectStateChangedEventHandler? Aria2ConnectStateChanged;

    public event Aria2WebSocketMessageEventHandler? Aria2WebSocketMessage;


    public Task<ResultCode<string>?> AddUriAsync(
        IEnumerable<string> uriList,
        IDictionary<string, object> options,
        int? location = null,
        CancellationToken token = default
    )
    {
        return RequestAsync<string>(
            GlobalUsings.AddUri_Method,
            token,
            uriList,
            options,
            location
        );
    }

    public Task<ResultCode<string>?> ChangeGlobalOption(IDictionary<string, string> queries,
        CancellationToken token = default)
    {
        return RequestAsync<string>(
            GlobalUsings.ChangeGlobalOption_Method,
            token,
            queries
        );
    }

    public Task<ResultCode<Aria2cOption>?> GetGlobalOption(CancellationToken token = default)
    {
        return RequestAsync<Aria2cOption>(GlobalUsings.GetGlobalOption_Method, token);
    }

    public Task<ResultCode<FileDownloadTell>?> GetTellStatusAsync(
        string gid,
        CancellationToken token = default
    )
    {
        return RequestAsync<FileDownloadTell>(
            GlobalUsings.GetTellStatus_Method,
            token,
            gid
        );
    }

    public Task<ResultCode<List<FileDownloadTell>>?> GetAllTellActiveAsync(
        CancellationToken token = default
    )
    {
        return RequestAsync<List<FileDownloadTell>>(
            GlobalUsings.GetTellActive_Method,
            token,
            null
        );
    }

    private async Task<ResultCode<T>?> RequestAsync<T>(
        string method,
        CancellationToken token,
        params IEnumerable<object?>? parameters
    )
    {
        var request = new Aria2Request
        {
            Id = "aria2request",
            Jsonrpc = "2.0",
            Method = method,
            Parameters = parameters?.Where(t => t is not null).ToList()!
        };

        var jsonRequest = JsonSerializer.Serialize(request);
        if (Socket.State != WebSocketState.Open)
            return null;
        // todo: cache client
        using var client = new HttpClient();
        var pos = await client.PostAsync(
            GlobalUsings.HttpRequestBaseUrl,
            new StringContent(jsonRequest, Encoding.UTF8, "application/json"), token);
        if (pos.IsSuccessStatusCode)
        {
            var result = await pos.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<ResultCode<T>>(result)!;
        }

        return null;
    }

    public Task LauncherAsync(Aria2LauncherConfig? config = null)
    {
        return Task.Run(() =>
        {
            if (config == null)
            {
                using var ps = new Process();
                ps.StartInfo = new()
                {
                    FileName = GlobalUsings.Aria2Path,
                    Arguments = $"--enable-rpc=true --rpc-listen-port={GlobalUsings.Port} --rpc-listen-all=true",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = false,
                };
                ps.Start();
                ProcessId = ps.Id;
            }
            else
            {
                var workingDirectory = "";
                var argument = $" --enable-rpc=true --rpc-listen-port={GlobalUsings.Port} --rpc-listen-all=true";
                if (config.BtTracker != null)
                {
                    argument += $" --bt-tracker={string.Join(",", config.BtTracker)}";
                }

                if (!string.IsNullOrWhiteSpace(config.SessionFilePath))
                {
                    argument += $" --save-session=\"{config.SessionFilePath}\"";
                    if (System.IO.File.Exists(config.SessionFilePath))
                    {
                        argument += $" --input-file=\"{config.SessionFilePath}\"";
                    }

                    argument += " --save-session-interval=30";
                    workingDirectory = Path.GetDirectoryName(config.SessionFilePath);
                }

                if (!string.IsNullOrWhiteSpace(config.LogFilePath))
                {
                    argument += $" --log=\"{config.LogFilePath}\"";
                }

                if (!string.IsNullOrWhiteSpace(config.MaxDownloadSpeed))
                {
                    argument += $" --max-download-limit=\"{config.MaxDownloadSpeed}\"";
                }

                if (!string.IsNullOrWhiteSpace(config.MaxUploadSpeed))
                {
                    argument += $" --max-upload-limit=\"{config.MaxDownloadSpeed}\"";
                }

                var ps = new Process
                {
                    StartInfo = new()
                    {
                        FileName = GlobalUsings.Aria2Path,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        WorkingDirectory = workingDirectory,
                        Arguments = argument
                    }
                };

                ps.Start();
                ProcessId = ps.Id;
            }

            return Task.CompletedTask;
        });
    }

    public async Task ExitAria2(CancellationToken token = default)
    {
        await PauseAllTask(token);
        Process.GetProcessById(ProcessId).Kill();
    }

    public Task<ResultCode<string>?> PauseAllTask(CancellationToken token = default) => RequestAsync<string>(GlobalUsings.PauseAll_Method, token, null);

    public Task<ResultCode<string>?> PauseTask(string gid, CancellationToken token = default) => RequestAsync<string>(GlobalUsings.Pause_Method, token, gid);

    public Task<ResultCode<string>?> ForcePause(string gid, CancellationToken token = default) => RequestAsync<string>(GlobalUsings.ForcePause, token, gid);

    public Task<ResultCode<List<string>>?> ForcePauseAll(CancellationToken token = default) => RequestAsync<List<string>>(GlobalUsings.PauseAll_Method, token);

    public Task<ResultCode<string>?> ForceRemove(string gid, CancellationToken token = default) => RequestAsync<string>(GlobalUsings.ForceRemove, token, gid);

    public Task<ResultCode<string>?> ChangeGlobalOption(
        Aria2GlobalOptionEnum type,
        string value,
        CancellationToken token = default
    ) => ChangeGlobalOption(
        new Dictionary<string, string>
        {
            { GlobalOptionConvert.EnumToString(type), value }
        },
        token
    );

    public async Task<bool> ChangeGlobalOptions(
        Dictionary<Aria2GlobalOptionEnum, string> keyValues,
        CancellationToken token = default
    )
    {
        List<bool> flags = [];
        foreach (var value in keyValues)
        {
            var result = await ChangeGlobalOption(value.Key, value.Value, token);
            if (result.Result != "OK")
                flags.Add(true);
        }

        return !flags.Contains(true);
    }



    public async Task<ResultCode<string>?> AddTorrentAsync(
        string torrentPath,
        IDictionary<string, object> options,
        int? location = null,
        CancellationToken token = default
    )
    {
        var bytes = await System.IO.File.ReadAllBytesAsync(torrentPath, token);
        return await RequestAsync<string>(
            GlobalUsings.AddTorrent_Method,
            token,
            Convert.ToBase64String(bytes),
            new List<string>(),
            options,
            location
        );
    }

    public Task<ResultCode<string>?> UnpauseAll(CancellationToken token = default) => RequestAsync<string>("aria2.unpauseAll", token);

    public Task<ResultCode<string>?> Unpause(string gid, CancellationToken token = default) => RequestAsync<string>("aria2.unpause", token, gid);

    public Task<ResultCode<TellOption>?> GetTellOption(string gid, CancellationToken token = default) => RequestAsync<TellOption>("aria2.getOption", token, gid);

    public Task<ResultCode<AllTellStatus>?> GetAllTellStatus(CancellationToken token = default) => RequestAsync<AllTellStatus>("aria2.getGlobalStat", token);

    public async Task<ResultCode<object>?> MultiCallRequestAsync(
        CancellationToken token = default,
        params IEnumerable<IReadOnlyList<object>> args
    )
    {
        var request = new List<MulticallRequest>();
        foreach (var item in args)
        {
            var parameter = item.Skip(1);
            request.Add(
                new()
                {
                    MethodName = (string)item[0],
                    Parameters = parameter.ToList()
                }
            );
        }

        var result = await RequestAsync<object>("system.multicall", token, request);
        return result;
    }

    public Task<ResultCode<List<FileDownloadTell>>?> GetWaitingTaskAsync(
        int offset = 0,
        int pagesize = 1000,
        CancellationToken token = default
    )
    {
        return RequestAsync<List<FileDownloadTell>>(
            "aria2.tellWaiting",
            token,
            offset,
            pagesize
        );
    }

    public Task<ResultCode<List<FileDownloadTell>>?> GetStoppedTaskAsync(
        int offset = 0,
        int pagesize = 1000,
        CancellationToken token = default
    )
    {
        return RequestAsync<List<FileDownloadTell>>(
            "aria2.tellStopped",
            token,
            offset,
            pagesize
        );
    }

    public async Task<ResultCode<List<FileDownloadTell>>> GetAllTaskAsync(
        CancellationToken token = default
    )
    {
        var result = await MultiCallRequestAsync(
            token,
            ["aria2.tellStopped", 0, 1000],
            ["aria2.tellWaiting", 0, 1000],
            ["aria2.tellActive"]
        );
        return new();
    }

    public Task<ResultCode<string>?> RemoveTask(string gid, CancellationToken token = default)
    {
        return RequestAsync<string>("aria2.remove", token, gid);
    }

    public async Task<WebSocketState> ConnectAsync(CancellationToken token = default)
    {
        if (_isReconnection)
            return WebSocketState.Connecting;
        _socketThread = new(SocketThread)
        {
            IsBackground = true
        };
        Socket = new();
        Socket.Options.RemoteCertificateValidationCallback = (_, _, _, _) => true;
        _isReconnection = true;
        await Socket.ConnectAsync(new(GlobalUsings.WebSocketBaseUrl), token);
        _socketThread.Start();
        Aria2ConnectStateChanged?.Invoke(this, Socket.State);
        _isReconnection = false;
        return Socket.State;
    }

    private async void SocketThread(object? obj)
    {
        var buffer = new byte[1024 * 1024];
        while (true)
        {
            try
            {
                Thread.Sleep(1000);
                var result = await Socket.ReceiveAsync(
                    new(buffer),
                    CancellationToken.None
                );
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                    {
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var jsonObject = JsonSerializer.Deserialize<WebSocketResultCode>(
                            message
                        )!;
                        Aria2WebSocketMessage?.Invoke(
                            this, jsonObject

                        );
                        OnWebSocketEvent(jsonObject);
                        break;
                    }
                    case WebSocketMessageType.Close:
                        Aria2ConnectStateChanged?.Invoke(this, Socket.State);
                        await DisconnectAsync();
                        break;
                }
            }
            catch (Exception)
            {
                Aria2ConnectStateChanged?.Invoke(this, Socket.State);
            }

        }
    }

    private void OnWebSocketEvent(WebSocketResultCode message)
    {
        switch (message.Method)
        {
            case Aria2Socket_Method.OnDownloadStart:
                Aria2DownloadStateEvent?.Invoke(Enums.WebSocketEventType.Start, message);
                break;
            case Aria2Socket_Method.OnDownloadPause:
                Aria2DownloadStateEvent?.Invoke(Enums.WebSocketEventType.Pause, message);
                break;
            case Aria2Socket_Method.OnDownloadStop:
                Aria2DownloadStateEvent?.Invoke(Enums.WebSocketEventType.Stop, message);
                break;
            case Aria2Socket_Method.OnDownloadComplete:
                Aria2DownloadStateEvent?.Invoke(Enums.WebSocketEventType.Complete, message);
                break;
            case Aria2Socket_Method.OnDownloadError:
                Aria2DownloadStateEvent?.Invoke(Enums.WebSocketEventType.Error, message);
                break;
            case Aria2Socket_Method.OnBtDownloadComplete:
                Aria2DownloadStateEvent?.Invoke(Enums.WebSocketEventType.BtComplete, message);
                break;
        }
    }

    public async Task<(WebSocketState, WebSocketCloseStatus?)> DisconnectAsync(
        CancellationToken token = default
    )
    {
        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Error", token);
        return (Socket.State, Socket.CloseStatus);
    }

    public Task<ResultCode<string>?> Aria2RemoveDownloadResult(string gid, CancellationToken token = default)
    {
        return RequestAsync<string>("aria2.removeDownloadResult", token, gid);
    }

    public Task<ResultCode<string>?> Aria2RemoveAllDownloadResult(CancellationToken token = default)
    {
        return RequestAsync<string>("aria2.purgeDownloadResult", token);
    }

    public Task<ResultCode<List<DownloadFile>>?> GetFiles(string gid, CancellationToken token = default)
    {
        return RequestAsync<List<DownloadFile>>("aria2.getFiles", token, gid);
    }

    public Task<ResultCode<string>?> ChangeTellOption(string gid, Dictionary<string, object> values,
        CancellationToken token = default)
    {
        return RequestAsync<string>("aria2.changeOption", token, gid, values);
    }

    public async Task<WebSocketState> ReconnectionSocket(CancellationToken token = default)
    {
        Socket = new();
        return await ConnectAsync(token);
    }

    public Task<ResultCode<List<BittorrentPeer>>?> GetBittorrentPeers(string gid,
        CancellationToken token = default)
    {
        return RequestAsync<List<BittorrentPeer>>("aria2.getPeers", token, gid);
    }

    public async Task<IpData> GetIpAsync(string ip, CancellationToken token = default)
    {
        var uri = $"https://opendata.baidu.com/api.php?query={ip}&co=&resource_id=6006&oe=utf8";
        var client = new HttpClient();
        var result = await client.GetAsync(uri, token);
        return JsonSerializer.Deserialize<IpData>(await result.Content.ReadAsStringAsync(token))!;
    }
}
