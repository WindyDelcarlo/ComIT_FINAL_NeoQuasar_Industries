using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite.Controllers;

public class HomeController : Controller
{

    private readonly NeoQuasarDBContext _dbContext;
    private readonly IAccountService _accountService;
    private readonly ISearchService _searchService;
    private readonly IDataService _dataService;
    private readonly IOrderService _orderService;

    public HomeController(NeoQuasarDBContext dbContext, IAccountService accountService, ISearchService searchService, IDataService dataService, IOrderService orderService)
    {
        _dbContext = dbContext;
        _accountService = accountService;
        _searchService = searchService;
        _dataService = dataService;
        _orderService = orderService;
    }
    
    public IActionResult Storefront(string? search)
    {
        int userID;
        if (User.Identity.IsAuthenticated)
        {
            userID = _accountService.GetCurrentUserID(User);
            // user = new UserModel(UserModel.GetUser(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))));
        }
        else
        {
            userID = 0;
        }
        UserDTO dto = new UserDTO
        {
            UserID = userID,
            Cart = _dataService.GetUserCart(userID)  
        };
        StorefrontModel model = new StorefrontModel();
        model.User = dto;
        List<string> searchTerms;
        int searchResultCount;
        if (search != null){
            model.SearchResults = _searchService.StockSearch(search,out searchTerms,out searchResultCount);
            model.SearchTerms = searchTerms;
            model.SearchResultCount = searchResultCount;
        }
        else
        {
            model.SearchResults = _searchService.NoFilterSearch();
        }
        return View(model);
    }
    
