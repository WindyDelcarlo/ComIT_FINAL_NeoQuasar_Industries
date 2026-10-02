using System.ComponentModel.DataAnnotations;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.ValidationProperties;

public class UniqueUsernameAttribute : ValidationAttribute
{

    public string GetErrorMessage() => $"Username is already taken!";

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        UserData? user = UserModel.GetDatabase().FirstOrDefault(u => u.Username == (string) value);
        if (user == null)
        {
            return ValidationResult.Success;
        }
        else return new ValidationResult(GetErrorMessage());
    }
}