using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class PaymentDetailData
{
    [Key]
    public int PaymentDetailID {get;set;}
    public int UserID{get;set;}
    public string CardName {get;set;} = string.Empty;
    public string CardNumber {get;set;} = string.Empty;
    public string CardType {get;set;} = string.Empty;
    
    // Extrinsic Properties
    public UserData User {get;set;}

    // These aren't even a little bit automatable anyhow
    public string GetImagePath()
    {
        if (CardNumber.StartsWith("ast0"))
        {
            return "cardastera.png";
        }
        else if (CardNumber.Contains("-ST:"))
        {
            return "cardst.png";
        }
        else return "carddefault.png";
    }
}