    public IActionResult Cart()
    {
        CartModel model = new CartModel();
        if (User.Identity.IsAuthenticated){
            int userID = _accountService.GetCurrentUserID(User);
            model.user = new UserDTO
            {
                UserID = userID,
                Cart = _dataService.GetUserCart(userID),
            };
        }
        else
        {
            model.user = new UserDTO
            {
                UserID = 0,
                Cart = new List<CartLineData>()
            };
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult CheckOut()
    {
        CheckOutModel model = new CheckOutModel();
        if (User.Identity.IsAuthenticated){
            int userID = _accountService.GetCurrentUserID(User);
            UserData user = _dataService.GetUser(userID);
            model.user = new UserDTO
            {
                UserID = userID,
                Cart = _dataService.GetUserCart(userID)
            };
            model.AddressName = user.Name;
            model.AddressStreet = user.Address;
            PaymentDTO? activeCard = null;
            if (_dataService.GetUserPayments(userID).Count() > 0)
            {
                activeCard = new PaymentDTO {
                    CardNumber = _dataService.GetUserPaymentDetail(userID).CardNumber
                };
                model.CardNumber = activeCard != null ? activeCard.CardNumber : "No Card Found";
                model.ContactAddress = user.ContactAddress();
                model.LocatorCode = user.LocatorCode;
            }
        }
        else
        {
            model.user = new UserDTO
            {
                UserID = 0,
                Cart = new List<CartLineData>()
            };
        }
        if (model.user.Cart.Count() > 0){
           return View(model);
        }
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public IActionResult CheckOut(CheckOutModel model)
    {
        
        if (User.Identity.IsAuthenticated){
            int userID = _accountService.GetCurrentUserID(User);
            model.user = new UserDTO
            {
                UserID = userID,
                Cart = _dataService.GetUserCart(userID),
                Name = _dataService.GetUser(userID).Name
            };
        }
        else
        {
            model.user = new UserDTO
            {
                UserID = 0,
                Cart = new List<CartLineData>()
            };
        }
        if (ModelState.IsValid){
            if (User.Identity.IsAuthenticated){
                string orderID = _orderService.CreateOrder(model.user.UserID,"OUTGOING",model.ShippingOption.ToString(),model.user.Name);
                foreach (CartLineData cld in model.user.Cart){
                    _orderService.AddLine(orderID,cld);
                    _accountService.RemoveFromCart(cld.ItemSKU,model.user.UserID,cld.Quantity);
                }
            }
            else
            {
                string sNPattern = @"^[a-z0-9.]{4}\/[a-z0-9]*\.[a-z0-9]*$";
                string stellarNet = Regex.Match(model.ContactAddress,sNPattern) != null ? model.ContactAddress : string.Empty;
                string slingName = Regex.Match(model.ContactAddress,sNPattern) != null ? string.Empty : model.ContactAddress;
                
                Console.WriteLine("No Auth");
            }
            Console.WriteLine("Order created successfully!");
            TempData["SuccessMessage"] = "Order created successfully!";
            return LocalRedirect("~/Home/Storefront");
        }
        else {
            
            return View(model);
        }
    }  

    public IActionResult Item(string id)
    {
        StockModel model = new StockModel();
        StockItemData sid = _dataService.GetStockItem(id);
        model.Item = new StockItemDTO
        {
                    Name = sid.Name,
                    NeoSKU = sid.NeoSKU,
                    ManufacturerSKU = sid.ManufacturerSKU,
                    ManufacturerName = _dataService.GetManufacturer(sid.ManufacturerID).Name,
                    Price = sid.Price,
                    Stock = sid.Stock,
                    Category = sid.StockCategory,
                    Image = sid.ImageLocation,
                    Description = sid.Description,
                    NoRestock = sid.DontRestock
        };
        
        if (User.Identity.IsAuthenticated){
            int userID = _accountService.GetCurrentUserID(User);
            model.User = new UserDTO{
                UserID = userID,
                Cart = _dataService.GetUserCart(userID)
            };
        }
        else
        {
            model.User = new UserDTO{
                UserID = 0,
                Cart = new List<CartLineData>()
            };
        }

        return View(model);
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // JAVASCRIPT FUNCTIONS

    public IActionResult JSGetCart()
    {
        if (User.Identity.IsAuthenticated)
        {
            int userID = _accountService.GetCurrentUserID(User);
            List<object> cartList = new List<object>();
            foreach (CartLineData cld in _dataService.GetUserCart(userID))
            {
                StockItemData item = _dataService.GetStockItem(cld.ItemSKU);
                cartList.Add(new
                {
                    SKU = cld.ItemSKU,
                    ManufacturerSKU = item.ManufacturerSKU,
                    Image = item.ImageLocation,
                    Quantity = cld.Quantity,
                    Name = item.Name,
                    Price = item.Price, 
                    Stock = item.Stock
                });
            }

            return Json(new
            {
                success = true,
                error = "",
                items = cartList
            });
        }
        else {
            return Json(new
            {
               success = false,
               error = "User not found.",
               items = ""
            });
        }
    }

    public IActionResult JSAddToCart(string id)
    {
        int userID;
        if (User.Identity.IsAuthenticated){
            userID = _accountService.GetCurrentUserID(User);
        }
        else
        {
            userID = 0;
        }

        _accountService.AddToCart(id,userID,1);
        CartLineData? cld = _dataService.GetCartLine(id,userID);
        if (cld != null){
        return Json(new { 
            success = true,
            count = cld.Quantity });
        }
        else
        {
            return Json(new
            {
                success = false,
                error = "Line Not Found"
            });
        }
    }

    public IActionResult JSRemoveFromCart(string id)
    {
        int userID;
        if (User.Identity.IsAuthenticated){
            userID = _accountService.GetCurrentUserID(User);
        }
        else
        {
            userID = 0;
        }

        bool removed = _accountService.RemoveFromCart(id,userID,1);
        CartLineData? cld = _dataService.GetCartLine(id,userID);
        if (cld != null && removed){
        return Json(new { 
            success = true,
            count = cld.Quantity });
        }
        else if (removed)
        {
            return Json(new {
                success = true,
                count = 0
            });
        }
        else
        {
            return Json(new
            {
                success = false,
                error = "Line Not Found"
            });
        }
    }

    public IActionResult JSCheckTax()
    {
        if (User.Identity.IsAuthenticated){
            int userID = _accountService.GetCurrentUserID(User);
            double taxPercent = 0.1;
            // Some Sort of LookUp Text Here
            return Json(new
            {
            success = true,
            error = "",
            tax = taxPercent
            });
        }
        return Json (new
        {
            success = false,
            error = "User not found",
            tax = 0
        });
    }
}
