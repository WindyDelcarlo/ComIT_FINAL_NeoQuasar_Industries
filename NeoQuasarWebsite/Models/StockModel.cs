using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public class StockModel
{
    public StockItem Item {get;set;}
    public UserModel User {get;set;}
    
    public int StockInCart(StockItem item)
    {
        foreach(CartLineData cld in User.Cart())
        {
            if (cld.ItemSKU == item.NeoSKU)
            {
                return cld.Quantity;
            }
        }
        return 0;
    }
}