using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class Order
{
    // Both
    private Direction ShippingDirection {get;}
    public string ID {get;}
    public User User {get;set;}
    public string Shipper {get;} = ""; // Don't need shipper for outgoing order, replaced by Recipient
    public string Recipient {get;} = "";
    public string Date {get;} // Might turn this into not a string later
    public string ShippingAddress {get;set;} = "";
    public string ContactAddress {get;set;} = "";
    public string LocationCode {get;set;} = ""; 

  
    // Editables
    public string Courier {get;set;} = "";
    public string TrackingNumber {get;set;} = "";
    public List<LineItem> LineItems = new List<LineItem>();
    public List<Receipt> Receipts = new List<Receipt>();
    public bool Outstanding {get;set;} = true;
    
    // Outgoing Orders aka Customers 
    public Order(Direction direction, User user, string id, string date, List<LineItem> lineItems, string recipient, string address, string locCode, string comms)
    {
        ShippingDirection = direction;
        User = user;
        Recipient = recipient;
        ID = id;
        Date = date;
        LineItems.AddRange(lineItems);
        ShippingAddress = address;
        ContactAddress = comms;
        LocationCode = locCode;
    }
    // Incoming Orders AKA stock and requisitions
    public Order(Direction direction, User user, string id, string date, List<LineItem> lineItems, string shipper, string comms)
    {
        ShippingDirection = direction;
        User = user;
        ID = id;
        Date = date;
        LineItems.AddRange(lineItems);
        Shipper = shipper;
        ContactAddress = comms;
    }

    public void CheckCompletion()
    {
        foreach (LineItem l in LineItems)
        {
            if (l.AmountOrdered > l.Shipped)
            {
                return;
            }
        }
        Outstanding = false;
    }

    public enum Direction
    {
        OUTGOING, INCOMING
    }
}