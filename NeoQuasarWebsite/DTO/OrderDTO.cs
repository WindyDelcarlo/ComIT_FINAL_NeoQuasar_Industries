namespace NeoQuasarWebsite.DTO;

public class OrderDTO
{
    // order: {shippingDirection, userId, recipient, shipper, id, data, contactAddress, shipping, shippingAddress, locatorCode, order, trackingNumber, lines}
    // lines[]: {received, outstanding, ordered, neoSKU, description}

    public string ShippingDirection {get;set;} = string.Empty;
    public int UserID {get;set;}
    public string? Recipient {get;set;} = string.Empty;
    public string? Shipper {get;set;} = string.Empty;
    public string ID {get;set;} = string.Empty;
    public string Date {get;set;} = string.Empty;
    public string ContactAddress {get;set;} = string.Empty;
    public string Shipping {get;set;} = string.Empty;
    public string ShippingAddress {get;set;} = string.Empty;
    public string LocatorCode {get;set;} = string.Empty;
    public string Courier {get;set;} = string.Empty;
    public string TrackingNumber {get;set;} = string.Empty;
    public List<LineDTO> lines {get;set;} = new List<LineDTO>();
    public int TotalPrice {get;set;}

}

public class LineDTO
{
    public int Received {get;set;}
    public int Outstanding {get;set;}
    public int Ordered {get;set;}
    public int Price {get;set;}
    public string NeoSKU {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
}