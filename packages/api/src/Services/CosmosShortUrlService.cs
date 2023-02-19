using System.Net;
using Harmonia.Data;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace Harmonia.Api.Services;

public class CosmosShortUrlService
{
    private readonly ILogger<CosmosShortUrlService> _logger;
    private readonly CosmosClient _cosmosClient;

    public CosmosShortUrlService(ILogger<CosmosShortUrlService> logger, CosmosClient cosmosClient)
    {
        _logger = logger;
        _cosmosClient = cosmosClient;
    }
    
    private Container Container => _cosmosClient.GetDatabase("harmonia").GetContainer("shorturls");

    public async Task<ShortUrl?> CreateAsync(ShortUrl shortUrl)
    {
        shortUrl.Slug = shortUrl.Slug?.ToLower();
        
        var response = await Container.CreateItemAsync(shortUrl, new PartitionKey(shortUrl.Slug?.ToLower()));
        _logger.LogInformation("Transaction cost for this request is {requestCharge}", response.RequestCharge);
        
        if (response.StatusCode != HttpStatusCode.Created)
        {
            throw new Exception(response.ActivityId);
        }

        return shortUrl;
    }

    public async Task<IEnumerable<ShortUrl>> GetAllShortUrlsAsync()
    {
        var queryable = Container.GetItemLinqQueryable<ShortUrl>();
        
        using var feed = queryable
            .ToFeedIterator();
        
        List<ShortUrl> results = new();
        
        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            _logger.LogInformation("Transaction cost for this request is {requestCharge}", response.RequestCharge);
            
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
        _logger.LogInformation("Transaction cost for this request is {requestCharge}", response.RequestCharge);
        
        var shortUrl = response.Resource.FirstOrDefault();

        return shortUrl ?? null;
    }
}
