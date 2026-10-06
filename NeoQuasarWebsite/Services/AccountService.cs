using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Services;

public interface IAccountService
{

    public UserDTO GetUser(UserDTO userDto);
}

public class AccountService : IAccountService
{
    private readonly NeoQuasarDBContext _dbContext;
    private readonly IPasswordHasher<UserData> _passwordHasher;
    public AccountService(NeoQuasarDBContext dbContext, IPasswordHasher<UserData> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public UserDTO GetUser(UserDTO userDto)
    {
        UserData? user = _dbContext.Users.FirstOrDefault(u => u.Username == userDto.Username);
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
            UserID = user.UserID
        };
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

        if (_dbContext.Users.Any(u => u.Username == username))
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
        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
        return true;
    }
}