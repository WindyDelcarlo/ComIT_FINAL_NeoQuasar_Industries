namespace NeoQuasarIndustries;

public static class UserFactory
{
    public static User Create(string? name,string username, string? dimAddress, string? locatorCode, string? sling, string? sN, User.Role? role)
    {
        int id = User.GetDatabase().Count();
        string outName = name == null ? "" : name;
        string outAddress = dimAddress == null ? "" : dimAddress;
        string outLocator = locatorCode == null ? "" : locatorCode;
        string outSling = sling == null ? "" : sling;
        string outSN = sN == null ? "" : sN;
        User.Role outRole = role == null ? User.Role.USER : (User.Role)role;
        User newUser;
        try {
            newUser = new User(id,outName,username,outAddress,outLocator,outSling,outSN,new List<PaymentDetails>(),outRole);
        }
        catch (Exception e)
        {
            throw new Exception("Failed to create user: " + e);
        }
        return newUser;
    }
}