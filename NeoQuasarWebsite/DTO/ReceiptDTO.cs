namespace NeoQuasarWebsite.DTO;

public class ReceiptDTO
{
    public int ReceiptID {get;set;}
    public string OrderID {get;set;} = string.Empty;
    public string Date {get;set;} = string.Empty;
    public string? Shipper {get;set;} = string.Empty;
    public string? Recipient {get;set;} = string.Empty;
    public string ContactAddress {get;set;} = string.Empty;
    public string Courier {get;set;} = string.Empty;
    public string TrackingNumber {get;set;} = string.Empty;
    public List<LineDTO> Lines {get;set;} = new List<LineDTO>();
}