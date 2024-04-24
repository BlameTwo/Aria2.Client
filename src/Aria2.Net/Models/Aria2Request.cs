using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aria2.Net.Models;

/// <summary>
/// 单方法请求
/// </summary>
public class Aria2Request
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("jsonrpc")]
    public string Jsonrpc { get; set; } = null!;

    [JsonPropertyName("method")]
    public string Method { get; set; } = null!;

    [JsonPropertyName("params")]
    public IList<object?>? Parameters { get; set; }
}


/// <summary>
/// 多方法请求组合
/// </summary>
public class MulticallRequest
{
    [JsonPropertyName("methodName")]
    public string MethodName { get; set; } = null!;

    [JsonPropertyName("params")]
    public IList<object> Parameters { get; set; } = new List<object>();
}
