using Harmonia.Data;

namespace Harmonia.Api.Repositories;

public interface IShortUrlRepository
{
    public ShortUrl Get(string path);
}

public class ShortUrlRepository : IShortUrlRepository
{
    // TODO: Retrieve data from database
    public ShortUrl Get(string path)
    {
        if (path == "test12")
            return new ShortUrl
            {
                Slug = "test12",
                Destination = "https://bing.com",
            };
        
        return new ShortUrl
        {
            Slug = "anything",
            Destination = "https://google.com",
        };
    }
}
