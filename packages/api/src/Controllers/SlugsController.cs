using Harmonia.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Harmonia.Api.Controllers;

[ApiController]
[Route("")]
public class SlugsController : ControllerBase
{
    private readonly ILogger<SlugsController> _logger;
    private readonly ShortUrlRepository _shortUrlRepository;
    
    public SlugsController(ILogger<SlugsController> logger, ShortUrlRepository shortUrlRepository)
    {
        _logger = logger;
        _shortUrlRepository = shortUrlRepository;
    }

    [HttpGet("{slug}", Name = "GetShortUrl")]
    public ActionResult Get(string slug)
    {
        _logger.LogDebug("Incoming request for slug \"{slug}\", trying to find destination in database.", slug);
        var shortUrl = _shortUrlRepository.Get(slug);

        if (shortUrl.Destination == null) return NotFound();
        
        _logger.LogDebug("Incoming request for slug \"{slug}\" will be redirected to \"{destination}\"", 
            shortUrl.Slug, shortUrl.Destination);
        
        return Redirect(shortUrl.Destination);
    }

    [HttpGet("[controller]", Name = "GetAllShortUrls")]
    public IActionResult All()
    {
        _logger.LogDebug("Retrieving all ShortUrls");
        var shortUrls = _shortUrlRepository.GetAll().Result;
        
        return Ok(shortUrls);
    }
}
