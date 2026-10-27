using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class PasswordViewModel : IValidatableObject
{
    [Required]
    public string Password {get;set;}
    [Required]
    public string NewPassword {get;set;}
    [Required]
    public string ConfirmNewPassword {get;set;}
    
    public int UserID {get;set;} = 0;
    public UserDTO User {get;set;}
    public List<OrderDTO> orderData;
    public List<PaymentDTO> paymentData;
    public int OrderNumber {get;set;} = 0;

    public string ErrorMessage {get;private set;}= string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (NewPassword == ConfirmNewPassword)
        {
            yield return new ValidationResult("Passwords don't match!", new []{nameof(NewPassword),nameof(ConfirmNewPassword)});
        }

        if (Password == NewPassword)
        {
            yield return new ValidationResult("New password can't be the same as the old password!", new []{nameof(Password),nameof(NewPassword)});
        }
    }
}