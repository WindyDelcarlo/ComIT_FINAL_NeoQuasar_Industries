using System.Text;
using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public partial class StockItem
{
    public string Name { get; set; }
    public string NeoSKU { get; set; }
    public string ManSKU { get; set; }
    public int ManufacturerID { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public StockCategory Category { get; set; }
    public string ImageLocation { get; set; }
    public bool NeverRestock { get; set; } = false;
    public int Stock { get; set; } = 0;

    public StockItem() : this("TEST", "NEO-001", "NQITEST", "NeoQuasar", StockCategory.MISC, "A test item. You shouldn't be able to buy this.", 0, 0, "blankitem.png")
    {

    }

    public StockItem(string name, string neoSKU, string manSKU, string manufacturer, StockCategory category, string description, int price, int stock) : this(name, neoSKU, manSKU, manufacturer, category, description, price, stock, "blankitem.png")
    {

    }

    public StockItem(StockItemData sid) : this(sid.Name, sid.NeoSKU, sid.ManufacturerSKU, Manufacturer.GetDatabase().Find(sid.ManufacturerID).Name, (StockCategory)Enum.Parse(typeof(StockCategory), sid.StockCategory), sid.Description, sid.Price, sid.Stock, sid.ImageLocation)
    {

    }

    public StockItem(string name, string neoSKU, string manSKU, string manufacturer, StockCategory category, string description, int price, int stock, string img)
    {
        Name = name;
        NeoSKU = neoSKU;
        ManSKU = manSKU;
        ManufacturerID = Manufacturer.ManufacturerByString(manufacturer);
        Category = category;
        Description = description;
        Price = price;
        Stock = stock;
        ImageLocation = img;
    }

    public static StockCategory CatFromInt(int i)
    {
        List<StockCategory> list = [.. Enum.GetValues(typeof(StockCategory)).Cast<StockCategory>()];
        return list[i];
    }

    public static DbSet<StockItemData> GetDatabase()
    {
        return Program.Database().StockItems;
    }

    public static void DatabaseAdd(StockItem item)
    {
        try
        {
            GetDatabase().Add(new StockItemData
            {
                NeoSKU = item.NeoSKU,
                ManufacturerSKU = item.ManSKU,
                Name = item.Name,
                Description = item.Description,
                ManufacturerID = item.ManufacturerID,
                Price = item.Price,
                StockCategory = item.Category.ToString(),
                ImageLocation = item.ImageLocation,
                DontRestock = item.NeverRestock,
                Stock = item.Stock
            });
            Program.Database().SaveChanges();
        }
        catch
        {
            Console.WriteLine($"StockItem {item.NeoSKU} already exists.");
        }
    }
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        StockItem? testItem = obj as StockItem;
        if (testItem == null)
        {
            return false;
        }
        else if (testItem.NeoSKU != NeoSKU)
        {
            return false;
        }
        return true;
    }

    public static StockItemData Get(string neoSKU)
    {
        return GetDatabase().FirstOrDefault(si => si.NeoSKU == neoSKU);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{Name.ToUpper()} - {Category.ToString().Replace("_", " ")}");
        sb.AppendLine($"NEO #{NeoSKU} :: {Manufacturer.GetDatabase().Find(ManufacturerID)}");
        sb.AppendLine(Description);
        sb.Append($"{Price} SIC");
        return sb.ToString();
    }
}

public enum StockCategory
{
    REACTOR,
    REACTOR_PART,
    QUANTUM_COMPUTER,
    ARCANOHARMONIC_DRIVE,
    FLOWTECH,
    SENSOR,
    COMPUTER,
    DIMSPACE_THRUSTERS,
    MECHANICAL_PART,
    FOOD_AND_DRINK,
    COMFORT,
    COMBAT,
    LIFE_SUPPORT,
    TOOL,
    MISC

}
