using Harmonia.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Harmonia.Api.Controllers;

[ApiController]
[Route("{slug}")]
public class SlugController : ControllerBase
{
    private readonly ILogger<SlugController> _logger;
    private readonly ShortUrlRepository _shortUrlRepository;
    
    public SlugController(ILogger<SlugController> logger, ShortUrlRepository shortUrlRepository)
    {
        _logger = logger;
        _shortUrlRepository = shortUrlRepository;
    }

    [HttpGet(Name = "GetShortUrl")]
    public ActionResult Get(string slug)
    {
        _logger.LogDebug("Incoming request for slug \"{slug}\", trying to find destination in database.", slug);
        var shortUrl = _shortUrlRepository.Get(slug);

        if (shortUrl.Destination == null) return NotFound();
        
        _logger.LogDebug("Incoming request for slug \"{slug}\" will be redirected to \"{destination}\"", 
            shortUrl.Slug, shortUrl.Destination);
        
        return Redirect(shortUrl.Destination);
    }
}
