using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite.Controllers;

[Authorize (Roles = "ADMIN")]
public class AdminController : Controller
{
    private readonly NeoQuasarDBContext _dbContext;
    private readonly IPasswordHasher<UserData> _passwordHasher;
    private readonly IAccountService _accountService;
    private readonly IDataService _dataService;
    private readonly IStockService _stockService;
    private readonly IFileService _fileService;

    public AdminController(NeoQuasarDBContext dbContext,IPasswordHasher<UserData> passwordHasher, IAccountService accountService, IDataService dataService, IStockService stockService, IFileService fileService){
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _accountService = accountService;
        _dataService = dataService;
        _stockService = stockService;
        _fileService = fileService;
    }

    public IActionResult RoleManagement()
    {
        return View();
    }

    [HttpGet]
    public IActionResult StockManagement()
    {
        StockManagementViewModel model = new StockManagementViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult StockManagement(CreateStockItemModel stockItem)
    {
        StockManagementViewModel model = new StockManagementViewModel();
        if (!ModelState.IsValid)
        {
            if (stockItem == null)
            {
                TempData["ErrorMessage"] = "No Item Entered.";
                return View(model);
            }
        }
        else
        {
            try
            {
                _stockService.AddStockItem(stockItem.NewStockItem());
                ViewData["SuccessMessage"] = $"{stockItem.Name} added successfully!";
            }
            catch (Exception e)
            {
                model.ManSKU = stockItem.ManSKU;
                model.Name = stockItem.Name;
                model.Manufacturer = stockItem.Manufacturer;
                model.Price = stockItem.Price;
                model.Stock = stockItem.Stock;

                TempData["ErrorMessage"] = e.ToString();
                Console.WriteLine("Error: " + e);
                return View(model);
            }
        }
        ModelState.Clear();
        return View(model);
    }

    [HttpGet]
    public IActionResult Manufacturers()
    {
        ManufacturersViewModel model = new ManufacturersViewModel();
        return View(model);
    }

    [HttpPost]
    public IActionResult Manufacturers(ManufacturersViewModel model)
    {
        model.DimCode = model.DimCode.ToLower();
        
        if (_dataService.GetDimension(model.DimCode) == null)
        {
            ModelState.AddModelError(nameof(model.DimCode),"Invalid DimCode!");
            return View(model);
        }

        if (!ModelState.IsValid)
        {

            if (model == null)
            {
                TempData["ErrorMessage"] = "No Item Entered.";
                return View(model);
            }
        }
        else
        {
            try
            {
                _dataService.AddManufacturer(model.NewManufacturer());
                TempData["SuccessMessage"] = $"{model.Name} added successfully!";
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.ToString();
                Console.WriteLine("Error: " + e);
                return View(model);
            }
        }
        model = new ManufacturersViewModel();
        ModelState.Clear();
        return View(model);
    }

    [HttpGet]
    public IActionResult EditItem(string id)
    {
        StockEditViewModel model = new StockEditViewModel();
        StockItemData item = _dataService.GetStockItem(id);
        model.Item = new StockItemDTO
        {
            NeoSKU = id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            Stock = item.Stock,
            Image = item.ImageLocation,
            ManufacturerName = _dataService.GetManufacturer(item.ManufacturerID).Name,
            ManufacturerSKU = item.ManufacturerSKU,
            Category = item.StockCategory,
            NoRestock = item.DontRestock
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult EditItem(string id, IFormFile? file)
    {
        StockEditViewModel model = new StockEditViewModel();
        StockItemData item = _dataService.GetStockItem(id);
        model.Item = new StockItemDTO
        {
            NeoSKU = id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            Stock = item.Stock,
            Image = item.ImageLocation,
            ManufacturerName = _dataService.GetManufacturer(item.ManufacturerID).Name,
            ManufacturerSKU = item.ManufacturerSKU,
            Category = item.StockCategory,
            NoRestock = item.DontRestock
        };

        if (file != null)
        {
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);

            model.Item.FileContent = memoryStream.ToArray();
            model.Item.FileName = file.FileName;
            model.Item.FileType = file.ContentType;
            model.Item.FileSize = memoryStream.Length;

            model.Item.Image = file.FileName;

            _fileService.AddFileContent(model.Item);
        } 

        _dataService.UpdateItem(model.Item);

        

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

    public IActionResult JSUpdateItem([FromBody] StockItemDTO item)
    {
        if (item == null)
        {
            return BadRequest("invalid data.");
        }
        else
        {
            if (_dataService.UpdateItem(item))
            {
                return Json(new
                {
                    success = true,
                    error = ""
                });
            }
            else
            {
                return Json(new {
                success = false,
                error = "Error updating item."});
            }
        }
    }

    public IActionResult JSFilteredResults(string? id)
    {
        if (id == null)
        {
            List<StockItemData> unfilteredList = _dataService.GetStockItemList();
            List<StockItemDTO> returnList = new List<StockItemDTO>();
            foreach (StockItemData sid in unfilteredList)
            {
                StockItemDTO si = new StockItemDTO
                {
                    NeoSKU = sid.NeoSKU,
                    ManufacturerSKU = sid.ManufacturerSKU,
                    Name = sid.Name,
                    ManufacturerName = _dataService.GetManufacturer(sid.ManufacturerID).Name,
                    Price = sid.Price,
                    Category = sid.StockCategory,
                    Stock = sid.Stock

                };
                returnList.Add(si);
            }
            return Json(new{
                success = true,
                error = "",
                list = JsonSerializer.Serialize(returnList)
                });
        }
        else {
            List<StockItemData> filteredList = _dataService.GetStockItemList(id);
            List<StockItemDTO> returnList = new List<StockItemDTO>();
            foreach (StockItemData sid in filteredList)
            {
                StockItemDTO si = new StockItemDTO
                {
                    NeoSKU = sid.NeoSKU,
                    ManufacturerSKU = sid.ManufacturerSKU,
                    Name = sid.Name,
                    ManufacturerName = _dataService.GetManufacturer(sid.ManufacturerID).Name,
                    Price = sid.Price,
                    Category = sid.StockCategory,
                    Stock = sid.Stock

                };
                returnList.Add(si);
            }
            return Json(new{
                success = true,
                error = "",
                list = JsonSerializer.Serialize(returnList)
                });
        }
    }
    public IActionResult JSFilteredUsers(string? id)
    {
        if (id == null)
        {
            List<UserData> unfilteredList = _dataService.GetUserList();
            List<object> returnList = new List<object>();
            foreach (UserData ud in unfilteredList)
            {
                object user = new
                {
                    UserID = ud.UserID,
                    Name = ud.Name,
                    Username = ud.Username,
                    Contact = ud.ContactAddress(),
                    Role = ud.Role,

                };
                returnList.Add(user);
            }
            return Json(new{
                success = true,
                error = "",
                list = JsonSerializer.Serialize(returnList)
                });
        }
        else {
            List<UserData> filteredList = _dataService.GetUserList(id);
            List<object> returnList = new List<object>();
            foreach (UserData ud in filteredList)
            {
                object user = new
                {
                    UserID = ud.UserID,
                    Name = ud.Name,
                    Username = ud.Username,
                    Contact = ud.ContactAddress(),
                    Role = ud.Role,

                };
                returnList.Add(user);
            }
            return Json(new{
                success = true,
                error = "",
                list = JsonSerializer.Serialize(returnList)
                });
        }
    }

    public IActionResult JSFilteredManufacturers(string? id)
    {
        if (id == null)
        {
            List<ManufacturerData> unfilteredList = _dataService.GetManufacturerList();
            List<object> returnList = new List<object>();
            foreach (ManufacturerData md in unfilteredList)
            {
                object manufacturer = new
                {
                    ManufacturerID = md.ManufacturerID,
                    Name = md.Name,
                    Abbreviation = md.Abbreviation,
                    ContactAddress = md.ContactAddress,
                    DimCode = md.DimCode
                };
                returnList.Add(manufacturer);
            }
            return Json(new{
                success = true,
                error = "",
                list = JsonSerializer.Serialize(returnList)
                });
        }
        else {
            List<ManufacturerData> filteredList = _dataService.GetManufacturerList(id);
            List<object> returnList = new List<object>();
            foreach (ManufacturerData md in filteredList)
            {
                object manufacturer = new
                {
                    ManufacturerID = md.ManufacturerID,
                    Name = md.Name,
                    Abbreviation = md.Abbreviation,
                    ContactAddress = md.ContactAddress,
                    DimCode = md.DimCode
                };
                returnList.Add(manufacturer);
            }
            return Json(new{
                success = true,
                error = "",
                list = JsonSerializer.Serialize(returnList)
                });
        }
    }

}