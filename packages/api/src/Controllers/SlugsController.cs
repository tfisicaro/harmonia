using Harmonia.Api.Repositories;
using Harmonia.Data;
using Harmonia.Data.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Harmonia.Api.Controllers;

[ApiController]
[Route("")]
public class SlugsController : ControllerBase
{
    private readonly ILogger<SlugsController> _logger;
    private readonly IConfiguration _configuration;
    private readonly ShortUrlRepository _shortUrlRepository;
    private readonly ShortUrlValidator _shortUrlValidator = new();
    
    public SlugsController(
        ILogger<SlugsController> logger,
        IConfiguration configuration,
        ShortUrlRepository shortUrlRepository)
    {
        _logger = logger;
        _configuration = configuration;
        _shortUrlRepository = shortUrlRepository;
    }

    // TODO: Create object from body
    [HttpPost("[controller]", Name = "CreateShortUrl")]
    public async Task<ActionResult<ShortUrl>> Create(string slug, string destination)
    {
        var shortUrl = new ShortUrl
        {
            Slug = slug,
            Destination = destination,
        };

        var shortUrlValidationResult = await _shortUrlValidator.ValidateAsync(shortUrl);
        if (!shortUrlValidationResult.IsValid)
            return BadRequest(shortUrlValidationResult.Errors);
        
        var result = await _shortUrlRepository.Create(shortUrl);
        if (result is null)
            return BadRequest();
        
        return Ok(shortUrl);
    }

    [HttpGet("[controller]", Name = "GetAllShortUrls")]
    public IActionResult All()
    {
        _logger.LogDebug("Retrieving all ShortUrls");
        var shortUrls = _shortUrlRepository.GetAll().Result;
        
        return Ok(shortUrls);
    }
    
    [HttpGet("{slug}", Name = "GetShortUrl")]
    public async Task<ActionResult> Get(string slug)
    {
        _logger.LogDebug("Incoming request for slug \"{slug}\", trying to find destination in database.", slug);
        var shortUrl = await _shortUrlRepository.Get(slug);

        if (shortUrl?.Destination is null)
        {
            // TODO: Move this logic into a helper method
            // TODO: Count traffic stats for invalid slugs
            var queryParams = $"?{_configuration.GetValue<string>("FallbackSettings:RefQueryParamName")}=cc&{_configuration.GetValue<string>("FallbackSettings:SourceQueryParamName")}={slug}";
            return Redirect(_configuration.GetValue<string>("FallbackSettings:FallbackUrl") + queryParams);
        }

        _logger.LogDebug("Incoming request for slug \"{slug}\" will be redirected to \"{destination}\"", 
            shortUrl.Slug, shortUrl.Destination);
        
        return Redirect(shortUrl.Destination);
    }
}
