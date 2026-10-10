using System.Text.Json;
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
    
    public string CartJson()
    {
        List<object> cartList = new List<object>();
        foreach (CartLineData cld in user.Cart())
        {
            cartList.Add(new
            {
                SKU = cld.ItemSKU,
                ManufacturerSKU = StockItem.Get(cld.ItemSKU).ManufacturerSKU,
                Image = StockItem.Get(cld.ItemSKU).ImageLocation,
                Quantity = cld.Quantity,
                Name = StockItem.Get(cld.ItemSKU).Name,
                Price = StockItem.Get(cld.ItemSKU).Price, 
                Stock = StockItem.Get(cld.ItemSKU).Stock
            });
        }
        string text = JsonSerializer.Serialize(cartList);
        return text;
    }

    public double GetTax()
    {
        return 0;
    }
}