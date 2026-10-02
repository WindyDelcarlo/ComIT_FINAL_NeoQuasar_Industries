using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NeoQuasarWebsite.Models;
public class StockManagementModel{
    public List<StockItem> stockItems = new List<StockItem>();
    
    [Required]
    [Display(Name="")]
    public string Name {get;set;} = string.Empty;
    
    [Required]
    [Display(Name="")]
    public string ManSKU {get;set;} = string.Empty;

    [Required]
    [Display(Name="")]
    public string Manufacturer {get;set;} = string.Empty;
    
    [Required]
    [Display(Name="")]
    public int Price {get;set;} = 0;

    [Required]
    [Display(Name="")]
    public int Stock {get;set;} = 0;
    
    [Required]
    [Display(Name="")]
    public string SelectedCategory = "MISCELLANEOUS";

    public string SelectCategory(StockItemModel si)
    {
        return si.Category switch 
        {
            StockCategory.ARCANOHARMONIC_DRIVE => "ahd",
            StockCategory.COMBAT => "combat",
            StockCategory.COMFORT => "comfort",
            StockCategory.COMPUTER => "computer",
            StockCategory.DIMSPACE_THRUSTERS => "thruster",
            StockCategory.FLOWTECH => "flowtech",
            StockCategory.FOOD_AND_DRINK => "food_and_drink",
            StockCategory.LIFE_SUPPORT => "life_support",
            StockCategory.MECHANICAL_PART => "mechanical_part",
            StockCategory.QUANTUM_COMPUTER => "aqc",
            StockCategory.REACTOR => "reactor",
            StockCategory.REACTOR_PART => "reactor_part",
            StockCategory.SENSOR => "sensor",
            StockCategory.TOOL => "tool",
            _ => "misc"
        };
    }

    public List<SelectListItem> catList = new List<SelectListItem>
    {
        new SelectListItem { Value = StockCategory.ARCANOHARMONIC_DRIVE.ToString(), Text = "Arcanoharmonic Drive" },
        new SelectListItem { Value = StockCategory.COMBAT.ToString(), Text = "Combat" },
        new SelectListItem { Value = StockCategory.COMFORT.ToString(), Text = "Comfort" },
        new SelectListItem { Value = StockCategory.COMPUTER.ToString(), Text = "Computers" },
        new SelectListItem { Value = StockCategory.DIMSPACE_THRUSTERS.ToString(), Text = "Thrusters" },
        new SelectListItem { Value = StockCategory.FLOWTECH.ToString(), Text = "Flowtech" },
        new SelectListItem { Value = StockCategory.FOOD_AND_DRINK.ToString(), Text = "Food and Drink" },
        new SelectListItem { Value = StockCategory.LIFE_SUPPORT.ToString(), Text = "Life Support" },
        new SelectListItem { Value = StockCategory.MECHANICAL_PART.ToString(), Text = "Mechanical Part" },
        new SelectListItem { Value = StockCategory.MISC.ToString(), Text = "Miscellaneous" },
        new SelectListItem { Value = StockCategory.QUANTUM_COMPUTER.ToString(), Text = "Quantum Computers" },
        new SelectListItem { Value = StockCategory.REACTOR.ToString(), Text = "Reactors" },
        new SelectListItem { Value = StockCategory.REACTOR_PART.ToString(), Text = "Reactor Parts" },
        new SelectListItem { Value = StockCategory.SENSOR.ToString(), Text = "Sensors" },
        new SelectListItem { Value = StockCategory.TOOL.ToString(), Text = "Tools" }
    };
}