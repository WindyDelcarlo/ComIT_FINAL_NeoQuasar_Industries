namespace NeoQuasarWebsite.DTO;

public class StockItemDTO
{
    public string NeoSKU {get;set;} = string.Empty;
    public string Image {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public string Name {get;set;} = string.Empty;
    public string ManufacturerName {get;set;} = string.Empty;
    public string ManufacturerSKU {get;set;} = string.Empty;
    public string Category {get;set;} = string.Empty;
    public int Price {get;set;}
    public int Stock {get;set;}
    public bool NoRestock {get;set;}
}