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

}