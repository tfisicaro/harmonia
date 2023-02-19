using System.Text.RegularExpressions;
using FluentValidation;

namespace Harmonia.Data.Validators;

public class ShortUrlValidator : AbstractValidator<ShortUrl>
{
    private static readonly Regex PathValidRegex = new(
        "^[a-zA-Z0-9_-]*$",
        RegexOptions.None,
        TimeSpan.FromMilliseconds(1)
    );
    
    private static readonly Regex PathContainsSlugRegex = new(
        @"\b(\w*slug\w*)\b",
        RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(1)
    );
    
    public ShortUrlValidator()
    {
        // Slug must not be empty
        RuleFor(su => su.Slug)
            .NotEmpty()
            .WithMessage("Slug must not be empty.");
        
        // Slug must not contain special characters
        RuleFor(su => su.Slug)
            .Must(s => s != null && PathValidRegex.IsMatch(s))
            .WithMessage("Slug can only contain alphanumeric characters, underscores, and dashes.");
        
        // Slug can not contain "slug" to prevent issues with controller routes
        RuleFor(su => su.Slug)
            .Must(s => s != null && !PathContainsSlugRegex.IsMatch(s))
            .WithMessage("Forbidden to use word \"slug\" inside the slug.");
        
        // Destination must not be empty
        RuleFor(su => su.Destination)
            .NotEmpty()
            .WithMessage("Destination must not be empty.");
        
        // Destination must be a well formed URL
        RuleFor(su => su.Destination)
            .Must(s => s != null && Uri.IsWellFormedUriString(s, UriKind.Absolute))
            .WithMessage("Destination must be a valid absolute URL.");
    }
}
