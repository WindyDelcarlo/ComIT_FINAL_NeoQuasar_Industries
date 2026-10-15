using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Services;

public interface IDataService
{
    public UserData GetUser(int id);
    public List<UserData> GetUserList();
    public List<CartLineData> GetUserCart(int id);
    public List<PaymentDetailData> GetUserPayments(int id);
    public List<OrderData> GetUserOrders(int id);
    public PaymentDetailData GetUserPaymentDetail (int userID);
    public string GetUserContact(int id);
    public StockItemData GetStockItem(string neoSKU);
    public List<StockItemData> GetStockItemList();
    public CartLineData? GetCartLine(string neoSKU, int userID);
    public ManufacturerData GetManufacturer(int id);
    public OrderData GetOrder(string orderID);
    public OrderData GetOrder(int receiptID);
    public List<OrderData> GetOrderList(bool outstanding);
    public List<OrderLineData> GetOrderLines(string orderID);
    public OrderLineData GetOrderLine(int receiptLineID);
    public ReceiptData GetReceipt(int receiptID);
    public List<ReceiptData> GetReceiptList(string? orderID);
    public List<ReceiptLineData> GetReceiptLines(int receiptID);
    public PaymentDetailData GetPaymentDetail (int paymentID);
}

public class DataService : IDataService

{
    private NeoQuasarDBContext database;

    public DataService(NeoQuasarDBContext dBContext)
    {
        database = dBContext;
    }

    // Database Gets
    public UserData GetUser (int id)
    {
        return database.Users.FirstOrDefault(u => u.UserID == id);
    }

    public List<UserData> GetUserList()
    {
        return database.Users.ToList();
    }

    public List<CartLineData> GetUserCart(int id)
    {
        return database.CartLines.Where(ci => ci.UserID == id).ToList();
    }

    public List<PaymentDetailData> GetUserPayments(int userID)
    {
        return database.PaymentDetails.Where(pd => pd.UserID == userID).ToList();
    }

    public List<OrderData> GetUserOrders(int id)
    {
        return database.Orders.Where(o => o.UserID == id).ToList();
    }

    public string GetUserContact(int userID)
    {
        return GetUser(userID).StellarNet != null ? GetUser(userID).StellarNet : GetUser(userID).SlingName;
    }

    public StockItemData GetStockItem(string neoSKU)
    {
        return database.StockItems.FirstOrDefault(sid => sid.NeoSKU == neoSKU);
    }
    
    public List<StockItemData> GetStockItemList()
    {
        return database.StockItems.ToList();
    }

    public CartLineData? GetCartLine(string neoSKU, int userID)
    {
        return GetUserCart(userID).FirstOrDefault(cld => cld.ItemSKU == neoSKU);
    }

    public ManufacturerData GetManufacturer (int id)
    {
        return database.Manufacturers.FirstOrDefault(m => m.ManufacturerID == id);
    }

    public PaymentDetailData GetUserPaymentDetail (int userID)
    {
        int selected = GetUser(userID).SelectedCard;
        return database.PaymentDetails.FirstOrDefault(pd => pd.PaymentDetailID == selected);
    }

    public OrderData GetOrder(string orderID)
    {
        return database.Orders.FirstOrDefault(o => o.OrderID == orderID);
    }

    public OrderData GetOrder(int receiptID)
    {
        ReceiptData receipt = GetReceipt(receiptID);
        return database.Orders.FirstOrDefault(o => o.OrderID == receipt.OrderID);
    }

    public List<OrderData> GetOrderList(bool outstanding)
    {
        if (outstanding)
        {
            return database.Orders.Where(o => !o.Completed).ToList();
        }
        else
        {
            return database.Orders.ToList();
        }
    }

    public List<OrderLineData> GetOrderLines(string orderID)
    {
        return database.OrderLines.Where(ol => ol.OrderID == orderID).ToList();
    }

    public OrderLineData GetOrderLine(int receiptLineID)
    {
        ReceiptLineData rld = database.ReceiptLines.FirstOrDefault(rld => rld.ReceiptLineID == receiptLineID);
        ReceiptData receipt = database.Receipts.FirstOrDefault(r => r.ReceiptID == rld.ReceiptID);
        return database.OrderLines.FirstOrDefault(old => old.OrderID == receipt.OrderID && old.ItemSKU == rld.ItemSKU);
    }

    public ReceiptData GetReceipt(int receiptID)
    {
        return database.Receipts.FirstOrDefault(r => r.ReceiptID == receiptID);
    }

    public List<ReceiptData> GetReceiptList(string? orderID)
    {
        if (!string.IsNullOrWhiteSpace(orderID))
        {
            return database.Receipts.Where(r => r.OrderID == orderID).ToList();
        }
        else
        {
            return database.Receipts.ToList();
        }
    }

    public List<ReceiptLineData> GetReceiptLines(int receiptID)
    {
        return database.ReceiptLines.Where(rl => rl.ReceiptID == receiptID).ToList();
    }

    public PaymentDetailData GetPaymentDetail(int paymentID)
    {
        return database.PaymentDetails.FirstOrDefault(pd => pd.PaymentDetailID == paymentID);
    }
}