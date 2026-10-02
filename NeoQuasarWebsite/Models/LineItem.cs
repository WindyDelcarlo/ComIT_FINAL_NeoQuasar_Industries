using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;
public class LineItem
{
    public string ItemSKU {get;}
    public int AmountOrdered {get;}

    public int BackOrdered {get;set;}

    public int Shipped {get;set;}

    public LineItem() : this("0", 1)
    {
        
    }

    public LineItem(OrderLineData old) : this(old.ItemSKU, old.AmountOrdered, old.Shipped)
    {
        
    } 

    public LineItem(CartLineData cld) : this(cld.ItemSKU, cld.Quantity, 0)
    {
        
    }
    
    public LineItem(string itemSKU, int amountOrdered) : this(itemSKU, amountOrdered, 0)
    {
        
    }

    public LineItem(string itemSKU, int amountOrdered, int shipped)
    {
        ItemSKU = itemSKU;
        AmountOrdered = amountOrdered;
        BackOrdered = 0;
        Shipped = shipped;
    }

    public static List<LineItem> LineItemsFromCart(List<CartLineData> cart)
    {
        List<LineItem> lines = new List<LineItem>();
        foreach (CartLineData cld in cart)
        {
            lines.Add(new LineItem(cld));
        }
        return lines;
    }

    public static DbSet<OrderLineData> OrderLineDatabase()
    {
        return Program.Database().OrderLines;
    }

    public static void AddOrderDatabase(string orderID, LineItem li)
    {
        OrderLineDatabase().Add(new OrderLineData
        {
            OrderID = orderID,
            ItemSKU = li.ItemSKU,
            AmountOrdered = li.AmountOrdered,
            Shipped = 0,
            LineItem = StockItem.GetDatabase().FirstOrDefault(si => si.NeoSKU == li.ItemSKU)
        });
        Program.Database().SaveChanges();
    }
    
    public static DbSet<ReceiptLineData> ReceiptLineDatabase()
    {
        return Program.Database().ReceiptLines;
    }

    public static DbSet<CartLineData> CartLineDatabase()
    {
        return Program.Database().CartLines;
    }
}