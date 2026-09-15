using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class Receipt
{
    public readonly string ID;
    public readonly string OrderID;
    public readonly User User;
    public readonly string Shipper = "";
    public readonly string Recipient = "";
    public readonly string Date; // Might turn this into not a string later
    public readonly string ShippingAddress;
    public readonly string ContactAddress;
    public readonly string LocationCode; 
    public readonly string Courier;
    public readonly string TrackingNumber;
    public readonly List<LineItem> LineItems;

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