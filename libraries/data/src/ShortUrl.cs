using System.Text.Json.Serialization;

namespace Harmonia.Data;

/// <summary>
/// URL mapping. Searches for the slug and redirects to the destination.
/// </summary>
public sealed class ShortUrl
{
    /// <summary>
    /// The path after the domain name in the request URL.
    /// </summary>
    [JsonPropertyName("slug")]
    public string? Slug { get; set; }
    
    /// <summary>
    /// The destination URL where the application should redirect to.
    /// </summary>
    [JsonPropertyName("destination")]
    public string? Destination { get; set; }
}
