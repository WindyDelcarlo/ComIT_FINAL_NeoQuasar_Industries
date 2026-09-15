using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Controllers;

public class StockController : Controller
{
    [HttpGet]
    public IActionResult StockManagement()
    {
        StockManagementModel model = new StockManagementModel();
        model.stockItems = StockItem.GetDatabase();
        return View(model);
    }

    [HttpPost]
    public IActionResult StockManagement(StockItemModel stockItem)
    {
        StockManagementModel model = new StockManagementModel();
        model.stockItems = StockItem.GetDatabase();
        model.newStockItem = stockItem;

        if (stockItem == null)
        {
            ViewData["ErrorMessage"] = "No Item Entered.";
            return View(model);
        }
        try {
            StockItem.DatabaseAdd(stockItem.NewStockItem());
            ViewData["SuccessMessage"] = $"{stockItem.Name} added successfully!";
        }
        catch (Exception e)
        {
            ViewData["ErrorMessage"] = e.ToString();
            Console.WriteLine("Error: " + e);
            return View(model);
        }
        return View(model);
    }
}