using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class CartItem
{
    public StockItem Item {get;}
    public int Quantity {get;set;}

    public CartItem(StockItem item) : this(item,1)
    {
    }
    public CartItem(StockItem item, int qty)
    {
        Item = item;
        Quantity = qty;
    }
}