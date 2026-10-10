using System.Text.Json;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public class SearchModel
{
    public List<string> SearchTerms {get;set;} = new List<string>();
    public int SearchResultCount {get;set;} = 0;
    public List<StockItem> SearchResults {get;set;} = new List<StockItem>();
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

    public string SearchResultsJson()
    {
        List<object> jsonObjects = new List<object>();
        foreach (StockItem si in SearchResults)
        {
            jsonObjects.Add(new {
                sku = si.NeoSKU,
                image = si.ImageLocation,
                description = si.Description,
                name = si.Name,
                manufacturer = Manufacturer.GetDatabase().FirstOrDefault(m => m.ManufacturerID == si.ManufacturerID).Name,
                manufacturerSKU = si.ManSKU,
                price = si.Price,
                cart = StockInCart(si),
                stock = si.Stock
            });
        }
        return JsonSerializer.Serialize(jsonObjects);
    }
}