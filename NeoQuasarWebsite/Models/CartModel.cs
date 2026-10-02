using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public class CartModel
{
    public UserModel user;

    public int CartTotal()
    {
        int i = 0;
        foreach (CartLineData cld in user.Cart())
        {
            i += StockItem.Get(cld.ItemSKU).Price * cld.Quantity;
        }
        return i;
    }

    public double GetTax()
    {
        return 0;
    }
}