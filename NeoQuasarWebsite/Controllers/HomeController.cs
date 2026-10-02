using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Controllers;

public class HomeController : Controller
{
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
                new PaymentDetails(PaymentDetails.GetDatabase().Where(pd => pd.UserId == userData.UserID).ToList()[userData.SelectedCard]);
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
    [ValidateAntiForgeryToken]
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
    
    [HttpGet]
    public IActionResult PendingOrders(string? id)
    {
        PendingOrdersModel pom = new PendingOrdersModel();
        pom.orderList = new List<Order>();
        foreach (OrderData od in Order.GetDatabase())
        {
            pom.orderList.Add(new Order(od));
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
    [ValidateAntiForgeryToken]
    public IActionResult PendingOrders(PendingOrdersModel pom)
    {
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
    
    public IActionResult ReceiptHistory()
    {
        return View();
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

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

    public IActionResult SelectOrder(string id)
    {
        return RedirectToAction("PendingOrders", new { id = id });
    }
}
