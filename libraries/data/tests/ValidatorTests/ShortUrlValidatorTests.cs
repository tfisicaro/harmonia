using System.Diagnostics.CodeAnalysis;
using Harmonia.Data.Validators;

namespace Harmonia.Data.Tests.ValidatorTests;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
public class ShortUrlValidatorTests
{
    private readonly ShortUrlValidator _validator = new();
    
    [Theory]
    [InlineData("bing", "https://bing.com", true)]
    [InlineData("slug", "https://bing.com", false)]
    [InlineData("slugs", "https://somesite.com", false)]
    [InlineData("slugglus", "https://somesite.com", false)]
    [InlineData("Sluggernaut", "https://mysite.org", false)]
    [InlineData("LookAtMySlug", "https://theezslugs.net", false)]
    [InlineData("ReallyLongNamesShouldWorkJustFine", "https://binarypiano.com", true)]
    [InlineData("TheOnlyConstraintIsThatYouCannotUseTheWordSlugAnywhere", "https://binarypiano.com", false)]
    [InlineData("dash-separated-words-work-fine", "https://thatsthefinger.com", true)]
    [InlineData("underscores_for_maximum_readability", "http://www.staggeringbeauty.com", true)]
    [InlineData("1337", "https://en.wikipedia.org/wiki/Leet", true)]
    [InlineData("ForPeople_that-cant-make-up_th3ir_MIND", "https://smashthewalls.com", true)]
    [InlineData("Über", "https://uber.com", false)]
    public void ShortUrlValidator_ShouldReturnValidValidationResult(string slug, string destination, bool expectedResult)
    {
        // Arrange
        var shortUrl = new ShortUrl
        {
            Slug = slug,
            Destination = destination,
        };
        
        // Act
        var results = _validator.Validate(shortUrl);

        // Assert
        results.IsValid.Should().Be(expectedResult);
    }
}
