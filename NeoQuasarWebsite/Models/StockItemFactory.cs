using System.Text;

namespace NeoQuasarWebsite.Models;

public class StockItemFactory
{
    public static StockItem Create(string name, string manSKU, string manufacturer, string description, StockCategory category, int price, int stock)
    {
        string neoSKU = CreateNEOSKU(Manufacturer.ManufacturerByString(manufacturer));

        try
        {
            StockItem item = new StockItem(name,neoSKU,manSKU,manufacturer,category,description,price,stock);  
            return item; 
        }
        catch (Exception e)
        {  
            throw new Exception("Error creating object: " + e);   
        }

    }

    private static string CreateNEOSKU(Manufacturer manufacturer)
    {   
        StringBuilder sb = new StringBuilder();

        string dimCode = manufacturer.DimCode.ToUpper();
        if (dimCode.StartsWith("."))
        {
            dimCode = dimCode.Substring(1);
        }
        sb.Append(dimCode);
        sb.Append(manufacturer.Abbreviation);
        sb.Append("-");

        int stockCount = 1;
        foreach (StockItem si in StockItem.GetDatabase())
        {
            if (si.Manufacturer.Equals(manufacturer))
            {
                stockCount++;
            }
        }
        if (stockCount / 100 < 1)
        {
            sb.Append("0");
        }
        if (stockCount / 10 < 1)
        {
            sb.Append("0");
        }
        sb.Append(stockCount);

        return sb.ToString();
    }
}