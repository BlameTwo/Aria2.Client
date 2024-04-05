using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aria2.Net.Models.ClientModel;


public class File
{
    [JsonPropertyName("completedLength")]
    public string CompletedLength { get; set; }

    [JsonPropertyName("index")]
    public string Index { get; set; }

    [JsonPropertyName("length")]
    public string Length { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; }

    [JsonPropertyName("selected")]
    public string Selected { get; set; }

    [JsonPropertyName("uris")]
    public List<Uri> Uris { get; set; }
}

public class Bittorrent
{
    [JsonPropertyName("announceList")]
    public List<List<string>> AnnounceList { get; set; }

    [JsonPropertyName("creationDate")]
    public int CreationDate { get; set; }

    [JsonPropertyName("info")]
    public Info Info { get; set; }

    [JsonPropertyName("mode")]
    public string Mode { get; set; }
}

public class Info
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}


public class FileDownloadTell
{
    [JsonPropertyName("bitfield")]
    public string Bitfield { get; set; }

    [JsonPropertyName("bittorrent")]
    public Bittorrent Bittorrent { get; set; }

    [JsonPropertyName("completedLength")]
    public string CompletedLength { get; set; }

    [JsonPropertyName("connections")]
    public string Connections { get; set; }

    [JsonPropertyName("dir")]
    public string Dir { get; set; }

    [JsonPropertyName("downloadSpeed")]
    public string DownloadSpeed { get; set; }

    [JsonPropertyName("files")]
    public List<File> Files { get; set; }

    [JsonPropertyName("gid")]
    public string Gid { get; set; }

    [JsonPropertyName("infoHash")]
    public string InfoHash { get; set; }

    [JsonPropertyName("numPieces")]
    public string NumPieces { get; set; }

    [JsonPropertyName("numSeeders")]
    public string NumSeeders { get; set; }

    [JsonPropertyName("pieceLength")]
    public string PieceLength { get; set; }

    [JsonPropertyName("seeder")]
    public string Seeder { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("totalLength")]
    public string TotalLength { get; set; }

    [JsonPropertyName("uploadLength")]
    public string UploadLength { get; set; }

    [JsonPropertyName("uploadSpeed")]
    public string UploadSpeed { get; set; }
}

public class Uri
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("uri")]
    public string uri { get; set; }
}
