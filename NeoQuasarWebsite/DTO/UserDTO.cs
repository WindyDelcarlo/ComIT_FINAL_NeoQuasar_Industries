namespace NeoQuasarWebsite.DTO;

public class UserDTO
{
    public int UserID {get;set;}
    public string Username {get;set;} = string.Empty;
    public string Name {get;set;} = string.Empty;
    public string? StellarNet {get;set;} = string.Empty;
    public string? SlingName {get;set;} = string.Empty;
    public string Address {get;set;} = string.Empty;
    public string LocatorCode {get;set;} = string.Empty;
    public string Password {get;set;} = string.Empty;
}