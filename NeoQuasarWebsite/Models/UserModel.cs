using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.Models;

public class UserModel()
{
    private string firstName;
    private string lastName;
    private static readonly string alphanumericPattern = @"^[a-zA-Z0-9 \-]*$";
    private string userName;
    private static readonly string usernamePattern = @"^[a-zA-Z0-9_]*$";
    private string slingName;
    private static readonly string slingPattern = @"^[a-zA-Z0-9_=<>.%$]*$";
    private string sN;
    private static readonly string sNPattern = @"^[a-z0-9.]{4}\/[a-z0-9]*\.[a-z0-9]*$";
    private string address;
    private static readonly string addressPattern = @"^[a-zA-Z0-9_=<>.,&%$ :\/\-]*$";
    private string locatorCode;
    private static readonly string locatorPattern = @"^[A-Z0-9\-]*$";

    public int ID
    {
        get;
    }
    public string Name
    {
        get
        {
            return firstName + " " + lastName;
        }
        set
        {
            if (Regex.IsMatch(value, alphanumericPattern))
            {
                List<string> names = [.. value.Split(" ")];
                firstName = names[0];
                names.Remove(names[0]);
                StringBuilder sb = new StringBuilder();
                foreach (string n in names)
                {
                    sb.Append(n);
                }
                lastName = sb.ToString();
            }
            else throw new ArgumentException("Only letters, numbers, hyphens, and spaces are permitted in names.");
        }
    }
    public string Username
    {
        get
        {
            return userName;
        }
        set
        {
            if (Regex.IsMatch(value, usernamePattern))
            {
                if (value.Length >= 4 && value.Length <= 16)
                {
                    userName = value;
                }
                else throw new ArgumentException($"Username cannot be {value}: must be between 4 and 16 characters.");
            }
            else throw new ArgumentException("Username can only contain letters, numbers, and underscores.");
        }
    }
    public string SlingName
    {
        get
        {
            return slingName;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                string intermediate = value;
                if (intermediate.StartsWith('@'))
                {
                    intermediate = value.Replace("@", "");
                }
                if (Regex.IsMatch(intermediate, slingPattern))
                {
                    slingName = intermediate;
                }
                else throw new ArgumentException("SlingName can only contain the following special characters: _ = < > . % $");
            }
            else
            {
                slingName = "";
            }
        }
    }
    public string StellarNet
    {
        get
        {
            return sN;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                string intermediate = value;
                string newIntermediate = intermediate.Replace("sN:", "");
                if (Regex.IsMatch(newIntermediate, sNPattern))
                {
                    sN = newIntermediate;
                }
                else throw new ArgumentException(value + " not recognized as a valid sN address.");
            }
            else
            {
                sN = "";
            }
        }
    }
    public string Address
    {
        get
        {
            return address;
        }
        set
        {
            if (Regex.IsMatch(value, addressPattern))
            {
                address = value;
            }
            else throw new ArgumentException($"Address {value} uses unrecognized characters.");
        }
    }
    public string LocatorCode
    {
        get
        {
            return locatorCode;
        }
        set
        {
            if (Regex.IsMatch(value, locatorPattern))
            {
                locatorCode = value;
            }
            else throw new ArgumentException($"{value}: Locator Codes use only capital letters, numbers, and hyphens.");
        }
    }
    public UserRole UserRole { get; set; }


    public UserModel(int id, string name, string username) : this(id, name, username, "", "", "", ".nqi/default.neo", UserRole.NONE)
    {
    }

    // Default Constructor
    public UserModel(int id, string name, string username, string dimAddress, string locatorCode, string sling, string sN) : this(id, name, username, dimAddress, locatorCode, sling, sN, UserRole.USER)
    {
    }

    // Constructor From DB
    public UserModel(UserData user) : this(user.UserID, user.Name, user.Username, user.Address, user.LocatorCode, user.SlingName, user.StellarNet, Enum.Parse<UserRole>(user.Role, true))
    {
    }

    // Admin constructor for making new SR and Admin accounts
    public UserModel(int id, string name, string username, string dimAddress, string locatorCode, string? sling, string? sN, UserRole role) : this()
    {
        ID = id;
        Name = name;
        Username = username;
        Address = dimAddress;
        LocatorCode = locatorCode;
        SlingName = sling != null ? sling : string.Empty;
        StellarNet = sN != null ? sN : string.Empty;
        UserRole = role;
    }

    public string ContactAddress()
    {
        if (StellarNet != null && StellarNet != "")
        {
            return StellarNet;
        }
        else return SlingName;
    }

}

public enum UserRole
{
    USER,
    SHIPPER,
    ADMIN,
    NONE
}