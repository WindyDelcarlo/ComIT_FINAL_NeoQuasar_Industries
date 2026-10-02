using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public class CartItem
{
    public string ItemSKU {get;}
    public int Quantity {get;set;}

    public CartItem(StockItem item) : this(item.NeoSKU,1)
    {
        
    }

    public CartItem(CartLineData cld) : this(cld.ItemSKU,cld.Quantity)
    {
        
    }
    public CartItem( string itemSKU, int qty)
    {
        ItemSKU = itemSKU;
        Quantity = qty;
    }

    public static DbSet<CartLineData> GetDatabase()
    {
        return Program.Database().CartLines;
    }
}