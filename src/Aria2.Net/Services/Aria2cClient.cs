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

    private Aria2ConnectStateChangedDelegate _aria2ConnectStateHandler;

    private Aria2WebSocketMessageDelegate Aria2WebSocketMessageDelegate;

    private bool _isReconnection=false;
    

    public async Task<ResultCode<string>> AddUriAsync(
        IEnumerable<string> uriList,
        IDictionary<string, object> options,
        int? location = null,
        CancellationToken token = default
    )
    {
        return await this.RequestAsync<string>(
            GlobalUsings.AddUri_Method,
            token,
            uriList,
            options,
            location
        );
    }

    public async Task<ResultCode<string>> ChangeGlobalOption(
        IDictionary<string, string> querys,
        CancellationToken token = default
    )
    {
        return await this.RequestAsync<string>(
            GlobalUsings.ChangeGlobalOption_Method,
            token,
            querys
        );
    }

    public async Task<ResultCode<Aria2cOption>> GetGlobalOption(CancellationToken token = default)
    {
        return await this.RequestAsync<Aria2cOption>(GlobalUsings.GetGlobalOption_Method, token);
    }

    public async Task<ResultCode<FileDownloadTell>> GetTellStatusAsync(
        string gid,
        CancellationToken token = default
    )
    {
        return await this.RequestAsync<FileDownloadTell>(
            GlobalUsings.GetTellStatus_Method,
            token,
            gid
        );
    }

    public async Task<ResultCode<List<FileDownloadTell>>> GetAllTellActiveAsync(
        CancellationToken token = default
    )
    {
        return await this.RequestAsync<List<FileDownloadTell>>(
            GlobalUsings.GetTellActive_Method,
            token,
            null
        );
    }

    private async Task<ResultCode<T>> RequestAsync<T>(
        string method,
        CancellationToken token,
        params object[]? parames
    )
    {
        var request = new Aria2Request
        {
            Id = "aria2request",
            Jsonrpc = "2.0",
            Method = method,
            Parameters = new List<object?>()
        };

        if (parames != null && parames.Length > 0)
        {
            foreach (var par in parames.Where(m => m != null))
            {
                request.Parameters.Add(par);
            }
        }
        var jsonRequest = JsonSerializer.Serialize(request);
        if (Socket.State != WebSocketState.Open)
            return null;
        HttpClient client = new HttpClient();
        var pos = await client.PostAsync(
            GlobalUsings.HttpRequetBaseUrl,
            new StringContent(jsonRequest, Encoding.UTF8, "application/json")
        );
        if (pos.IsSuccessStatusCode)
        {
            var result = await pos.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ResultCode<T>>(result)!;
        }
        return null;
    }

    public async Task LauncherAsync()
    {
        await Task.Run(() =>
        {
            Process ps = new Process();
            ps.StartInfo = new ProcessStartInfo()
            {
                FileName = GlobalUsings.Aria2Path,
                Arguments = $"--enable-rpc=true --rpc-listen-port={GlobalUsings.Port} --rpc-listen-all=true",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = false,
            };
            ps.Start();
            this.ProcessId = ps.Id;
        });
    }
    
    public async Task LauncherAsync(Aria2LauncherConfig config= null)
    {
        await Task.Run(async() =>
        {
            if (config == null)
            {
                await LauncherAsync();
                return;
            }
            Process ps = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = GlobalUsings.Aria2Path;
            info.RedirectStandardError = true;
            string argument = $" --enable-rpc=true --rpc-listen-port={GlobalUsings.Port} --rpc-listen-all=true";
            if (config.BtTracker != null)
            {
                argument += $" --bt-tracker={string.Join(",", config.BtTracker)}";
            }

            if (!string.IsNullOrWhiteSpace(config.SesionFilePath))
            {
                argument += $" --save-session=\"{config.SesionFilePath}\"";
                if (System.IO.File.Exists(config.SesionFilePath))
                {
                    argument += $" --input-file=\"{config.SesionFilePath}\"";
                }
                argument += $" --save-session-interval=30";
                info.WorkingDirectory = Path.GetDirectoryName(config.SesionFilePath);
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
            info.UseShellExecute = false;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            info.CreateNoWindow = true;
            info.Arguments = argument;
            ps.StartInfo = info;
            ps.Start();
            this.ProcessId = ps.Id;
        });
    }

    public async Task ExitAria2(CancellationToken token = default)
    {
        await this.PaushAllTask(token);
        Process.GetProcessById(this.ProcessId).Kill();
    }

    public async Task<ResultCode<string>> PaushAllTask(CancellationToken token = default)
    {
        return await RequestAsync<string>(GlobalUsings.PauseAll_Method, token, null);
    }

    public async Task<ResultCode<string>> PauseTask(string gid, CancellationToken token = default)
    {
        return await RequestAsync<string>(GlobalUsings.Pause_Method, token, gid);
    }

    public async Task<ResultCode<string>> ForcePaush(
        string gid,
        CancellationToken token = default
    )
    {
        return await RequestAsync<string>(GlobalUsings.ForcePause, token, gid);
    }

    public async Task<ResultCode<List<string>>> ForcePauseAll(CancellationToken token = default)
    {
        return await RequestAsync<List<string>>(GlobalUsings.PauseAll_Method, token);
    }

    public async Task<ResultCode<string>> ForceRemove(string gid,CancellationToken token = default)
    {
        return await RequestAsync<string>(GlobalUsings.ForceRemove, token, gid);
    }

    public Task<ResultCode<string>> ChangGlobalOption(
        Aria2GlobalOptionEnum type,
        object value,
        CancellationToken token = default
    )
    {
        return this.ChangeGlobalOption(
            new Dictionary<string, string>() { { GlobalOptionConvert.EnumToString(type), value.ToString() } },
            token
        );
    }

    public async Task<bool> ChangGlobalOptions(
        Dictionary<Aria2GlobalOptionEnum, object> keyvalues,
        CancellationToken token = default
    )
    {
        List<bool> flages = new();
        foreach (var value in keyvalues)
        {
            var result = await this.ChangGlobalOption(value.Key, value.Value, token);
            if (result.Result != "OK")
                flages.Add(true);
        }
        if (flages.Contains(true))
        {
            return false;
        }
        return true;
    }



    public async Task<ResultCode<string>> AddTorrentAsync(
        string torrentPath,
        IDictionary<string, object> options,
        int? location = null,
        CancellationToken token = default
    )
    {
        var bytes = await System.IO.File.ReadAllBytesAsync(torrentPath);
        return await this.RequestAsync<string>(
            GlobalUsings.AddTorrent_Method,
            default,
            Convert.ToBase64String(bytes),
            new List<string>(),
            options,
            location
        );
    }

    public async Task<ResultCode<string>> UnpauseAll(CancellationToken token = default)
    {
        return await RequestAsync<string>("aria2.unpauseAll", token);
    }

    public async Task<ResultCode<string>> Unpause(string gid, CancellationToken token = default)
    {
        return await RequestAsync<string>("aria2.unpause", token, gid);
    }

    public async Task<ResultCode<TellOption>> GetTellOption(
        string gid,
        CancellationToken token = default
    )
    {
        return await RequestAsync<TellOption>("aria2.getOption", token, gid);
    }

    public async Task<ResultCode<AllTellStatus>> GetAllTellStatus(CancellationToken token = default)
    {
        return await RequestAsync<AllTellStatus>("aria2.getGlobalStat", token);
    }

    public async Task<ResultCode<object>> MulticallRequestAsync(
        CancellationToken token = default,
        params object[] args
    )
    {
        List<MulticallRequest> request = new List<MulticallRequest>();
        foreach (var item in args)
        {
            if (item is not object[] objvalue)
                throw new ArgumentException("参数头错误！");
            var paramter = objvalue.Skip(1);
            request.Add(
                new MulticallRequest()
                {
                    MethodName = (string)objvalue[0],
                    Parameters = paramter.ToList()
                }
            );
        }
        var result = await RequestAsync<object>("system.multicall", token, request);
        return result;
    }

    public async Task<ResultCode<List<FileDownloadTell>>> GetWaitingTaskAsync(
        int offset = 0,
        int pagesize = 1000,
        CancellationToken token = default
    )
    {
        return await RequestAsync<List<FileDownloadTell>>(
            "aria2.tellWaiting",
            token,
            offset,
            pagesize
        );
    }

    public async Task<ResultCode<List<FileDownloadTell>>> GetStopedTaskAsync(
        int offset = 0,
        int pagesize = 1000,
        CancellationToken token = default
    )
    {
        return await RequestAsync<List<FileDownloadTell>>(
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
        var result = await MulticallRequestAsync(
            token,
            new Object[] { "aria2.tellStopped", 0, 1000 },
            new Object[] { "aria2.tellWaiting", 0, 1000 },
            new Object[] { "aria2.tellActive" }
        );
        return new ResultCode<List<FileDownloadTell>>();
    }

    public async Task<ResultCode<string>> RemoveTask(string gid, CancellationToken token = default)
    {
        return await RequestAsync<string>("aria2.remove", token, gid);
    }

    public async Task<WebSocketState> ConnectAsync(CancellationToken token = default)
    {
        if (_isReconnection)
            return WebSocketState.Connecting;
        _socketThread = new Thread(SocketThread);
        _socketThread.IsBackground = true;
        Socket = new ClientWebSocket();
        Socket.Options.RemoteCertificateValidationCallback = (_, _, _, _) => true;
        this._isReconnection = true;
        await Socket.ConnectAsync(new System.Uri(GlobalUsings.WebSocketBaseUrl), token);
        _socketThread.Start();
        this._aria2ConnectStateHandler?.Invoke(this, Socket.State);
        _isReconnection = false;
        return Socket.State;
    }

    private async void SocketThread(object obj)
    {
        byte[] buffer = new byte[1024 * 1024];
        while (true)
        {
            try
            {
                Thread.Sleep(1000);
                WebSocketReceiveResult result = await Socket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    default
                );
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var jsonObject = JsonSerializer.Deserialize<WebSocketResultCode>(
                           message
                        )!;
                    Aria2WebSocketMessageDelegate?.Invoke(
                        this, jsonObject

                    );
                    OnWebSocketEvent(jsonObject);
                }
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    this._aria2ConnectStateHandler?.Invoke(this, Socket.State);
                    await DisconnectAsync();
                }
            }
            catch (Exception)
            {
                this._aria2ConnectStateHandler?.Invoke(this, Socket.State);
            }
            
        }
    }

    private void OnWebSocketEvent(WebSocketResultCode message)
    {
        switch (message.Method)
        {
            case Aria2Socket_Method.OnDownloadStart:
                this.aria2DownloadStateChangedHandler?.Invoke( Enums.WebSocketEventType.Start, message);
                break;
            case Aria2Socket_Method.OnDownloadPause:
                this.aria2DownloadStateChangedHandler?.Invoke(Enums.WebSocketEventType.Pause, message);
                break;
            case Aria2Socket_Method.OnDownloadStop:
                this.aria2DownloadStateChangedHandler?.Invoke(Enums.WebSocketEventType.Stop, message);
                break;
            case Aria2Socket_Method.OnDownloadComplete:
                this.aria2DownloadStateChangedHandler?.Invoke(Enums.WebSocketEventType.Complete, message);
                break;
            case Aria2Socket_Method.OnDownloadError:
                this.aria2DownloadStateChangedHandler?.Invoke(Enums.WebSocketEventType.Error, message);
                break;
            case Aria2Socket_Method.OnBtDownloadComplete:
                this.aria2DownloadStateChangedHandler?.Invoke(Enums.WebSocketEventType.BtComplete, message);
                break;
        }
    }

    public async Task<Tuple<WebSocketState, WebSocketCloseStatus?>> DisconnectAsync(
        CancellationToken token = default
    )
    {
        await Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Error", token);
        return new(Socket.State, Socket.CloseStatus);
    }

    public async Task<ResultCode<string>> Aria2RemoveDownloadResult(string gid, CancellationToken token = default)
    {
        return await RequestAsync<string>("aria2.removeDownloadResult", token, gid);
    }

    public async Task<ResultCode<string>> Aria2RemoveAllDownloadResult(CancellationToken token= default)
    {
        return await RequestAsync<string>("aria2.purgeDownloadResult", token);
    }

    public async Task<ResultCode<List<DownloadFile>>> GetFiles(string gid, CancellationToken token = default)
    {
        return await RequestAsync<List<DownloadFile>>("aria2.getFiles", token, gid);
    }

    public async Task<ResultCode<string>> ChangeTellOption(string gid, Dictionary<string, object> values, CancellationToken token = default)
    {
        return await RequestAsync<string>("aria2.changeOption", token, gid,values);
    }

    public async Task<WebSocketState> ReconnectionSocket(CancellationToken token = default)
    {
        this.Socket = new ClientWebSocket();
        return await this.ConnectAsync(token);
    }

    public async Task<ResultCode<List<BittorrentPeer>>> GetBittorrentPeers(string gid, CancellationToken token = default)
    {
        return await RequestAsync<List<BittorrentPeer>>("aria2.getPeers",token,gid);
    }

    public async Task<IpData> GetIpAsync(string ip, CancellationToken token = default)
    {
        var uri = $"https://opendata.baidu.com/api.php?query={ip}&co=&resource_id=6006&oe=utf8";
        HttpClient client = new HttpClient();
        var result = await client.GetAsync(uri,token);
        return JsonSerializer.Deserialize<IpData>(await result.Content.ReadAsStringAsync())!;
    }

    public event Aria2ConnectStateChangedDelegate Aria2ConnectStateChanged
    {
        add => _aria2ConnectStateHandler += value;
        remove => _aria2ConnectStateHandler -= value;
    }

    public event Aria2WebSocketMessageDelegate Aria2WebSocketMessage
    {
        add => Aria2WebSocketMessageDelegate += value;
        remove => Aria2WebSocketMessageDelegate -= value;
    }


}
