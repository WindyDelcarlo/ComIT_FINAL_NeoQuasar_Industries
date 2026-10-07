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

    public static DbSet<ReceiptData> GetDatabase()
    {
        return Program.Database().Receipts;
    }

    public ReceiptData DatabaseThis()
    {
        return GetDatabase().FirstOrDefault(r => r.ReceiptID == ID);
    }

    public Order GetOrder()
    {
        return new Order(Order.GetDatabase().FirstOrDefault(o => o.OrderID == OrderID));
    }

    public static void DatabaseAdd(Receipt r, int[] received)
    {
        OrderData order = Order.GetDatabase().FirstOrDefault(o => o.OrderID == r.OrderID);
        ReceiptData receipt = new ReceiptData
        {
            OrderID = r.OrderID,
            Date = r.Date,
            Courier = r.Courier,
            WaybillNumber = r.TrackingNumber,
            Order = order
        };
        GetDatabase().Add(receipt);
        Program.Database().SaveChanges();
        List<OrderLineData> orderLines = LineItem.OrderLineDatabase().Where(ol => ol.OrderID == order.OrderID).ToList();
        List<ReceiptLineData> receiptLines = new List<ReceiptLineData>();
        for (int i = 0; i < orderLines.Count(); i++)
        {
            orderLines[i].Shipped += received[i];
            if (order.Direction == "OUTGOING")
            {
                StockItem.Get(orderLines[i].ItemSKU).Stock -= orderLines[i].Shipped;
            }
            else
            {
                StockItem.Get(orderLines[i].ItemSKU).Stock += orderLines[i].Shipped;
            }
            ReceiptLineData rld = new ReceiptLineData
            {
                ReceiptID = receipt.ReceiptID,
                ItemSKU = orderLines[i].ItemSKU,
                NumberReceived = received[i]
            };
            
            rld.LineItem = StockItem.GetDatabase().FirstOrDefault(si => si.NeoSKU == rld.ItemSKU);
            receiptLines.Add(rld);
        }
        foreach (ReceiptLineData rld in receiptLines)
        {
            Program.Database().Add(rld);
        }
        Program.Database().SaveChanges();
        order.CheckCompleted();
        
    }
}