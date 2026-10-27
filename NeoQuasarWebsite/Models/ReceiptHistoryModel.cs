using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class ReceiptHistoryModel
{
    public ReceiptDTO Receipt {get;set;}
    public List<ReceiptDTO> Receipts {get;set;} 
    public string OrderID {get;set;} = string.Empty;
}