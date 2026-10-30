using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class CreateStockItemModel
{
    public string Name {get;set;} = string.Empty;
    public string ManSKU {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public string Manufacturer {get;set;} = string.Empty;
    public string Category {get;set;} = "MISCELLANEOUS";
    public int Price {get;set;} = 0;
    public int Stock {get;set;} = 0;

    public StockItemDTO NewStockItem()
    {
        try {
            return new StockItemDTO{
                ManufacturerSKU = ManSKU,
                ManufacturerName = Manufacturer,
                Name = Name,
                Description = Description,
                Price = Price,
                Category = Category,
                Stock = Stock 
            };
        }
    catch (Exception e)
        {
            throw new Exception("ERROR: " + e);
        }
    }
}