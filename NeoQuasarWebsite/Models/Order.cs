using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public class Order
{
    // Both
    public Direction ShippingDirection {get;}
    public string ID {get;}
    public int UserID {get;set;}
    public string Shipper {get;} = ""; // Don't need shipper for outgoing order, replaced by Recipient
    public string Recipient {get;} = "";
    public string Date {get;} // Might turn this into not a string later
    public string ShippingAddress {get;set;} = "";
    public string ContactAddress {get;set;} = "";
    public string LocationCode {get;set;} = ""; 
    public string ShippingOption {get;set;} = "";

    public bool Outstanding {get;set;} = true;

    private static List<Order> orderDatabase = new List<Order>{
        new Order(Direction.OUTGOING,1,"PR00000","0000-000", "Sella","sella")
        };
    
    public Order() : this(Direction.OUTGOING,1,"PR00000","0000-000", "Sella", "sella")
    {
        
    }

    public Order(OrderData od) : this((Direction)Enum.Parse(typeof(Direction), od.Direction),od.UserID,od.OrderID,od.Date,od.Shipper,od.ContactAddress())
    {
        Recipient = od.Recipient;
        if (od.Direction == "OUTGOING")
        {
            UserModel user = new UserModel(UserModel.GetUser(od.UserID));
            LocationCode = user.LocatorCode;
            ShippingAddress = user.Address;
        }
        ShippingOption = od.ShippingOption;
        Outstanding = !od.Completed;
    }
    public Order(Direction direction, int userID, string id, string date, string recipient, string address, string locCode, string comms) : this(direction,userID,id,date,recipient,address,locCode,comms,null)
    {
        
    }
    
    // Outgoing Orders aka Customers 
    public Order(Direction direction, int userID, string id, string date, string recipient, string address, string locCode, string comms, ShippingOptions? shipping)
    {
        ShippingDirection = direction;
        UserID = userID;
        Recipient = recipient;
        ID = id;
        Date = date;
        ShippingAddress = address;
        ContactAddress = comms;
        LocationCode = locCode;
        switch(shipping){
            case null:
                {
                    break;
                }
            case ShippingOptions.STANDARD:
                {
                    ShippingOption = "Standard";
                    break;
                }
            case ShippingOptions.EXPEDITED:
                {
                    ShippingOption = "Expedited";
                    break;
                }
            case ShippingOptions.EMERGENCY:
                {
                    ShippingOption = "Emergency";
                    break;
                }
        }
    }
    // Incoming Orders AKA stock and requisitions
    public Order(Direction direction, int userID, string id, string date, string shipper, string comms)
    {
        ShippingDirection = direction;
        UserID = userID;
        ID = id;
        Date = date;
        Shipper = shipper;
        ContactAddress = comms;
    }

    public void CheckCompletion()
    {
        foreach (OrderLineData old in OrderLines())
        {
            if (old.AmountOrdered > old.Shipped)
            {
                return;
            }
        }
        Outstanding = false;
    }

    /*
    public int GetOutstandingLine(OrderLineData old)
    {
        int outstanding = old.AmountOrdered;
        if (OrderLines().Contains(old)){
            foreach (ReceiptData rd in Receipt.GetDatabase())
            {
                foreach (LineItem l in r.LineItems)
                {
                    if (l.ItemSKU == li.ItemSKU)
                    {
                        outstanding -= l.Shipped;
                    }
                }
            }
            outstanding -= li.Shipped;
        }
        return outstanding;
    
    } */

    public int GetTotalPrice()
    {
        List<OrderLineData> lines = OrderLines();
        int price = 0;
        foreach (OrderLineData old in lines)
        {
            price += old.AmountOrdered * StockItem.Get(old.ItemSKU).Price;
        }
        return price;
    }

    public OrderData DatabaseThis()
    {
        return GetDatabase().FirstOrDefault(o => o.OrderID == ID);
    }

    public List<OrderLineData> OrderLines()
    {
        return Program.Database().OrderLines.Where(old => old.OrderID == ID).ToList();
    }

    public static DbSet<OrderData> GetDatabase()
    {
        return Program.Database().Orders;
    }

    public static void DatabaseAdd(Order o)
    {
        GetDatabase().Add(new OrderData{
            OrderID = o.ID,
            UserID = o.UserID,
            Shipper = o.Shipper,
            Recipient = o.Recipient,
            Direction = o.ShippingDirection.ToString(),
            Date = o.Date,
            Completed = false,
            ShippingOption = o.ShippingOption
        });
    }

    public enum Direction
    {
        OUTGOING, INCOMING
    }
}