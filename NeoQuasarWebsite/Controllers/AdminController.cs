using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite.Controllers;

public class AdminController : Controller
{
    private readonly NeoQuasarDBContext _dbContext;
    private readonly IPasswordHasher<UserData> _passwordHasher;
    private readonly IAccountService _accountService;

    public AdminController(NeoQuasarDBContext dbContext,IPasswordHasher<UserData> passwordHasher, IAccountService accountService){
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _accountService = accountService;
    }

    public IActionResult RoleManagement()
    {
        var roleModel = new RoleManagementModel();
        roleModel.userList = new List<UserModel>();
        foreach (UserData ud in UserModel.GetDatabase())
        {
            roleModel.userList.Add(new UserModel(ud));
        }
        
        return View(roleModel);
    }

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

    public IActionResult EditItem(string id)
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


    // JAVASCRIPT METHODS


    public IActionResult JSChangeUserRole(int userid, string role)
    {
        UserData user = UserModel.GetUser(userid);
        UserRole? userRole = (UserRole) Enum.Parse(typeof(UserRole),role);

        if (user.Role != "ADMIN" && userRole != null){
            user.Role = userRole.ToString();
            return Json(new
            {
                success = true
            });
        }
        else
        {
            return Json(new
            {
                success = false,
                error = "Cannot edit roles of admin. Cannot set role to none."
            });
        }
    }
}