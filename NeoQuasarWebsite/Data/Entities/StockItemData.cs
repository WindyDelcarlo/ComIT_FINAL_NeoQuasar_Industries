using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class StockItemData
{
    [Key]
    public string NeoSKU {get;set;}
    public string ManufacturerSKU {get;set;}
    public string Name {get;set;}
    public int ManufacturerID {get;set;}
    public ManufacturerData Manufacturer {get;set;}
    public string Description {get;set;}
    public int Price {get;set;}
    public string StockCategory {get;set;}
    public string ImageLocation {get;set;}
    public bool DontRestock {get;set;}
    public int Stock {get;set;}

    // Extrinsic Properties
    public List<ReceiptLineData> ReceiptLines {get;set;}
    public List<OrderLineData> OrderLines {get;set;}
    public List<CartLineData> CartLines {get;set;}
}