using System.Text;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Services;

public interface IStockService
{
    public void AddStockItem(StockItemDTO dto);
}

public class StockService : IStockService
{
    private NeoQuasarDBContext database;

    public StockService(NeoQuasarDBContext dBContext)
    {
        database = dBContext;
    }

    public void AddStockItem(StockItemDTO dto)
    {
        int manufacturerID = database.Manufacturers.FirstOrDefault(m => m.Name == dto.ManufacturerName).ManufacturerID;
        string neoSKU = GenerateNeoSKU(dto);
        
        database.Add(new StockItemData
        {
            NeoSKU = neoSKU,
            ManufacturerSKU = dto.ManufacturerSKU,
            Name = dto.Name,
            Description = dto.Description,
            ManufacturerID = manufacturerID,
            Price = dto.Price,
            StockCategory = dto.Category,
            ImageLocation = "blankitem.png",
            DontRestock = false,
            Stock = dto.Stock
        });
        database.SaveChanges();
    }

    private string GenerateNeoSKU(StockItemDTO dto)
    {
        ManufacturerData manufacturer = database.Manufacturers.FirstOrDefault(m => m.Name == dto.ManufacturerName);
        
        string dimCode = manufacturer != null ? manufacturer.DimCode.ToUpper() : ".NQI";
        string abbreviation = manufacturer != null ? manufacturer.Abbreviation : "";
        int manufacturerID = manufacturer != null ? manufacturer.ManufacturerID : 1;
        int stockCount = 1 + database.StockItems.Where(si => si.ManufacturerID == manufacturerID).Count();
        bool valid = false;

        if (dimCode.StartsWith("."))
        {
            dimCode = dimCode.Substring(1);
        }
        
        StringBuilder sb = new StringBuilder();
        while (!valid){
            sb.Clear();
            sb.Append(dimCode);
            sb.Append(abbreviation);
            sb.Append("-");

            string number = stockCount.ToString().PadLeft(3,'0');
            sb.Append(number);

            if (database.StockItems.Any(si => si.NeoSKU == sb.ToString()))
            {
                stockCount++;
            }
            else
            {
                valid = true;
            }

        }

        return sb.ToString();
    }
}