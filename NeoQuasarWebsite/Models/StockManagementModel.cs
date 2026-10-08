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

    public List<SelectListItem> catList = new List<SelectListItem>();

    public StockManagementModel()
    {
        foreach (StockCategory cat in Enum.GetValues(typeof(StockCategory)))
        {
            catList.Add(new SelectListItem { Value = cat.ToString(), Text = StockItem.CategoryReadable(cat)});
        }
    }
}