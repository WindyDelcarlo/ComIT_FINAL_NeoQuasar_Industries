using System.Text.Json;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class StorefrontViewModel
{
    public List<string> SearchTerms {get;set;} = new List<string>();
    public int SearchResultCount {get;set;} = 0;
    public List<StockItemDTO> SearchResults {get;set;} = new List<StockItemDTO>();
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

    public string SearchResultsJson()
    {
        List<object> jsonObjects = new List<object>();
        foreach (StockItemDTO si in SearchResults)
        {
            jsonObjects.Add(new {
                sku = si.NeoSKU,
                image = si.Image,
                description = si.Description,
                name = si.Name,
                manufacturer = si.ManufacturerName,
                manufacturerSKU = si.ManufacturerSKU,
                price = si.Price,
                cart = StockInCart(si),
                stock = si.Stock
            });
        }
        return JsonSerializer.Serialize(jsonObjects);
    }
}