using System.Text.RegularExpressions;

namespace NeoQuasarWebsite.Models;

public class UserUpdateViewModel
{
    public string Name {get;set;}
    private static readonly string alphanumericPattern = @"^[a-zA-Z0-9 \-]*$";
    public string StellarNet {get;set;}
    private static readonly string sNPattern = @"^[a-z0-9.]{4}\/[a-z0-9]*\.[a-z0-9]*$";
    public string SlingName {get;set;}
    private static readonly string slingPattern = @"^[a-zA-Z0-9_=<>.%$]*$";
    public string Address {get;set;}
    
    private static readonly string addressPattern = @"^[a-zA-Z0-9_=<>.,&%$ :\/\-]*$";
    
    public string LocatorCode {get;set;} = string.Empty;
    public static readonly string locPattern = @"^[A-Z0-9\-]*$";

    public string ErrorMessage {get;private set;}= string.Empty;

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
            }
        }

        // Contact Address
        if (string.IsNullOrEmpty(StellarNet) && string.IsNullOrEmpty(SlingName))
        {
            ErrorMessage += "StellarNet Address and Sling Name can't both be empty!";
        }

        if (!string.IsNullOrEmpty(Address)){
            if (!Regex.IsMatch(Address, addressPattern))
            {
                ErrorMessage += Address + " not recognized as a valid address.";
            }
        }

        if (!string.IsNullOrEmpty(LocatorCode)){
            if (!Regex.IsMatch(LocatorCode, locPattern))
            {
                ErrorMessage += LocatorCode + " not recognized as a valid LOC Code.";
            }
        }

        return ErrorMessage == string.Empty;
    }
}