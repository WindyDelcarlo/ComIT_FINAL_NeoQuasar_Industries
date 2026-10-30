using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;
public class StockManagementViewModel{
    
    [Required]
    public string Name {get;set;} = string.Empty;
    
    [Required]
    public string ManSKU {get;set;} = string.Empty;

    [Required]
    public string Manufacturer {get;set;} = string.Empty;
    
    [Required]
    public int Price {get;set;} = 0;

    [Required]
    public int Stock {get;set;} = 0;
    
    [Required]
    public string Category = "MISCELLANEOUS";

    public List<SelectListItem> catList = new List<SelectListItem>();

    public StockManagementViewModel()
    {
        foreach (StockCategory cat in Enum.GetValues(typeof(StockCategory)))
        {
            catList.Add(new SelectListItem { Value = cat.ToString(), Text = StockCategories.CategoryReadable(cat)});
        }
    }
}