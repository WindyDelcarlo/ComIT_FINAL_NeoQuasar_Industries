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
}