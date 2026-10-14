using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite.Controllers;

public class AdminController : Controller
{
    private readonly NeoQuasarDBContext _dbContext;
    private readonly IPasswordHasher<UserData> _passwordHasher;
    private readonly IAccountService _accountService;
    private readonly IDataService _dataService;
    private readonly IStockService _stockService;

    public AdminController(NeoQuasarDBContext dbContext,IPasswordHasher<UserData> passwordHasher, IAccountService accountService, IDataService dataService, IStockService stockService){
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _accountService = accountService;
        _dataService = dataService;
        _stockService = stockService;
    }

    public IActionResult RoleManagement()
    {
        var roleModel = new RoleManagementModel();
        roleModel.userList = new List<UserModel>();
        foreach (UserData ud in _dataService.GetUserList())
        {
            roleModel.userList.Add(new UserModel(ud));
        }
        
        return View(roleModel);
    }

        [HttpGet]
    public IActionResult StockManagement()
    {
        StockManagementModel model = new StockManagementModel();
        foreach (StockItemData sid in _dataService.GetStockItemList())
            {
                model.stockItems.Add(new StockItemDTO
                {
                    NeoSKU = sid.NeoSKU,
                    Name = sid.Name,
                    ManufacturerSKU = sid.ManufacturerSKU,
                    ManufacturerName = _dataService.GetManufacturer(sid.ManufacturerID).Name,
                    Price = sid.Price,
                    Stock = sid.Stock,
                    Category = sid.StockCategory
                });
            }
        return View(model);
    }

    [HttpPost]
    public IActionResult StockManagement(StockItemModel stockItem)
    {
        StockManagementModel model = new StockManagementModel();
        foreach (StockItemData sid in _dataService.GetStockItemList())
            {
                model.stockItems.Add(new StockItemDTO
                {
                    Name = sid.Name,
                    ManufacturerSKU = sid.ManufacturerSKU,
                    ManufacturerName = _dataService.GetManufacturer(sid.ManufacturerID).Name,
                    Price = sid.Price,
                    Stock = sid.Stock,
                    Category = sid.StockCategory
                });
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
                _stockService.AddStockItem(stockItem.NewStockItem());
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

    public IActionResult EditItem(string id)
    {
        StockEditModel model = new StockEditModel();
        model.NeoSKU = id;
        StockItem referenceItem = new StockItem(_dataService.GetStockItem(id));
        model.StockItem = _dataService.GetStockItem(id);
        model.Name = referenceItem.Name;
        model.Description = referenceItem.Description;
        model.Price = referenceItem.Price;
        model.Stock = referenceItem.Stock;
        model.ImageLocation = referenceItem.ImageLocation;

        return View(model);
    }


    // JAVASCRIPT METHODS

    public IActionResult JSChangeUserRole(int userid, string role)
    {
        UserData user = _dataService.GetUser(userid);
        UserRole? userRole = (UserRole) Enum.Parse(typeof(UserRole),role);

        if (userRole != null){
            if (_accountService.ChangeRole(userid,role))
            {
                return Json(new
                {
                    success = true,
                    error = ""
                });
            }
        }
        return Json(new
        {
            success = false,
            error = "Cannot edit roles of admin. Cannot set role to none."
        });
    }
}