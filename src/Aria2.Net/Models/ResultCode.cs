using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aria2.Net.Models;

public class ResultCode<T>
{
    [JsonPropertyName("id")]
    public string ID { get; set; }

    [JsonPropertyName("jsonrpc")]
    public string Code { get; set; }

    [JsonPropertyName("result")]
    public T Result { get; set; }
}


public class WebSocketResultCode
{
    [JsonPropertyName("jsonrpc")]
    public string JsonRpc { get; set; }

    [JsonPropertyName("method")]
    public string Method { get; set; }

    [JsonPropertyName("params")]
    public List<Param> Params { get; set; }

    [JsonPropertyName("error")]
    public WebSocketErrorCode Code { get; set; }
}

public class Param
{
    [JsonPropertyName("gid")]
    public string Gid { get; set; }
}

public class WebSocketErrorCode
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}