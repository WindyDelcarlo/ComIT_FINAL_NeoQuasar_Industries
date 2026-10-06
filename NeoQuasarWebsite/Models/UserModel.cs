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
                    // if (Program.nqiDatabase.Find())
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

    // Constructor From Database
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

    public void AddToCart(string itemID, int qty)
    {
        CartLineData? priorData = Program.Database().CartLines.FirstOrDefault(ci => ci.UserID == ID && ci.ItemSKU == itemID);
        if (priorData != null)
        {
            priorData.Quantity += qty;
        }
        else
        {
            var newCI = new CartLineData
            {
                UserID = ID,
                ItemSKU = itemID,
                Quantity = qty,
                LineItem = StockItem.GetDatabase().FirstOrDefault(si => si.NeoSKU == itemID)
            };
            Program.Database().Add(newCI);
        }
        
        Program.Database().SaveChanges();
        /*
        CartItem? cartItem = null;
        foreach (CartItem ci in Cart)
        {
            if (ci.ItemSKU == itemID)
            {
                cartItem = ci;
                break;
            }
        }
        if (cartItem == null)
        {
            Cart.Add(new CartItem(itemID,1));
        }
        else
        {
            cartItem.Quantity += qty;
        } */
    }

    public bool RemoveFromCart(string itemID, int qty)
    {
        CartLineData? priorData = Program.Database().CartLines.FirstOrDefault(ci => ci.UserID == ID && ci.ItemSKU == itemID);
        if (priorData == null)
        {
            return false;
        }
        if (priorData.Quantity > qty)
        {
            priorData.Quantity -= qty;
            Program.Database().SaveChanges();
            return true;
        }
        Program.Database().Remove(priorData);
        Program.Database().SaveChanges();
        return true;
        /*CartItem? cartItem = null;
        foreach (CartItem ci in Cart)
        {
            if (ci.ItemSKU == itemID)
            {
                cartItem = ci;
                break;
            }
        }
        if (cartItem != null)
        {
            cartItem.Quantity -= qty;
            if (cartItem.Quantity <= 0)
            {
                Cart.Remove(cartItem);
            }
        }*/
    }

    public CartLineData? GetCartLine(string neoSKU)
    {
        return LineItem.CartLineDatabase().FirstOrDefault(cl => cl.ItemSKU == neoSKU && cl.UserID == ID);
    }

    public bool AddPaymentMethod(PaymentDetails payment)
    {
        bool valid = PaymentDetails.ValidateCard(payment);
        if (valid)
        {
            int paymentID;
            if (Program.Database().PaymentDetails.ToList().Count() > 0){
                paymentID = Program.Database().PaymentDetails.ToList().Last().PaymentDetailID + 1;
            }
            else
            {
                paymentID = 0;
            }
            Program.Database().Add(new PaymentDetailData
            {
                PaymentDetailID = paymentID,
                UserId = ID,
                CardName = payment.CardName,
                CardNumber = payment.CardNumber,
                CardType = payment.CardType.ToString()
            });
            DatabaseThis().SelectedCard = paymentID;
            Program.Database().SaveChanges();

            return true;
            /*PaymentDetailsList.Add(payment);
            if (PrimaryPaymentMethod == null)
            {
                PrimaryPaymentMethod = payment;
            }*/
        }
        else return false;
    }

    public bool RemovePaymentMethod(int id)
    {
        PaymentDetailData? priorData = Program.Database().PaymentDetails.FirstOrDefault(pd => pd.PaymentDetailID == id);
        if (priorData != null)
        {
            Program.Database().Remove(priorData);
            Program.Database().SaveChanges();
            return true;
        }
        else return false;

        /*
        PaymentDetailsList.Remove(payment);
        if (PrimaryPaymentMethod == payment)
        {
            PrimaryPaymentMethod = null;
        } */
    }

    public bool SelectPaymentMethod(int id)
    {
        PaymentDetailData? details = PaymentDetails.GetDatabase().FirstOrDefault(d => d.PaymentDetailID == id);
        if (details == null)
        {
            return false;
        }
        else
        {
            DatabaseThis().SelectedCard = details.PaymentDetailID;
            Program.Database().SaveChanges();
            return true;
        }
    }

    public bool ChangeName(string newName)
    {
        try
        {
            Name = newName;
            DatabaseThis().Name = newName;
            Program.Database().SaveChanges();
            return true;

        }
        catch
        {
            return false;
        }
    }

    public bool ChangeAddress(string address, string locatorCode)
    {
        try
        {
            Address = address;
            LocatorCode = locatorCode;
            DatabaseThis().Address = address;
            DatabaseThis().LocatorCode = locatorCode;
            Program.Database().SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }


    /*    
    public static List<UserModel> GetDatabase()
    {
        return TempUserDatabase;
    } 
    */

    public static void DatabaseAdd(UserModel user, string password)
    {
        throw new Exception("Defunct method called");
    }

    public static UserData? GetUser(string s)
    {
        int ID = 0;
        bool tryParse = int.TryParse(s, out ID);
        return GetUser(ID);
    }

    public static UserData? GetUser(int i)
    {
        return UserModel.GetDatabase().Find(i);

    }

    public static string EncryptPassword(string password)
    {

        List<int> charList = [.. password];
        List<int> output = new List<int>();
        for (int i = 0; i < charList.Count; i++)
        {
            charList[i] *= 2;
            charList[i] += 11 + i;
            output.Add(charList[i]);
        }
        StringBuilder sb = new StringBuilder();
        foreach (int i in output)
        {
            sb.Append(i);
        }

        return sb.ToString();
    }

    public List<CartLineData> Cart()
    {
        return CartItem.GetDatabase().Where(ci => ci.UserID == ID).ToList();
    } 

    public static DbSet<UserData> GetDatabase()
    {
        return Program.Database().Users;
    }

    public UserData DatabaseThis()
    {
        return GetDatabase().FirstOrDefault(u => u.UserID == ID);
    }



}

public enum UserRole
{
    USER,
    SHIPPER,
    ADMIN,
    NONE
}