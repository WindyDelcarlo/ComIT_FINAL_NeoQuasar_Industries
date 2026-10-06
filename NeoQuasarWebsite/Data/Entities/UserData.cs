using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class UserData
{
    [Key]
    public int UserID {get;set;}
    public string Username {get;set;}
    public string Name {get;set;}

    public string? StellarNet {get;set;}
    public string? SlingName {get;set;}
    public string Address {get;set;}
    public string LocatorCode {get;set;}
    public string Role {get;set;}
    public int SelectedCard {get;set;}
    public string PassHash {get;set;}
    
    // Extrinsic Properties
    public List<PaymentDetailData> PaymentDetails {get;set;} = new List<PaymentDetailData>();
    public List<CartLineData> CartLines {get;set;} = new List<CartLineData>();
}