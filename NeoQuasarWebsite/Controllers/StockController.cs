using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Controllers;

public class StockController : Controller
{
    [HttpGet]
    public IActionResult StockManagement()
    {
        StockManagementModel model = new StockManagementModel();
        foreach (StockItemData sid in StockItem.GetDatabase())
            {
                model.stockItems.Add(new StockItem(sid));
            }
        return View(model);
    }

    [HttpPost]
    public IActionResult StockManagement(StockItemModel stockItem)
    {
        StockManagementModel model = new StockManagementModel();
        foreach (StockItemData sid in StockItem.GetDatabase())
            {
                model.stockItems.Add(new StockItem(sid));
            }
        if (!ModelState.IsValid)
        {

            if (stockItem == null)
            {
                ViewData["ErrorMessage"] = "No Item Entered.";
                return View(model);
            }
        }
        else
        {
            model.ManSKU = stockItem.ManSKU;
            model.Name = stockItem.Name;
            model.Manufacturer = stockItem.Manufacturer;
            model.Price = stockItem.Price;
            model.Stock = stockItem.Stock;
            try
            {
                StockItem.DatabaseAdd(stockItem.NewStockItem());
                ViewData["SuccessMessage"] = $"{stockItem.Name} added successfully!";
            }
            catch (Exception e)
            {
                ViewData["ErrorMessage"] = e.ToString();
                Console.WriteLine("Error: " + e);
                return View(model);
            }
        }
        return View(model);
    }

    public IActionResult Item(string id)
    {
        StockModel model = new StockModel();
        model.Item = new StockItem(StockItem.GetDatabase().FirstOrDefault(si => si.NeoSKU == id));
        
        if (User.Identity.IsAuthenticated){
            model.User = new UserModel(UserModel.GetDatabase().FirstOrDefault(u => u.UserID == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0")));
        }
        else
        {
            model.User = new UserModel();
        }

        return View(model);
    }

    public IActionResult Edit(string id)
    {
        StockEditModel model = new StockEditModel();
        model.NeoSKU = id;
        StockItem referenceItem = new StockItem(StockItem.GetDatabase().FirstOrDefault(si => si.NeoSKU == id));
        model.Name = referenceItem.Name;
        model.Description = referenceItem.Description;
        model.Price = referenceItem.Price;
        model.Stock = referenceItem.Stock;
        model.ImageLocation = referenceItem.ImageLocation;

        return View(model);
    }

        public IActionResult ItemEdit(StockEditModel model, string id)
        {


        return View(model);
        }
}