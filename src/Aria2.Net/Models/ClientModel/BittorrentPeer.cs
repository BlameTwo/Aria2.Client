using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aria2.Net.Models.ClientModel;

public class BittorrentPeer
{
    [JsonPropertyName("amChoking")]
    public string AmChoking { get; set; }

    [JsonPropertyName("bitfield")]
    public string Bitfield { get; set; }

    [JsonPropertyName("downloadSpeed")]
    public string DownloadSpeed { get; set; }

    [JsonPropertyName("ip")]
    public string Ip { get; set; }

    [JsonPropertyName("peerChoking")]
    public string PeerChoking { get; set; }

    [JsonPropertyName("peerId")]
    public string PeerId { get; set; }

    [JsonPropertyName("port")]
    public string Port { get; set; }

    [JsonPropertyName("seeder")]
    public string Seeder { get; set; }

    [JsonPropertyName("uploadSpeed")]
    public string UploadSpeed { get; set; }
}

