using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class Receipt
{
    // Both
    private Direction ShippingDirection {get;}
    private string ID {get;}
    private string ShipperRecipient {get;} // Don't need shipper for outgoing orders
    private string Courier {get;}
    private string CommAddress{get;}
    private string TrackingNumber {get;}
    private string Date {get;} // Might turn this into not a string later
    private List<LineItem> LineItems = new List<LineItem>();

    // Incoming Only
    private string Address {get;}
    private string DimAddress {get;} 

    public Receipt(Direction direction, string date, string id, string shipperRecipient, string courier, string commAddress, string trackingNumber, List<LineItem> lineItems)
    : this(direction, date, id, shipperRecipient,courier,commAddress,trackingNumber,lineItems,"","") {
        
    }

    public Receipt(Direction direction, string date, string id, string shipperRecipient, string courier, string commAddress, string trackingNumber, List<LineItem> lineItems, string address, string dimAddress)
    {
        ShippingDirection = direction;
        Date = date;
        ID = id;
        ShipperRecipient = shipperRecipient;
        Courier = courier;
        CommAddress = commAddress;
        TrackingNumber = trackingNumber;
        LineItems.AddRange(lineItems);
        Address = address;
        DimAddress = dimAddress;
    }


    public enum Direction
    {
        OUTGOING, INCOMING
    }
}