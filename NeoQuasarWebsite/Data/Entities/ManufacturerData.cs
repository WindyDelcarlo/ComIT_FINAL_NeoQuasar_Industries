using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class ManufacturerData
{
    [Key]
    public int ManufacturerID {get;set;}
    public string Name {get;set;} = string.Empty;
    public string Abbreviation {get;set;} = string.Empty;
    public string DimCode {get;set;} = string.Empty;
    public string ContactAddress {get;set;} = string.Empty;

    // Extrinsic Properties
    public List<StockItemData> StockItems {get;set;}

}