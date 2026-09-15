using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Models;

public static class UserFactory
{
    public static UserModel Create(string? name,string username, string? dimAddress, string? locatorCode, string? sling, string? sN, UserRole? role)
    {
        int id = UserModel.GetDatabase().Count();
        string outName = name == null ? "" : name;
        string outAddress = dimAddress == null ? "" : dimAddress;
        string outLocator = locatorCode == null ? "" : locatorCode;
        string outSling = sling == null ? "" : sling;
        string outSN = sN == null ? "" : sN;
        UserRole outRole = role == null ? UserRole.USER : (UserRole)role;
        UserModel newUser;
        try {
            newUser = new UserModel(id,outName,username,outAddress,outLocator,outSling,outSN,new List<PaymentDetails>(),outRole);
        }
        catch (Exception e)
        {
            throw new Exception("Failed to create user: " + e);
        }
        return newUser;
    }
}