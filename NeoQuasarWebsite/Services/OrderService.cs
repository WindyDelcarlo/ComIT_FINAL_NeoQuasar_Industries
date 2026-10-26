using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Services;

public interface IOrderService
{
    public string CreateOrder(int userID, string direction, string shippingOption, string shiprec, string contact, string address, string locCode);
    public void AddLine(string orderID, CartLineData cld);
    public void CreateReceipt(OrderDTO order);
    public void AddReceiptLine(string orderID, string neoSKU, int received, string waybill);
    public void DeleteReceipt(ReceiptData receipt);
    public void CompleteOrder(OrderDTO order);
}

public class OrderService : IOrderService
{
    NeoQuasarDBContext database;

    public OrderService(NeoQuasarDBContext context)
    {
        database = context;
    }

    public string CreateOrder(int userID, string direction, string shippingOption, string shiprec, string contact, string address, string locCode)
    {
        string orderID = "0";
        string orderLabel = direction.ToUpper() == "INCOMING" ? "PR" : "PO";
        string shipper = direction.ToUpper() == "INCOMING" ? shiprec : string.Empty;
        string recipient = direction.ToUpper() == "INCOMING" ? string.Empty : shiprec;
        string shipping = shippingOption;
        switch (shipping)
        {
            case "STANDARD":
                { shipping = "Standard"; break; }
            case "EXPEDITED":
                { shipping = "Expedited"; break; }
            case "EMERGENCY":
                { shipping = "Emergency"; break; }

        }

        bool valid = false;
        int orderCount = database.Orders.Count();
        while (!valid)
        {
            orderID = orderLabel + orderCount.ToString().PadLeft(5, '0');
            if (database.Orders.Any(o => o.OrderID == orderID))
            {
                orderCount++;
            }
            else
            {
                valid = true;
            }
        }

        database.Add(new OrderData
        {
            OrderID = orderID,
            UserID = userID,
            Shipper = shipper,
            Recipient = recipient,
            Direction = direction,
            ContactAddress = contact,
            ShippingAddress = address,
            LocatorCode = locCode,
            Date = Program.Date(),
            Completed = false,
            ShippingOption = shipping
        });
        database.SaveChanges();

        return orderID;
    }

    public void AddLine(string orderID, CartLineData cld)
    {
        database.Add(new OrderLineData
        {
            OrderID = orderID,
            ItemSKU = cld.ItemSKU,
            Shipped = 0,
            AmountOrdered = cld.Quantity,
            LineItem = database.StockItems.FirstOrDefault(sid => sid.NeoSKU == cld.ItemSKU)
        });
        database.SaveChanges();
    }

    public void CreateReceipt(OrderDTO order)
    {
        database.Add(new ReceiptData
        {
            OrderID = order.ID,
            Date = Program.Date(),
            Courier = order.Courier,
            WaybillNumber = order.TrackingNumber,
            ContactAddress = order.ContactAddress
        });
        database.SaveChanges();
    }

    public void AddReceiptLine(string orderID, string neoSKU, int received, string waybill)
    {
        ReceiptData receipt = database.Receipts.FirstOrDefault(r => r.OrderID == orderID && r.WaybillNumber == waybill);
        database.Add(new ReceiptLineData
        {
            ReceiptID = receipt.ReceiptID,
            ItemSKU = neoSKU,
            LineItem = database.StockItems.FirstOrDefault(sid => sid.NeoSKU == neoSKU),
            NumberReceived = received
        });
        database.OrderLines.FirstOrDefault(old => old.OrderID == orderID && old.ItemSKU == neoSKU).Shipped += received;
        database.SaveChanges();
    }

    public void DeleteReceipt(ReceiptData receipt)
    {
        List<ReceiptLineData> lines = database.ReceiptLines.Where(rld => rld.ReceiptID == receipt.ReceiptID).ToList();

        foreach (ReceiptLineData line in lines)
        {
            OrderData? matchingOrder = database.Orders.FirstOrDefault(o => o.OrderID == receipt.OrderID);
            OrderLineData? matchingOld = database.OrderLines.FirstOrDefault(old => old.OrderID == matchingOrder.OrderID && old.ItemSKU == line.ItemSKU);
            if (matchingOrder != null && matchingOld != null)
            {
                matchingOld.Shipped -= line.NumberReceived;
                if (receipt.Order.Direction == "OUTGOING")
                {
                    database.StockItems.FirstOrDefault(si => si.NeoSKU == line.ItemSKU).Stock += line.NumberReceived;
                }
                else
                {
                    database.StockItems.FirstOrDefault(si => si.NeoSKU == line.ItemSKU).Stock -= line.NumberReceived;
                }   
            }
            database.Remove(line);
            database.SaveChanges();
        }
        database.Remove(receipt);
        database.SaveChanges();
    }

    public void CompleteOrder(OrderDTO dto)
    {
        OrderData? order = database.Orders.FirstOrDefault(o => o.OrderID == dto.ID);
        if (order != null){
            foreach (OrderLineData old in database.OrderLines.Where(old => old.OrderID == order.OrderID))
            {
                if (old.AmountOrdered > old.Shipped)
                {
                    return;
                }
            }
            order.Completed = true;
            database.SaveChanges();
        }
    }
}