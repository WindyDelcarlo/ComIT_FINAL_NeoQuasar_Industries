using System.Text;
using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public partial class StockCategories
{

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
