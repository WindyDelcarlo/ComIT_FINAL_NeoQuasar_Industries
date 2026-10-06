using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class ReceiptLineData
{
    [Key]
    public int ReceiptLineID{get;set;}
    public int ReceiptID{get;set;}
    public ReceiptData Receipt {get;set;}
    public string ItemSKU {get;set;} = string.Empty;
    public StockItemData LineItem{get;set;}
    public int NumberReceived {get;set;}
}