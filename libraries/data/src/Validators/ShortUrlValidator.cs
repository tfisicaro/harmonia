using System.Text.RegularExpressions;
using FluentValidation;

namespace Harmonia.Data.Validators;

public class ShortUrlValidator : AbstractValidator<ShortUrl>
{
    private static readonly Regex PathRegex = new(
        "^[a-zA-Z0-9_-]*$",
        RegexOptions.None,
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
            .Must(s => s != null && PathRegex.IsMatch(s))
            .WithMessage("Slug can only contain alphanumeric characters, underscores, and dashes.");
        
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
