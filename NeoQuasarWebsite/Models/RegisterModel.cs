using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class RegisterModel
{
    public int Id {get; private set;} = NewId();

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

    [RegularExpression(@"^[a-z0-9.]{4}\/[a-z0-9]*\.[a-z0-9]*$",ErrorMessage = "not recognized as a valid sN address.")]
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

    private static int NewId()
    {
        return UserModel.GetDatabase().Count;
    }

/*    public string ErrorMessage {get;private set;} = string.Empty;

    public bool Validate()
    {
        // Name
        if (Name == null)
        {
            ErrorMessage += "Name can't be blank.";
        }    
        else if (!Regex.IsMatch(Name,alphanumericPattern)){   
                ErrorMessage += "Only letters, numbers, hyphens, and spaces are permitted in names.";
                ErrorMessage += Environment.NewLine;
        }

        // Username
        if (string.IsNullOrEmpty(Username))
        {
            ErrorMessage += "Username can't be blank.";
        }
        else {
            if (!Regex.IsMatch(Username, usernamePattern))
            {
                ErrorMessage += "Username can only contain letters, numbers, and underscores.";
            }
            if (!(Username.Length >= 4 && Username.Length <= 16))
            {
                ErrorMessage += "Username must be between 4 and 16 characters.";
            }
            else
            {
                bool taken = false;
                foreach (UserModel um in UserModel.GetDatabase())
                {
                    if (um.Username == Username)
                    {
                        taken = true;
                    }
                }
                if (taken)
                {
                    ErrorMessage += "Username taken!";
                }
            }
        }

        // Password Degeneracy
        if (Password != ConfirmPassword)
        {
            ErrorMessage += "Password and Confirm Password fields must match!";
            ErrorMessage += Environment.NewLine;
        }
        

        // Sling
        if (!string.IsNullOrEmpty(SlingName)){
        string slingIntermediate = SlingName;
            if (slingIntermediate.StartsWith('@'))
            {
                slingIntermediate = SlingName.Replace("@","");
            }
            if (!Regex.IsMatch(slingIntermediate,slingPattern))
            {
                ErrorMessage += "SlingName can only contain the following special characters: _ = < > . % $";
                ErrorMessage += Environment.NewLine;
            }
        }


        // StellarNet
        if (!string.IsNullOrEmpty(StellarNet)){
            string snInt = StellarNet;
            string snIntermediate = snInt.Replace("sN:","");
            if (!Regex.IsMatch(snIntermediate, sNPattern))
            {
                ErrorMessage += snIntermediate + " not recognized as a valid sN address.";
                ErrorMessage += Environment.NewLine;
            }
        }

        // Contact Address
        if (string.IsNullOrEmpty(StellarNet) && string.IsNullOrEmpty(SlingName))
        {
            ErrorMessage += "StellarNet Address and Sling Name can't both be empty!";
            ErrorMessage += Environment.NewLine;
        }

        // Dim Address
        if (string.IsNullOrEmpty(DimAddress)){
            ErrorMessage += "Address cannot be empty!";
            ErrorMessage += Environment.NewLine;
        }
        else if (!Regex.IsMatch(DimAddress, addressPattern))
        {
            ErrorMessage += $"Address uses unrecognized characters.";
            ErrorMessage += Environment.NewLine;
        }

        // Loc Code
        if (string.IsNullOrEmpty(LocatorCode))
        {
            ErrorMessage += "Locator Code cannot be empty!";
            ErrorMessage += Environment.NewLine;
        }
        else if (!Regex.IsMatch(LocatorCode, locatorPattern))
        {
                ErrorMessage += $"Locator Codes use only capital letters, numbers, and hyphens.";
                ErrorMessage += Environment.NewLine;
        }

        return ErrorMessage == string.Empty;
    }
    */
}
