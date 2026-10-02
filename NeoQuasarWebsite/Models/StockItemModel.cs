namespace NeoQuasarWebsite.Models;

public class StockItemModel
{
    public string Name {get;set;} = string.Empty;
    public string ManSKU {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public string Manufacturer {get;set;} = string.Empty;
    public StockCategory Category {get;set;} = StockCategory.MISC;
    public int Price {get;set;} = 0;
    public int Stock {get;set;} = 0;

    public StockItem NewStockItem()
    {
        try {
            return StockItemFactory.Create(Name,ManSKU,Manufacturer,Category,"",Price,Stock);
        }
    catch (Exception e)
        {
            throw new Exception("ERROR: " + e);
        }
    }
}