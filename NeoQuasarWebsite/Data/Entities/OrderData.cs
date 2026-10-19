using System.ComponentModel.DataAnnotations;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Data;

public class OrderData
{
    [Key]
    public string OrderID{get;set;} = string.Empty;
    public int UserID{get;set;}
    public string? Shipper {get;set;}
    public string? Recipient {get;set;}
    public string ContactAddress {get;set;} = string.Empty;
    public string ShippingAddress {get;set;} = string.Empty;
    public string LocatorCode {get;set;} = string.Empty;
    public string Direction {get;set;} = string.Empty;
    public string Date {get;set;} = string.Empty;
    public bool Completed {get;set;}

    public string ShippingOption {get;set;} = string.Empty;

    // Extrinsic Properties
    public UserData User {get;set;}
    public List<OrderLineData> OrderLines {get;set;}
    public List<ReceiptData> Receipts {get;set;}

}