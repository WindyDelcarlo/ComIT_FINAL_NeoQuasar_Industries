using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class ReceiptHistoryViewModel
{
    public ReceiptDTO Receipt {get;set;}
    public List<ReceiptDTO> Receipts {get;set;} 
    public string OrderID {get;set;} = string.Empty;
}