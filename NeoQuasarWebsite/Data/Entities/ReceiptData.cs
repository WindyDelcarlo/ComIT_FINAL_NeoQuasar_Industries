using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class ReceiptData
{
    [Key]
    public int ReceiptID {get;set;}
    public string OrderID {get;set;} = string.Empty;
    public string Date {get;set;} = string.Empty;
    public string Courier {get;set;} = string.Empty;
    public string WaybillNumber {get;set;} = string.Empty;

    public OrderData Order {get;set;}
    public List<ReceiptLineData> ReceiptLines {get;set;}
}