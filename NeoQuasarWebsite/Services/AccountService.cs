using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Services;

public interface IAccountService
{
    public UserDTO GetUser(UserDTO userDto);
    public int GetCurrentUserID(ClaimsPrincipal User);
    public UserData GetCurrentUser(ClaimsPrincipal User);
    public bool Register(UserDTO user);
    public bool CheckUniqueUsername(string username);
    public void AddToCart(string neoSKU, int userID, int qty);
    public bool RemoveFromCart(string neoSKU, int userID, int qty);   
    public void EditAccountDetails(UserDTO dto, UserData user);
    public void AddPaymentDetails(PaymentDTO payment, int userID);
    public bool ValidateCard (PaymentDTO payment, out string errorMessage);
    public void RemoveCard (int paymentID, int userID);
    public void SelectCard (int paymentID, int userID);
    public bool ChangeRole(int userID, string role);
    public bool VerifyUniqueUsernameAndContact(UserDTO user, out int error);
}

public class AccountService : IAccountService
{
    private readonly NeoQuasarDBContext database;
    private readonly IPasswordHasher<UserData> _passwordHasher;
    public AccountService(NeoQuasarDBContext dbContext, IPasswordHasher<UserData> passwordHasher)
    {
        database = dbContext;
        _passwordHasher = passwordHasher;
    }

    public UserDTO GetUser(UserDTO userDto)
    {
        UserData? user = database.Users.FirstOrDefault(u => u.Username == userDto.Username);
        if (user == null)
        {
            return null;
        }

        if (_passwordHasher.VerifyHashedPassword(user, user.PassHash, userDto.Password) == PasswordVerificationResult.Failed)
        {
            return null;
        }
        return new UserDTO
        {
            UserID = user.UserID,
            Username = user.Username,
            Role = user.Role
        };
    }

