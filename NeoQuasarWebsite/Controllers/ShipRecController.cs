using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite.Controllers;

[Authorize (Roles = "SHIPPER, ADMIN")]
public class ShipRecController : Controller
{
    private readonly NeoQuasarDBContext _dbContext;
    private readonly IPasswordHasher<UserData> _passwordHasher;
    private readonly IAccountService _accountService;
    private readonly IDataService _dataService;
    private readonly IOrderService _orderService;

    public ShipRecController(NeoQuasarDBContext dbContext,IPasswordHasher<UserData> passwordHasher, IAccountService accountService, IDataService dataService, IOrderService orderService){
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _accountService = accountService;
        _dataService = dataService;
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult PendingOrders(string? id)
    {
        PendingOrdersViewModel pom = new PendingOrdersViewModel();
        pom.orderList = new List<OrderModel>();
        UserData user = _accountService.GetCurrentUser(User);
        foreach (OrderData od in _dataService.GetOrderList(true))
        {
            UserData orderUser = _dataService.GetUser(user.UserID);
            pom.orderList.Add(new OrderModel(od,user));
        }
        if (pom.orderList.FirstOrDefault(o => o.ID == id) != null)
        {
            pom.OrderID = pom.orderList.FirstOrDefault(o => o.ID == id).ID;
        }
        else
        {
        pom.OrderID = pom.orderList[0].ID;
        }
        pom.Received = new int[_dataService.GetOrderLines(pom.OrderID).Count()];
        return View(pom);
    }

    [HttpPost]
    public IActionResult PendingOrders([FromBody] OrderDTO order)
    {
        if (order == null)
        {
            return BadRequest("invalid Data.");
        }
        else
        {
            _orderService.CreateReceipt(order);
            foreach (LineDTO line in order.lines)
            {
                _orderService.AddReceiptLine(order.ID,line.NeoSKU,line.Received,order.TrackingNumber);
            }
            _orderService.CompleteOrder(order);

            return Json(new
            {
               success = true,
               error = "",
               redirectURL = "/ShipRec/PendingOrders/" + order.ID 
            });
        }
    }
    
    public IActionResult ReceiptHistory(int? id, string? orderID)
    {
        ReceiptHistoryViewModel model = new ReceiptHistoryViewModel();
        if (_dataService.GetReceiptList().Count() < 1)
        {
            return RedirectToAction("Storefront","Home");
        }
        List<ReceiptData> list = _dataService.GetReceiptList();
        int receiptID = _dataService.GetReceipt(id ?? 0) != null ? (int) id : list[0].ReceiptID; 
        if (orderID != null)
        {
            model.OrderID = orderID;
        }
        
        ReceiptData receipt = _dataService.GetReceipt(receiptID);
        List<LineDTO> receiptLines = new List<LineDTO>();
        model.Receipts = new List<ReceiptDTO>();
        foreach (ReceiptData rd in list)
        {
            model.Receipts.Add(new ReceiptDTO
            {
                ReceiptID = rd.ReceiptID,
                Shipper = _dataService.GetOrder(rd.OrderID).Shipper,
                Recipient = _dataService.GetOrder(rd.OrderID).Recipient,
                OrderID = rd.OrderID,
                ContactAddress = rd.ContactAddress
            });
        }
        foreach (ReceiptLineData rld in _dataService.GetReceiptLines(receiptID))
        {
            receiptLines.Add(new LineDTO
            {
               Received = rld.NumberReceived,
               Ordered = _dataService.GetOrderLine(rld.ReceiptLineID).AmountOrdered,
               NeoSKU = rld.ItemSKU,
               Description = _dataService.GetStockItem(rld.ItemSKU).Name 
            });
            
        }
        model.Receipt = new ReceiptDTO
        {
            ReceiptID = receiptID,
            OrderID = receipt.OrderID,
            Date = receipt.Date,
            Shipper = _dataService.GetOrder(receipt.OrderID).Shipper,
            Recipient = _dataService.GetOrder(receipt.OrderID).Recipient,
            ContactAddress = receipt.ContactAddress,
            Courier = receipt.Courier,
            TrackingNumber = receipt.WaybillNumber,
            Lines = receiptLines
        };

        return View(model);
    }

    // JAVASCRIPT FUNCTIONS
    public IActionResult JSReceiptDelete(int id)
    {
        ReceiptData receipt = _dataService.GetReceipt(id);
        if (receipt != null){
            _orderService.DeleteReceipt(receipt);
            
            return Json (new
            {
                success = true,
                error = ""
            });
        }
        return Json (new
        {
            success = false,
            error = "Receipt Not Found"
        });

    }

    public IActionResult JSGetOrder(string id)
    {
        OrderData order = _dataService.GetOrder(id);
        if (order != null){
        List<OrderLineData> orderLines = _dataService.GetOrderLines(id);
        List<object> lineObjects = new List<object>();

        foreach(OrderLineData old in orderLines)
        {
            lineObjects.Add(new
            {
                received = 0,
                outstanding = old.AmountOrdered - old.Shipped,
                ordered = old.AmountOrdered,
                neoSKU = old.ItemSKU,
                description = _dataService.GetStockItem(old.ItemSKU).Name,
            });
        }

        string shipper = !string.IsNullOrWhiteSpace(order.Shipper) ? order.Shipper : string.Empty;
        string recipient = !string.IsNullOrWhiteSpace(order.Recipient) ? order.Recipient : string.Empty;

        object orderItem = new
        {
            shippingDirection = order.Direction,
            userID = order.UserID,
            recipient = recipient,
            shipper = shipper,
            id = order.OrderID,
            date = order.Date,
            contactAddress = order.ContactAddress,
            shipping = order.ShippingOption,
            shippingAddress = order.ShippingAddress,
            locatorCode = order.LocatorCode,
            courier = string.Empty,
            trackingNumber = string.Empty,
            lines = lineObjects
        };

        return Json (new
        {
            success = true,
            error = "",
            order = orderItem
        });
        }
        else
        {
            return Json (new
            {
                success = false,
                error = "Order not found.",
                order = new {
                    shippingDirection = "outgoing",
                    userID = 0,
                    recipient = string.Empty,
                    shipper = string.Empty,
                    id = string.Empty,
                    date = string.Empty,
                    contactAddress = string.Empty,
                    shipping = string.Empty,
                    shippingAddress = string.Empty,
                    locatorCode = string.Empty,
                    courier = string.Empty,
                    trackingNumber = string.Empty,
                    lines = new List<object>()
            }
            });
        }
    }
    
    public IActionResult JSFilteredReceipts(string? id)
    {
        if (id == null)
        {
            List<ReceiptData> unfilteredList = _dataService.GetReceiptList();
            List<object> returnList = new List<object>();
            foreach (ReceiptData rd in unfilteredList)
            {
                string? shipper = _dataService.GetOrder(rd.OrderID).Shipper;
                string? recipient = _dataService.GetOrder(rd.OrderID).Recipient;
                object receipt = new
                {
                    ReceiptID = rd.ReceiptID,
                    ShipRec = !string.IsNullOrWhiteSpace(shipper) ? shipper : recipient,
                    OrderID = rd.OrderID
                };
                returnList.Add(receipt);
            }
            return Json(new{
                success = true,
                error = "",
                list = JsonSerializer.Serialize(returnList)
                });
        }
        else {
            List<ReceiptData> filteredList = _dataService.GetReceiptList(id);
            List<object> returnList = new List<object>();
            foreach (ReceiptData rd in filteredList)
            {
                string shipper = _dataService.GetOrder(rd.OrderID).Shipper;
                string recipient = _dataService.GetOrder(rd.OrderID).Recipient;
                object receipt = new
                {
                    ReceiptID = rd.ReceiptID,
                    ShipRec = !string.IsNullOrWhiteSpace(shipper) ? shipper : recipient,
                    OrderID = rd.OrderID
                };
                returnList.Add(receipt);
            }
            return Json(new{
                success = true,
                error = "",
                list = JsonSerializer.Serialize(returnList)
                });
        }
    }
}