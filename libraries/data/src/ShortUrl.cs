using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Harmonia.Data;

/// <summary>
/// URL mapping. Searches for the slug and redirects to the destination.
/// </summary>
public sealed class ShortUrl
{
    /// <summary>
    /// Required property on Azure Cosmos DB items.
    /// </summary>
    [JsonProperty("id")]
    [JsonPropertyName("id")]
    public string? Id { get; init; } = Guid.NewGuid().ToString().ToLower();
    
    /// <summary>
    /// The path after the domain name in the request URL.
    /// </summary>
    [JsonProperty("slug")]
    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
    
    /// <summary>
    /// The destination URL where the application should redirect to.
    /// </summary>
    [JsonProperty("destination")]
    [JsonPropertyName("destination")]
    public string? Destination { get; init; }
}
