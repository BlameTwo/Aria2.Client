using Aria2.Net.Services.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;

namespace Aria2.Test;

[TestClass]
public class UnitTest1
{
    public string RequestUrl = "http://127.0.0.1:5050/jsonrpc";

    [TestMethod("测试获取全部下载链接")]
    public async Task TestTallStatus()
    {
        var client = Register.GetService<IAria2cClient>();
        var a = await client.AddUriAsync(
            new List<string>()
            {
                "http://updates-http.cdn-apple.com/2019WinterFCS/fullrestores/041-39257/32129B6C-292C-11E9-9E72-4511412B0A59/iPhone_4.7_12.1.4_16D57_Restore.ipsw",
            },
            new Dictionary<string, object>() { { "dir", "D:\\alistDownload" }, },
            1
        );
        var b = await client.AddUriAsync(
            new List<string>() { "https://sgp.proof.ovh.net/files/1Gb.dat", },
            new Dictionary<string, object>() { { "dir", "D:\\alistDownload" }, },
            1
        );
        var result = await client.GetAllTellActiveAsync();
    }

    [TestMethod("测试停止所有下载")]
    public async Task TestPaushTask()
    {
        var client = Register.GetService<IAria2cClient>();

        var result = await client.PaushAllTask();
    }

    [TestMethod("测试BT下载")]
    public async Task TestTorrentDownload()
    {
        var client = Register.GetService<IAria2cClient>();
        var result = await client.AddTorrentAsync(
            "C:\\Users\\30140\\Downloads\\f.torrent",
            new Dictionary<string, object>() { { "dir", "C:\\Users\\30140\\Downloads" } },
            1
        );
    }

    [TestMethod("测试获取下载设置（单下载任务）")]
    public async Task TestTellOption()
    {
        var client = Register.GetService<IAria2cClient>();
        var a = await client.AddUriAsync(
            new List<string>()
            {
                "http://updates-http.cdn-apple.com/2019WinterFCS/fullrestores/041-39257/32129B6C-292C-11E9-9E72-4511412B0A59/iPhone_4.7_12.1.4_16D57_Restore.ipsw",
            },
            new Dictionary<string, object>() { { "dir", "D:\\alistDownload" }, },
            1
        );
        var result = await client.GetTellOption(a.Result);
    }

    [TestMethod]
    public async Task TestAllTellStatus()
    {
        var client = Register.GetService<IAria2cClient>();
        var a = await client.AddUriAsync(
            new List<string>()
            {
                "http://updates-http.cdn-apple.com/2019WinterFCS/fullrestores/041-39257/32129B6C-292C-11E9-9E72-4511412B0A59/iPhone_4.7_12.1.4_16D57_Restore.ipsw",
            },
            new Dictionary<string, object>() { { "dir", "D:\\alistDownload" }, },
            1
        );
        var result = await client.GetAllTellStatus();
    }


    [TestMethod]
    public async Task TestPauseTellStatus()
    {
        var client = Register.GetService<IAria2cClient>();
        var a = await client.AddUriAsync(
            new List<string>()
            {
                "http://updates-http.cdn-apple.com/2019WinterFCS/fullrestores/041-39257/32129B6C-292C-11E9-9E72-4511412B0A59/iPhone_4.7_12.1.4_16D57_Restore.ipsw",
            },
            new Dictionary<string, object>() { { "dir", "D:\\alistDownload" }, },
            1
        );
        var pause = await client.PauseTask(a.Result);

        //var allvalue = await client.GetAllTaskAsync();
    }


    [TestMethod]
    public async Task TestWebSocket()
    {

        CancellationTokenSource tokensource = new CancellationTokenSource();
        ClientWebSocket socket = new ClientWebSocket();
        socket.Options.RemoteCertificateValidationCallback = (_, _, _, _) => true;
        await socket.ConnectAsync(new Uri("ws://localhost:5050/jsonrpc"),tokensource.Token);
        var b = socket.State;

    }
}
