﻿using System.Text.Json.Serialization;

namespace Aria2.Net.Models.ClientModel;

public class AllTellStatus
{
    [JsonPropertyName("downloadSpeed")]
    public string DownloadSpeed { get; set; }

    [JsonPropertyName("numActive")]
    public string NumActive { get; set; }

    [JsonPropertyName("numStopped")]
    public string NumStopped { get; set; }

    [JsonPropertyName("numStoppedTotal")]
    public string NumStoppedTotal { get; set; }

    [JsonPropertyName("numWaiting")]
    public string NumWaiting { get; set; }

    [JsonPropertyName("uploadSpeed")]
    public string UploadSpeed { get; set; }
}


