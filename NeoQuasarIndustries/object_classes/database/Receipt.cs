using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class Receipt
{
    public readonly string ID;
    public readonly string OrderID;
    public readonly User User;
    private readonly string? Shipper;
    private readonly string? Recipient;
    private readonly string Date; // Might turn this into not a string later
    private readonly string ShippingAddress;
    private readonly string ContactAddress;
    private readonly string LocationCode; 

  
    // Editables
    private readonly string Courier;
    private readonly string TrackingNumber;
    private readonly List<LineItem> LineItems;

    public Receipt(string id, string orderID, User user, string? shipper, string? receipient, string date, string shippingAddress, string contactAddress, string locCode, string courier, string trackingNumber, List<LineItem> lineItems)
    {
        ID = id;
        OrderID = orderID;
        User = user;
        Shipper = shipper;
        Recipient = receipient;
        Date = date;
        ShippingAddress = shippingAddress;
        ContactAddress = contactAddress;
        LocationCode = locCode;
        Courier = courier;
        TrackingNumber = trackingNumber;
        LineItems = lineItems;
    }
}