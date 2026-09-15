using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Controllers;

public class AccountController : Controller
{
    [Authorize]
    public IActionResult AccountDetails()
    {
        AccountDetailsModel model = new AccountDetailsModel();
        // model.user = ????
        model.user = UserModel.GetDatabase()[1];
        model.orderDatabase = Order.GetOrderDatabase();
        return View(model);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Update(int id)
    {
        AccountDetailsModel model = new AccountDetailsModel();
        model.user = UserModel.GetDatabase().FirstOrDefault(user => user.ID == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        model.orderDatabase = Order.GetOrderDatabase();
        return View(model);
    }

    [HttpPost]
    public IActionResult Update(int id,UserUpdateModel uum)
    {
        AccountDetailsModel model = new AccountDetailsModel();
        model.user = UserModel.GetDatabase().FirstOrDefault(user => user.ID == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
        model.orderDatabase = Order.GetOrderDatabase();
        if (uum.Validate())
        {
            UserModel user = UserModel.GetDatabase().FirstOrDefault(user => user.ID == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            user.Name = uum.Name;
            user.SlingName = uum.SlingName;
            user.StellarNet = uum.StellarNet; 
            ViewData["SuccessMessage"] = "Details edited successfully!";
            return RedirectToAction("AccountDetails");
        }
        else
        {
            ViewData["ErrorMessage"] = uum.ErrorMessage;
            return View(model);
        }
    }
    
    [HttpGet]
    public IActionResult LogIn()
    {
        return View();
    }

    [HttpPost]
    public IActionResult LogIn(LogInModel model)
    {
        if (ModelState.IsValid)
        {
            foreach (UserModel um in UserModel.GetDatabase())
            {
                if (um.Username == model.Username && UserModel.CheckPassword(model.Username,model.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, um.ID.ToString()),
                        new Claim(ClaimTypes.Name, um.Name)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity))
                    .GetAwaiter()
                    .GetResult();
                }
                else
                {
                    
                }
            }

            return LocalRedirect("/Home/Storefront");
        }
        
        return View(model);
    }

    public IActionResult LogOut(){
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
        .GetAwaiter()
        .GetResult();

        return LocalRedirect("/home/storefront");
    }

    [HttpGet]
    public IActionResult Register()
    {
        RegisterModel model = new RegisterModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            ViewData["SuccessMessage"] = "Registration completed successfully!";
            UserModel.DatabaseAdd(UserFactory.Create(model.Name,model.Username,model.DimAddress,model.LocatorCode,model.SlingName,model.StellarNet,UserRole.USER),model.Password);
            return RedirectToAction("Storefront");
        }
        else
        {
            return View(model);
        }
    }


}