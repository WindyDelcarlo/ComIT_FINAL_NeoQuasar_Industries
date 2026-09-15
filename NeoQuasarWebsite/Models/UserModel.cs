using System.Text;
using System.Text.RegularExpressions;
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
            if (Regex.IsMatch(value,alphanumericPattern)){   
                List<string> names = [..value.Split(" ")];
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
            if (!string.IsNullOrEmpty(value)){
            string intermediate = value;
            if (intermediate.StartsWith('@'))
            {
                intermediate = value.Replace("@","");
            }
            if (Regex.IsMatch(intermediate,slingPattern))
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
            if (!string.IsNullOrEmpty(value)){
            string intermediate = value;
            string newIntermediate = intermediate.Replace("sN:","");
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
    public string LocatorCode {
        get
        {
            return locatorCode;
        }
        set
        {
            if (Regex.IsMatch(value, locatorPattern))
            {
                locatorCode = locatorPattern;
            }
            else throw new ArgumentException($"{value}: Locator Codes use only capital letters, numbers, and hyphens.");
        }
        }
    public List<PaymentDetails> PaymentDetailsList = new List<PaymentDetails>();
    public PaymentDetails? PrimaryPaymentMethod;
    public List<CartItem> Cart = new List<CartItem>();

    public UserRole UserRole {get;set;}

    private static List<UserModel> TempUserDatabase = new List<UserModel>{ new UserModel(0,"Default","none") };
    private static Dictionary<string,string> PasswordDatabase = new Dictionary<string,string>();


    public UserModel(int id, string name, string username) : this(id,name, username, "", "", "",".nqi/default.neo", new List<PaymentDetails>(),UserRole.NONE)
    {   
    }

    // Default Constructor
    public UserModel(int id, string name, string username, string dimAddress, string locatorCode, string sling, string sN, List<PaymentDetails> paymentDetails) :this(id, name,username,dimAddress,locatorCode,sling,sN,paymentDetails,UserRole.USER)
    {
    }
    
    // Admin constructor for making new SR and Admin accounts
    public UserModel(int id, string name, string username, string dimAddress, string locatorCode, string sling, string sN, List<PaymentDetails> paymentDetails,UserRole role) : this()
    {
        ID = id;
        Name = name;
        Username = username;
        Address = dimAddress;
        LocatorCode = locatorCode;
        SlingName = sling;
        StellarNet = sN;
        PaymentDetailsList.AddRange(paymentDetails);
        UserRole = role;
    }

    public string ContactAddress()
    {
        if (StellarNet != null && StellarNet != "")
        {
            return "sN:" + StellarNet;
        }
        else return "@" + SlingName;
    }

    public void AddToCart(StockItem item, int qty)
    {
        CartItem? cartItem = null;
        foreach (CartItem ci in Cart)
        {
            if (ci.Item.Equals(item))
            {
                cartItem = ci;
                break;
            }
        }
        if (cartItem == null)
        {
            Cart.Add(new CartItem(item,1));
        }
        else
        {
            cartItem.Quantity += qty;
        }
    }

    public void RemoveFromCart(StockItem item, int qty)
    {
        CartItem? cartItem = null;
        foreach (CartItem ci in Cart)
        {
            if (ci.Item.Equals(item))
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
        }
    }

    public void AddPaymentMethod(PaymentDetails payment)
    {
        bool valid = PaymentDetails.ValidateCard(payment);
        if (valid){
            PaymentDetailsList.Add(payment);
            if (PrimaryPaymentMethod == null)
            {
                PrimaryPaymentMethod = payment;
            }
        }
        else throw new Exception("Card Number doesn't match correct structure.");
    }

    public void RemovePaymentMethod(PaymentDetails payment)
    {
        PaymentDetailsList.Remove(payment);
        if (PrimaryPaymentMethod == payment)
        {
            PrimaryPaymentMethod = null;
        }
    }

    public PaymentDetails SelectPaymentMethod(int method)
    {
        if (method < PaymentDetailsList.Count)
        {
            PrimaryPaymentMethod = PaymentDetailsList[method];
            return PaymentDetailsList[method];
        }
        else throw new Exception("Payment method not found.");
    }

            public void ChangeName(string newName)
        {
            try {
                Name = newName;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Error changing name: " + e);
            }
        }

        public void ChangeAddress(string address, string locatorCode)
        {
            string addressStored = Address;
            string locatorStored = LocatorCode;
            try {
                Address = address;
                LocatorCode = locatorCode;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Error changing address: " + e);
                Address = addressStored;
                LocatorCode = locatorStored;
            }
        }

    public static List<UserModel> GetDatabase()
    {
        return TempUserDatabase;
    }

    public static void DatabaseAdd(UserModel user, string password)
    {
        TempUserDatabase.Add(user);
        PasswordDatabase.Add(user.userName,password);
    }

    public static UserModel GetUser(string s)
    {
        int ID = 0;
        bool tryParse = int.TryParse(s, out ID);
        return GetUser(ID);
    }

    public static UserModel GetUser(int i)
    {
        return UserModel.GetDatabase()[i];
    }

    public static bool CheckPassword(string username, string password)
    {
        return password == PasswordDatabase.GetValueOrDefault(username);
    }   

}

public enum UserRole
    {
        USER,
        SHIPPER,
        ADMIN,
        NONE
    }