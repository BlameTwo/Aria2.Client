

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Aria2.Client.Models.Anime;

public class Fansub
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("href")]
    public string Href { get; set; }
}

public class Filter
{
    [JsonPropertyName("duplicate")]
    public bool Duplicate { get; set; }

    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
}

public class Publisher
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("href")]
    public string Href { get; set; }
}

public class AnimeResource
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("provider")]
    public string Provider { get; set; }

    [JsonPropertyName("providerId")]
    public string ProviderId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("href")]
    public string Href { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("magnet")]
    public string Magnet { get; set; }

    [JsonPropertyName("size")]
    public string Size { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("fetchedAt")]
    public DateTime FetchedAt { get; set; }

    [JsonPropertyName("fansub")]
    public Fansub Fansub { get; set; }

    [JsonPropertyName("publisher")]
    public Publisher Publisher { get; set; }
}

public class AnimeTorrentModel
{
    [JsonPropertyName("filter")]
    public Filter Filter { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("resources")]
    public List<AnimeResource> Resources { get; set; }

    [JsonPropertyName("complete")]
    public bool Complete { get; set; }
}
