using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite.Controllers;

public class ShipRecController : Controller
{
    private readonly NeoQuasarDBContext _dbContext;
    private readonly IPasswordHasher<UserData> _passwordHasher;
    private readonly IAccountService _accountService;

    public ShipRecController(NeoQuasarDBContext dbContext,IPasswordHasher<UserData> passwordHasher, IAccountService accountService){
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult PendingOrders(string? id)
    {
        PendingOrdersModel pom = new PendingOrdersModel();
        pom.orderList = new List<Order>();
        foreach (OrderData od in Order.GetDatabase())
        {
            if (!od.Completed) {
                pom.orderList.Add(new Order(od));
            }
        }
        if (pom.orderList.FirstOrDefault(o => o.ID == id) != null)
        {
            pom.OrderID = pom.orderList.FirstOrDefault(o => o.ID == id).ID;
        }
        else
        {
        pom.OrderID = pom.orderList[0].ID;
        }
        pom.Received = new int[LineItem.OrderLineDatabase().Where(ol => ol.OrderID == pom.OrderID).Count()];
        return View(pom);
    }

    [HttpPost]
    public IActionResult PendingOrders(PendingOrdersModel pom)
    {        
        pom.orderList = new List<Order>();
        foreach (OrderData od in Order.GetDatabase())
        {
            if (!od.Completed) {
                pom.orderList.Add(new Order(od));
            }
        }

        if (ModelState.IsValid)
        {
            Receipt receipt = new Receipt(0,pom.OrderID,Program.Date(),pom.Courier,pom.TrackingNumber);
            Receipt.DatabaseAdd(receipt,pom.Received);
            return RedirectToAction("PendingOrders", new {id = pom.OrderID});
        }
        else
        {
            return View(pom);
        }
    }
    
    public IActionResult ReceiptHistory(int? id)
    {
        ReceiptHistoryModel model = new ReceiptHistoryModel();
        if (Receipt.GetDatabase().Count() < 1)
        {
            return RedirectToAction("Storefront","Home");
        }
        int? receiptID = Receipt.GetDatabase().FirstOrDefault(r => r.ReceiptID == id) != null ? id : Receipt.GetDatabase().ToList()[0].ReceiptID; 
        if (receiptID == null)
        {
            return RedirectToAction("Storefront","Home");
        }
        model.ReceiptID = (int) receiptID; 

        return View(model);
    }

    // JAVASCRIPT FUNCTIONS
    public IActionResult JSReceiptDelete(int id)
    {
        ReceiptData receipt = _dbContext.Receipts.FirstOrDefault(r => r.ReceiptID == id);
        if (receipt != null){
            List<ReceiptLineData> lines = _dbContext.ReceiptLines.Where(rld => rld.ReceiptID == id).ToList();

            foreach(ReceiptLineData line in lines)
            {
                OrderData? matchingOrder = _dbContext.Orders.FirstOrDefault(o => o.OrderID == receipt.OrderID);
                OrderLineData? matchingOld = _dbContext.OrderLines.FirstOrDefault(old => old.OrderID == receipt.OrderID && old.ItemSKU == line.ItemSKU);
                if (matchingOrder != null && matchingOld != null)
                {
                    matchingOld.Shipped -= line.NumberReceived;
                    if (receipt.Order.Direction == "OUTGOING"){
                        StockItem.GetDatabase().FirstOrDefault(si => si.NeoSKU == line.ItemSKU).Stock += line.NumberReceived;
                    }
                    else
                    {
                        StockItem.GetDatabase().FirstOrDefault(si => si.NeoSKU == line.ItemSKU).Stock -= line.NumberReceived;
                    }
                }
                _dbContext.Remove(line);
                _dbContext.SaveChanges();
            }
            _dbContext.Remove(receipt);
            _dbContext.SaveChanges();
            
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

    public IActionResult SelectOrder(string id)
    {
        return RedirectToAction("PendingOrders", new { id = id });
    }
}