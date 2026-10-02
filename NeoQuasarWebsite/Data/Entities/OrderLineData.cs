using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class OrderLineData
{
    [Key]
    public int OrderLineID { get;set;}
    public string ItemSKU {get;set;} = string.Empty;
    public string OrderID {get;set;} = string.Empty;
    public int AmountOrdered {get;set;}
    public int Shipped {get;set;}

    // Extrinsic Properties
    public OrderData Order{get;set;}
    public StockItemData LineItem{get;set;}
}