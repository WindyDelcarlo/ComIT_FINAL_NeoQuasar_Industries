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

    public StockItem() : this("TEST", "NEO-001", "NQITEST", 1, StockCategory.MISC, "A test item. You shouldn't be able to buy this.", 0, 0, "blankitem.png")
    {

    }

    public StockItem(string name, string neoSKU, string manSKU, int manufacturerId, StockCategory category, string description, int price, int stock) : this(name, neoSKU, manSKU, manufacturerId, category, description, price, stock, "blankitem.png")
    {

    }

    public StockItem(StockItemData sid) : this(sid.Name, sid.NeoSKU, sid.ManufacturerSKU, sid.ManufacturerID, (StockCategory)Enum.Parse(typeof(StockCategory), sid.StockCategory), sid.Description, sid.Price, sid.Stock, sid.ImageLocation)
    {

    }

    public StockItem(string name, string neoSKU, string manSKU, int manufacturerId, StockCategory category, string description, int price, int stock, string img)
    {
        Name = name;
        NeoSKU = neoSKU;
        ManSKU = manSKU;
        ManufacturerID = manufacturerId;
        Category = category;
        Description = description;
        Price = price;
        Stock = stock;
        ImageLocation = img;
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

    public static string CategoryReadable(StockCategory cat)
    {
        return cat switch
        {
            StockCategory.REACTOR => "Reactor",
            StockCategory.REACTOR_PART => "Reactor Part",
            StockCategory.QUANTUM_COMPUTER => "Quantum Computer",
            StockCategory.ARCANOHARMONIC_DRIVE => "Arcanoharmonic Drive",
            StockCategory.FLOWTECH => "Flowtech",
            StockCategory.SENSOR => "Sensor",
            StockCategory.COMPUTER => "Computer",
            StockCategory.DIMSPACE_THRUSTERS => "Dimspace Thrusters",
            StockCategory.MECHANICAL_PART => "Mechanical Part",
            StockCategory.FOOD_AND_DRINK => "Food And Drink",
            StockCategory.COMFORT => "Comfort",
            StockCategory.COMBAT => "Combat",
            StockCategory.LIFE_SUPPORT => "Life Support",
            StockCategory.TOOL => "Tool",
            StockCategory.MISC => "Miscellaneous",
            _ => ""
        };
    }

    public static StockCategory CategoryFromString(string name)
    {
        return name switch
        {
            "Reactor" or "REACTOR" => StockCategory.REACTOR,
            "Reactor Part" or "REACTOR_PART" => StockCategory.REACTOR_PART,
            "Quantum Computer" or "QUANTUM_COMPUTER" => StockCategory.QUANTUM_COMPUTER,
            "Arcanoharmonic Drive" or "ARCANOHARMONIC_DRIVE" => StockCategory.ARCANOHARMONIC_DRIVE,
            "Flowtech" or "FLOWTECH" => StockCategory.FLOWTECH,
            "Sensor" or "SENSOR" => StockCategory.SENSOR,
            "Computer" or "COMPUTER" => StockCategory.COMPUTER,
            "Dimspace Thrusters" or "DIMSPACE_THRUSTERS" => StockCategory.DIMSPACE_THRUSTERS,
            "Mechanical Part" or "MECHANICAL_PART" => StockCategory.MECHANICAL_PART,
            "Food And Drink" or "FOOD_AND_DRINK" => StockCategory.FOOD_AND_DRINK,
            "Comfort" or "COMFORT" => StockCategory.COMFORT,
            "Combat" or "COMBAT" => StockCategory.COMBAT,
            "Life Support" or "LIFE_SUPPORT" => StockCategory.LIFE_SUPPORT,
            "Tool" or "TOOL" => StockCategory.TOOL,
            _ => StockCategory.MISC
        };
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
