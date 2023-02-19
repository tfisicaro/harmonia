using Harmonia.Data;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Harmonia.Api.Services;

public class CosmosShortUrlService
{
    private readonly CosmosClient _cosmosClient;

    public CosmosShortUrlService(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }
    
    private Container Container => _cosmosClient.GetDatabase("harmonia").GetContainer("shorturls");

    public async Task<IEnumerable<ShortUrl>> GetAllShortUrlsAsync()
    {
        var queryable = Container.GetItemLinqQueryable<ShortUrl>();
        
        using var feed = queryable
            .ToFeedIterator();
        
        List<ShortUrl> results = new();
        
        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }

    public async Task<ShortUrl?> GetBySlugAsync(string slug)
    {
        var parameterizedQuery = new QueryDefinition(
            query: "SELECT * FROM shorturls s WHERE LOWER(s.slug) = @slug")
            .WithParameter("@slug", slug.ToLower());
        
        using var filteredFeed = Container.GetItemQueryIterator<ShortUrl>(queryDefinition: parameterizedQuery);

        var response = await filteredFeed.ReadNextAsync();
        var shortUrl = response.Resource.FirstOrDefault();

        return shortUrl ?? null;
    }
}
