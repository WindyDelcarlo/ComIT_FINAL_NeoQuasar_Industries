using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class UserData
{
    [Key]
    public int UserID {get;set;}
    public string Username {get;set;} = string.Empty;
    public string Name {get;set;} = string.Empty;

    public string? StellarNet {get;set;}
    public string? SlingName {get;set;}
    public string Address {get;set;} = string.Empty;
    public string LocatorCode {get;set;} = string.Empty;
    public string Role {get;set;} = string.Empty;
    public int SelectedCard {get;set;}
    public string PassHash {get;set;} = string.Empty;
    
    // Extrinsic Properties
    public List<PaymentDetailData> PaymentDetails {get;set;} = new List<PaymentDetailData>();
    public List<CartLineData> CartLines {get;set;} = new List<CartLineData>();

    public string ContactAddress()
    {
        return StellarNet != null ? StellarNet : SlingName;
    }
}