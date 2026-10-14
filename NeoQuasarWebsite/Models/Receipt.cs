using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public class Receipt
{
    public readonly int ID;
    public readonly string OrderID;
    public readonly string Date; // Might turn this into not a string later
    public readonly string Courier;
    public readonly string TrackingNumber;

    public Receipt(ReceiptData rd) : this(rd.ReceiptID, rd.OrderID, rd.Date, rd.Courier, rd.WaybillNumber)
    {
        
    }

    public Receipt(int id, string orderID, string date, string courier, string trackingNumber)
    {
        ID = id;
        OrderID = orderID;
        Date = date;
        Courier = courier;
        TrackingNumber = trackingNumber;
        
    }

}