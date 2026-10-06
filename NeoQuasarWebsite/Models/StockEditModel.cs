using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public class StockEditModel
{
    public string Name {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public int Price {get;set;}
    public int Stock {get;set;}
    public string ImageLocation {get;set;} = string.Empty;
    public string NeoSKU {get;set;} = string.Empty;
    public bool NeverRestock {get;set;}

    public StockItemData Item()
    {
        return StockItem.GetDatabase().FirstOrDefault(si => si.NeoSKU == NeoSKU);
    }
}