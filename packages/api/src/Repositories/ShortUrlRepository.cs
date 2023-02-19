using Harmonia.Api.Services;
using Harmonia.Data;

namespace Harmonia.Api.Repositories;

public class ShortUrlRepository
{
    private readonly CosmosShortUrlService _cosmosShortUrlService;

    public ShortUrlRepository(CosmosShortUrlService cosmosShortUrlService)
    {
        _cosmosShortUrlService = cosmosShortUrlService;
    }

    public async Task<IEnumerable<ShortUrl>> GetAll()
    {
        return await _cosmosShortUrlService.GetAllShortUrlsAsync();
    }
    
    public async Task<ShortUrl?> Get(string path)
    {
        return await _cosmosShortUrlService.GetBySlugAsync(path);
    }
}