    public int GetCurrentUserID(ClaimsPrincipal User)
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        
    }

    public UserData GetCurrentUser(ClaimsPrincipal User)
    {
        int id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        return database.Users.FirstOrDefault(u => u.UserID == id);
    }

    public bool Register(UserDTO user)
    {
        string username = user.Username;
        string name = user.Name;
        string slingName = user.SlingName;
        string stellarNet = user.StellarNet;
        string address = user.Address;
        string locatorCode = user.LocatorCode;
        string password = user.Password;

        if (database.Users.Any(u => u.Username == username))
        {
            return false;
        }

        UserData newUser = new UserData
        {
            Username = username,
            Name = name,
            SlingName = slingName,
            StellarNet = stellarNet,
            Address = address,
            LocatorCode = locatorCode,
            Role = "USER",
            SelectedCard = 0
        };
        newUser.PassHash = _passwordHasher.HashPassword(newUser, user.Password);
        database.Users.Add(newUser);
        database.SaveChanges();
        return true;
    }

    public bool CheckUniqueUsername(string username)
    {
        return !database.Users.Any(u => u.Username == username);
    }

        public void AddToCart(string neoSKU, int userID, int qty)
    {
        CartLineData? priorData = database.CartLines.FirstOrDefault(cld => cld.ItemSKU == neoSKU && cld.UserID == userID);
        if (priorData != null)
        {
            priorData.Quantity += qty;
        }
        else
        {
            var newCI = new CartLineData
            {
                UserID = userID,
                ItemSKU = neoSKU,
                Quantity = qty,
                LineItem = database.StockItems.FirstOrDefault(si => si.NeoSKU == neoSKU)
            };
            database.Add(newCI);
            database.SaveChanges();
        }
        
        database.SaveChanges();
    }

    public bool RemoveFromCart(string neoSKU, int userID, int qty)
    {
        CartLineData? priorData = database.CartLines.FirstOrDefault(cld => cld.ItemSKU == neoSKU && cld.UserID == userID);
        if (priorData == null)
        {
            return false;
        }
        if (priorData.Quantity > qty)
        {
            priorData.Quantity -= qty;
            database.SaveChanges();
            return true;
        }
        database.Remove(priorData);
        database.SaveChanges();
        return true;
    }

    public void EditAccountDetails(UserDTO dto, UserData user)
    {
        user.Name = dto.Name;
        user.SlingName = dto.SlingName;
        user.StellarNet = dto.StellarNet;
        database.SaveChanges();
    }

    public void AddPaymentDetails(PaymentDTO payment, int userID)
    {
        database.PaymentDetails.Add(new PaymentDetailData {
           UserID = userID,
           CardType = payment.CardType,
           CardNumber = payment.CardNumber,
           CardName = payment.CardName 
        });
        database.SaveChanges();
        UserData user = database.Users.FirstOrDefault(u => u.UserID == userID);
        int paymentID = database.PaymentDetails.FirstOrDefault(pd => pd.CardName == payment.CardName && pd.UserID == userID).PaymentDetailID;
        user.SelectedCard = paymentID;
        database.SaveChanges();
    }

    public bool ValidateCard (PaymentDTO payment, out string errorMessage)
    {
        string pattern = payment.CardType switch
        {
            // two lowercase letters + 4 numbers + hyphen + any number of other numbers
            // example -- as1441-258
            
            "STELLARNET_DEBIT" => @"^[a-z]{2}[0-9]{4}-[0-9]{2,}$",

            // 4-alphanumeric + colon + 10-Digit Number, possibly split up by whitespace
            // example -- ast0:123 2449 797
            "CREDIT" => @"^[a-zA-Z0-9]{4}:(?:\s?[0-9]){10}$",

            // Any number of all-caps + hyphen + two all-caps + colon + numbers;
            // example -- RASTERA-ST:1223
            "REQUISITIONS_ACCOUNT" => @"^[A-Z]{2,}-[A-Z]{2}:[0-9]+$",
            _ => "[^.]"
        };
        errorMessage = payment.CardType switch
        {
            // two lowercase letters + 4 numbers + hyphen + any number of other numbers
            // example -- as1441-258
            
            "STELLARNET_DEBIT" => @"Card Number Mismatch! StellarNet Debit format is as1234-567 (bankCode - cardNumber of any length)",

            // 4-alphanumeric + colon + 10-Digit Number, possibly split up by whitespace
            // example -- ast0:123 2449 797
            "CREDIT" => @"Card Number Mismatch! Credit Card format is ast0:123 4567 890. (creditCode : 10 Digits)",

            // Any number of all-caps + hyphen + two all-caps + colon + numbers;
            // example -- RASTERA-ST:1223
            "REQUISITIONS_ACCOUNT" => @"Card Number Mismatch! Requisitions Account format is MARIREY-NQ:1234 (Internal NQI code : 4 Digits)",
            _ => "An unknown error has occured. Please check the card number and try again."
        };
        return Regex.IsMatch(payment.CardNumber,pattern);
    }

    public void RemoveCard (int paymentID, int userID)
    {
        UserData user = database.Users.FirstOrDefault(u => u.UserID == userID);
        if (paymentID == user.SelectedCard)
        {
            user.SelectedCard = 0;
            database.SaveChanges();
        }
        PaymentDetailData pd = database.PaymentDetails.FirstOrDefault(pd => pd.PaymentDetailID == paymentID);
        database.PaymentDetails.Remove(pd);
    }

    public void SelectCard (int paymentID, int userID)
    {
        UserData user = database.Users.FirstOrDefault(u => u.UserID == userID);
        user.SelectedCard = paymentID;
        database.SaveChanges();
    }

    public bool ChangeRole(int userID, string role)
    {
        UserData user = database.Users.FirstOrDefault(u => u.UserID == userID);
        if (user != null && user.Role != "ADMIN")
        {
            user.Role = role;
            database.SaveChanges();
            return true;
        }
        else return false;
    }

    public bool VerifyUniqueUsernameAndContact(UserDTO user, out int error)
    {
        if (database.Users.Any(u => u.Username == user.Username))
        {
            error = 0;
            return false;
        }
        if (!string.IsNullOrWhiteSpace(user.SlingName) && database.Users.Any(u => u.SlingName == user.SlingName))
        {
            error = 1;
            return false;
        }
        if (!string.IsNullOrWhiteSpace(user.StellarNet) && database.Users.Any(u => u.StellarNet == user.StellarNet))
        {
            error = 2;
            return false;
        }
        error = -1;
        return true;
    }
}