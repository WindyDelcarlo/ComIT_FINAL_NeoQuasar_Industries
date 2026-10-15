using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class RegisterModel : IValidatableObject
{

    [Required(ErrorMessage = "Name is required.")]
    [RegularExpression(@"^[a-zA-Z0-9 \-]*$",ErrorMessage = "Only letters, numbers, hyphens, and spaces are permitted in names.")]
    [Display(Name = "Name")]
    public string Name {get;set;} = string.Empty;
    
    [Required(ErrorMessage="Username is required.")]
    [Length(4,16,ErrorMessage = "Username must be between 4 and 16 characters.")]
    [RegularExpression(@"^[a-zA-Z0-9_]*$",ErrorMessage = "Username can only contain letters, numbers, and underscores.")]
    [Display(Name="Username")]
    public string Username {get;set;} = string.Empty;

    [Required(ErrorMessage="Password is required.")]
    [Display(Name="Password")]
    public string Password {get;set;} = string.Empty;
    
    [Required(ErrorMessage="Password is required.")]
    [Display(Name="Confirm Password")]
    public string ConfirmPassword {get;set;} = string.Empty;

    [RegularExpression(@"^[a-z0-9.]{4}\/[a-z0-9]*\.[a-z0-9]*$",ErrorMessage = "sN addresses are either formatted as dim0/carrier.user or .dim/carrier.user")]
    [Display(Name="sN Address")]
    public string? StellarNet {get;set;} = string.Empty;

    [RegularExpression(@"^[a-zA-Z0-9_=<>.%$]*$",ErrorMessage = "SlingName can only contain the following special characters: _ = < > . % $")]
    [Display(Name="Sling Name")]
    public string? SlingName {get;set;} = string.Empty;
    
    [Required(ErrorMessage = "Address is required.")]
    [RegularExpression(@"^[a-zA-Z0-9_=<>.,&%$ :\/\-]*$",ErrorMessage = "Address uses unrecognized characters.")]
    [Display(Name = "Address")]
    public string DimAddress {get;set;} = string.Empty;

    [Required(ErrorMessage = "LOC Code is required.")]
    [RegularExpression(@"^[A-Z0-9\-]*$",ErrorMessage = "Locator Codes use only capital letters, numbers, and hyphens.")]
    [Display(Name = "LOC Code")]
    public string LocatorCode {get;set;} = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.Equals(Password, ConfirmPassword, StringComparison.Ordinal))
        {
            yield return new ValidationResult("Passwords do not match.", new[] { nameof(Password), nameof(ConfirmPassword) });
        }

        if (string.IsNullOrWhiteSpace(StellarNet) && string.IsNullOrWhiteSpace(SlingName))
        {
            yield return new ValidationResult("Both contact fields cannot be empty!", new []{ nameof(StellarNet), nameof(SlingName)});
        }
    }

}