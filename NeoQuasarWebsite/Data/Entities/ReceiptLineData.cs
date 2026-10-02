using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class ReceiptLineData
{
    [Key]
    public string ReceiptLineID{get;set;} = string.Empty;
    public string ReceiptID{get;set;} = string.Empty;
    public ReceiptData Receipt {get;set;}
    public string ItemSKU {get;set;} = string.Empty;
    public StockItemData LineItem{get;set;}
    public int NumberReceived {get;set;}
}