namespace NeoQuasarWebsite.Models;

public class StockItemModel
{
    public string Name {get;set;}
    public string ManSKU {get;set;}
    public string Description {get;set;}
    public string Manufacturer {get;set;}
    public StockCategory Category {get;set;}
    public int Price {get;set;}

    public StockItem NewStockItem()
    {
        try {
            return StockItemFactory.Create(Name,ManSKU,Manufacturer,"",Category,Price);
        }
    catch (Exception e)
        {
            throw new Exception("ERROR: " + e);
        }
    }
}