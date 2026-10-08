using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite.Controllers;

public class HomeController : Controller
{

    private readonly NeoQuasarDBContext _dbContext;
    private readonly IAccountService _accountService;

    public HomeController(NeoQuasarDBContext dbContext, IAccountService accountService){
        _dbContext = dbContext;
        _accountService = accountService;
    }
    
    public IActionResult Storefront(string? search)
    {
        UserModel user;
        if (User.Identity.IsAuthenticated)
        {
            user = new UserModel(UserModel.GetUser(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))));
        }
        else
        {
            user = new UserModel();
        }
        SearchModel model = new SearchModel();
        model.User = user;
        List<string> searchTerms;
        int searchResultCount;
        if (search != null){
            model.SearchResults = SearchHandler.StockSearch(search,out searchTerms,out searchResultCount);
            model.SearchTerms = searchTerms;
            model.SearchResultCount = searchResultCount;
        }
        else
        {
            var stockDatabase = new List<StockItem>();
            foreach (StockItemData sid in StockItem.GetDatabase())
            {
                stockDatabase.Add(new StockItem(sid));
            }
            model.SearchResults = stockDatabase;
        }
        return View(model);
    }
    
    [HttpGet]
    public IActionResult Cart(int id)
    {
        CartModel model = new CartModel();
        if (User.Identity.IsAuthenticated){
            model.user = new UserModel(UserModel.GetDatabase().FirstOrDefault(u => u.UserID == id));
        }
        else
        {
            model.user = new UserModel();
        }

        return View(model);
    }

    [HttpPost] IActionResult Cart()
    {
        return RedirectToAction("CheckOut");
    }

    [HttpGet]
    public IActionResult CheckOut()
    {
        CheckOutModel model = new CheckOutModel();
        if (User.Identity.IsAuthenticated){
            model.user = new UserModel(UserModel.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            UserData userData = model.user.DatabaseThis(); 
            model.AddressName = model.user.Name;
            model.AddressStreet = model.user.Address;
            PaymentDetails? activeCard = null;
            if (PaymentDetails.GetDatabase().Where(pd => pd.UserId == userData.UserID).Count() > 0)
            {
                activeCard = new PaymentDetails(PaymentDetails.GetDatabase().FirstOrDefault(pd => pd.PaymentDetailID == userData.SelectedCard));
            }
            model.CardNumber = activeCard != null ? activeCard.CardNumber : "No Card Found";
            model.ContactAddress = model.user.ContactAddress();
            model.LocatorCode = model.user.LocatorCode;
        }
        else
        {
            model.user = new UserModel();
        }
        if (CartItem.GetDatabase().Where(ci => ci.UserID == model.user.ID).Count() > 0){
           return View(model);
        }
        else {
            return RedirectToAction("Cart",new {id = model.user.ID});
        }
    }

    [HttpPost]
    public IActionResult CheckOut(CheckOutModel model)
    {
        if (User.Identity.IsAuthenticated){
            model.user = new UserModel(UserModel.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        }
        else
        {
            model.user = new UserModel(UserModel.GetUser(0));
        }
        if (ModelState.IsValid){
            if (User.Identity.IsAuthenticated){
                OrderFactory.Create(Order.Direction.OUTGOING,model.user.ID,LineItem.LineItemsFromCart(model.user.Cart()),null,model.ShippingOption);
                foreach (CartLineData cld in model.user.Cart()){
                    model.user.RemoveFromCart(cld.ItemSKU,cld.Quantity);
                }
                Console.WriteLine("User Authed");
            }
            else
            {
                string sNPattern = @"^[a-z0-9.]{4}\/[a-z0-9]*\.[a-z0-9]*$";
                string stellarNet = Regex.Match(model.ContactAddress,sNPattern) != null ? model.ContactAddress : string.Empty;
                string slingName = Regex.Match(model.ContactAddress,sNPattern) != null ? string.Empty : model.ContactAddress;
                OrderFactory.Create(
                    Order.Direction.OUTGOING,
                    0,
                    LineItem.LineItemsFromCart(model.user.Cart()), //That's gonna break, need to come back to this once I know how I'm handling carts of anonymous
                    null, model.ShippingOption);
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
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // JAVASCRIPT FUNCTIONS


    public IActionResult JSAddToCart(string id)
    {
        UserModel user;
        if (User.Identity.IsAuthenticated){
            user = new UserModel(UserModel.GetDatabase().FirstOrDefault(u => u.UserID == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0")));
        }
        else
        {
            user = new UserModel();
        }

        user.AddToCart(id,1);
        CartLineData? cld = user.GetCartLine(id);
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
        UserModel user;
        if (User.Identity.IsAuthenticated){
            user = new UserModel(UserModel.GetDatabase().FirstOrDefault(u => u.UserID == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0")));
        }
        else
        {
            user = new UserModel();
        }

        bool removed = user.RemoveFromCart(id,1);
        CartLineData? cld = user.GetCartLine(id);
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

}
