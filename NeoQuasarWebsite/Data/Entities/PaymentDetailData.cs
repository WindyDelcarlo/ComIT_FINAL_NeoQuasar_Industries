using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class PaymentDetailData
{
    [Key]
    public int PaymentDetailID {get;set;}
    public int UserId{get;set;}
    public string CardName {get;set;} = string.Empty;
    public string CardNumber {get;set;} = string.Empty;
    public string CardType {get;set;} = string.Empty;
    
    // Extrinsic Properties
    public UserData User {get;set;}

    public string GetImagePath()
    {
        if (CardNumber.StartsWith("ast0"))
        {
            return "img/cardastera.png";
        }
        else if (CardNumber.Contains("-ST:"))
        {
            return "img/cardst.png";
        }
        else return "img/carddefault.png";
    }
}