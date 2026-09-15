namespace NeoQuasarWebsite.Models;

public class Order
{
    // Both
    public Direction ShippingDirection {get;}
    public string ID {get;}
    public UserModel User {get;set;}
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

    private static List<Order> orderDatabase = new List<Order>{
        new Order(Direction.OUTGOING,UserModel.GetDatabase()[1],"PR00000","0000-000",new List<LineItem> {new LineItem()}, "Sella","sella")
        };
    
    public Order() : this(Direction.OUTGOING,UserModel.GetDatabase()[1],"PR00000","0000-000",new List<LineItem> {new LineItem()}, "Sella", "sella")
    {
        
    }

    // Outgoing Orders aka Customers 
    public Order(Direction direction, UserModel user, string id, string date, List<LineItem> lineItems, string recipient, string address, string locCode, string comms)
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
    public Order(Direction direction, UserModel user, string id, string date, List<LineItem> lineItems, string shipper, string comms)
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

    public int GetOutstandingLine(LineItem li)
    {
        int outstanding = li.AmountOrdered;
        if (LineItems.Contains(li)){
            foreach (Receipt r in Receipts)
            {
                foreach (LineItem l in r.LineItems)
                {
                    if (l.Item == li.Item)
                    {
                        outstanding -= l.Shipped;
                    }
                }
            }
            outstanding -= li.Shipped;
        }
        return outstanding;
    
    }

    public int GetTotalPrice()
    {
        int price = 0;
        foreach (LineItem li in LineItems)
        {
            price += li.AmountOrdered * li.Item.Price;
        }
        return price;
    }

    public static List<Order> GetOrderDatabase()
    {
        return orderDatabase;
    }

    public static void DatabaseAdd(Order o)
    {
        orderDatabase.Add(o);
    }

    public enum Direction
    {
        OUTGOING, INCOMING
    }
}