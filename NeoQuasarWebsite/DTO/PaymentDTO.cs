namespace NeoQuasarWebsite.DTO;

public class PaymentDTO
{
    public int Amount {get;set;}
    public int CardID {get;set;}
    public string CardNumber {get;set;} = string.Empty;
    public string CardName {get;set;} = string.Empty;
    public string CardType {get;set;} = string.Empty;
    public string Image {get;set;} = string.Empty;
    public bool Selected {get;set;} = false;
}