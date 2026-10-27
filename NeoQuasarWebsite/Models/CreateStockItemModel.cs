using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class CreateStockItemModel
{
    public string Name {get;set;} = string.Empty;
    public string ManSKU {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public string Manufacturer {get;set;} = string.Empty;
    public StockCategory Category {get;set;} = StockCategory.MISC;
    public int Price {get;set;} = 0;
    public int Stock {get;set;} = 0;

    public StockItemDTO NewStockItem()
    {
        try {
            return new StockItemDTO{
                ManufacturerSKU = ManSKU,
                Name = Name,
                Description = Description,
                Price = Price,
                Category = StockCategories.CategoryReadable(Category),
                Stock = Stock 
            };
        }
    catch (Exception e)
        {
            throw new Exception("ERROR: " + e);
        }
    }
}