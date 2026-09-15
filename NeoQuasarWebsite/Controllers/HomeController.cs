using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Controllers;

public class HomeController : Controller
{
    public IActionResult Storefront()
    {
        return View();
    }
    
    public IActionResult Cart(int id)
    {
        CartModel model = new CartModel();
        model.user = UserModel.GetDatabase().FirstOrDefault(u => u.ID == id);
        return View(model);
    }

    public IActionResult CheckOut()
    {
        return View();
    }

    
    public IActionResult PendingOrders()
    {
        PendingOrdersModel pom = new PendingOrdersModel();
        pom.orderList = Order.GetOrderDatabase();
        pom.selectedOrder = pom.orderList[0];
        return View(pom);
    }
    
    public IActionResult ReceiptHistory()
    {
        return View();
    }
    
    public IActionResult RoleManagement()
    {
        var roleModel = new RoleManagementModel();
        roleModel.userList = UserModel.GetDatabase();
        
        return View(roleModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
