// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class Datum
{
    [JsonPropertyName("ExtendedLocation")]
    public string ExtendedLocation { get; set; }

    [JsonPropertyName("OriginQuery")]
    public string OriginQuery { get; set; }

    [JsonPropertyName("appinfo")]
    public string Appinfo { get; set; }

    [JsonPropertyName("disp_type")]
    public int DispType { get; set; }

    [JsonPropertyName("fetchkey")]
    public string Fetchkey { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("origip")]
    public string Origip { get; set; }

    [JsonPropertyName("origipquery")]
    public string Origipquery { get; set; }

    [JsonPropertyName("resourceid")]
    public string Resourceid { get; set; }

    [JsonPropertyName("role_id")]
    public int RoleId { get; set; }

    [JsonPropertyName("shareImage")]
    public int ShareImage { get; set; }

    [JsonPropertyName("showLikeShare")]
    public int ShowLikeShare { get; set; }

    [JsonPropertyName("showlamp")]
    public string Showlamp { get; set; }

    [JsonPropertyName("titlecont")]
    public string Titlecont { get; set; }

    [JsonPropertyName("tplt")]
    public string Tplt { get; set; }
}

public class IpData
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("t")]
    public string T { get; set; }

    [JsonPropertyName("set_cache_time")]
    public string SetCacheTime { get; set; }

    [JsonPropertyName("data")]
    public List<Datum> Data { get; set; }
}

