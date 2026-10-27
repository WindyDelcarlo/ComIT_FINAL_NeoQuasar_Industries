using System.Text.Json;
using System.Text.Json.Serialization;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class StockViewModel
{
    public StockItemDTO Item {get;set;}
    public UserDTO User {get;set;}
    
    public int StockInCart(StockItemDTO item)
    {
        foreach(CartLineData cld in User.Cart)
        {
            if (cld.ItemSKU == item.NeoSKU)
            {
                return cld.Quantity;
            }
        }
        return 0;
    }

    public string ItemJson()
    {
        return JsonSerializer.Serialize(new
        {
            neoSKU = Item.NeoSKU,
            cart = StockInCart(Item),
            stock = Item.Stock
        });
    }
}