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
    public string Direction {get;set;} = string.Empty;
    public string Date {get;set;} = string.Empty;
    public bool Completed {get;set;}

    public string ShippingOption {get;set;} = string.Empty;

    // Extrinsic Properties
    public UserData User {get;set;}
    public List<OrderLineData> OrderLines {get;set;}
    public List<ReceiptData> Receipts {get;set;}


    public string ContactAddress()
    {
        return string.IsNullOrEmpty(UserModel.GetDatabase().Find(UserID).StellarNet) ? UserModel.GetDatabase().Find(UserID).SlingName : UserModel.GetDatabase().Find(UserID).StellarNet;
    }

    public bool CheckCompleted()
    {
        List<OrderLineData> orderLines = LineItem.OrderLineDatabase().Where(ol => ol.OrderID == OrderID).ToList();
        foreach (OrderLineData old in orderLines)
        {
            if (old.Shipped < old.AmountOrdered)
            {
                return false;
            }
        }
        Completed = true;
        Program.Database().SaveChanges();
        return true;
    }
}