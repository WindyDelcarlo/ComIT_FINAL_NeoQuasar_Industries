using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Controllers;

public class AccountController : Controller
{
    [Authorize]
    public IActionResult AccountDetails()
    {
        AccountDetailsModel model = new AccountDetailsModel();
        // model.user = ????
        model.UserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        model.orderDatabase = new List<Order>();
        foreach (OrderData od in Order.GetDatabase().ToList())
        {
            model.orderDatabase.Add(new Order(od));
        }
        return View(model);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Update()
    {
        AccountDetailsModel model = new AccountDetailsModel();
        model.UserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        model.orderDatabase = new List<Order>();
        foreach (OrderData od in Order.GetDatabase().ToList())
        {
            model.orderDatabase.Add(new Order(od));
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Update(UserUpdateModel uum)
    {
        
        AccountDetailsModel model = new AccountDetailsModel();
        model.UserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        model.orderDatabase = new List<Order>();
        foreach (OrderData od in Order.GetDatabase().ToList())
        {
            model.orderDatabase.Add(new Order(od));
        }
        if (uum.Validate())
        {
            /*UserModel user = UserModel.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            user.Name = uum.Name;
            user.SlingName = uum.SlingName;
            user.StellarNet = uum.StellarNet; */

            UserData user = UserModel.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            user.Name = uum.Name;
            user.SlingName = uum.SlingName;
            user.StellarNet = uum.StellarNet;
            Program.Database().SaveChanges();

            if (user != null)

            ViewData["SuccessMessage"] = "Details edited successfully!";
            return RedirectToAction("AccountDetails");
        }
        else
        {
            ViewData["ErrorMessage"] = uum.ErrorMessage;
            return View(model);
        }
    }
    [Authorize]
    [HttpGet]
    public IActionResult Cards()
    {
        AddingPaymentsModel model = new AddingPaymentsModel();
        model.UserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cards(AddingPaymentsModel apm)
    {
        if (!ModelState.IsValid)
        { 
            apm.UserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));;
            return View(apm);
        }
        apm.UserID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        string pdType = apm.CardType;
        switch (apm.CardType)
        {
            case "Requisitions Account":
                {
                    pdType = "REQUISITIONS_ACCOUNT";
                    break;
                }
            case "StellarNet Debit":
                {
                    pdType = "STELLARNET_DEBIT";
                    break;
                }
                
            case "Credit":
                {
                    pdType = "CREDIT";
                    break;
                }
        }
        PaymentDetails pd = new PaymentDetails((PaymentDetails.Type)Enum.Parse(typeof(PaymentDetails.Type),pdType),apm.CardName,apm.CardNumber);
        if (PaymentDetails.ValidateCard(pd))
        {
            PaymentDetails.DatabaseAdd(pd,int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            TempData["SuccessMessage"] = "Order created successfully!";
            return RedirectToAction("AccountDetails");
        }
        else if (pd.CardType == PaymentDetails.Type.CREDIT)
        {
            TempData["ErrorMessage"] = "Card Number Mismatch! Credit Card format is ast0:123 4567 890. (creditCode : 10 Digits)";
        }
        else if (pd.CardType == PaymentDetails.Type.STELLARNET_DEBIT)
        {
            TempData["ErrorMessage"] = "Card Number Mismatch! StellarNet Debit format is as1234-567 (bankCode - cardNumber of any length)";
        }
        else if (pd.CardType == PaymentDetails.Type.REQUISITIONS_ACCOUNT)
        {
            TempData["ErrorMessage"] = "Card Number Mismatch! Requisitions Account format is MARIREY-NQ:1234 (Internal NQI code : 4 Digits)";
        }
        else
        {
            TempData["ErrorMessage"] = "An unknown error has occured. Please check the card number and try again.";
        }
        return View(apm);
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
            UserData? user = UserModel.GetDatabase().FirstOrDefault(u => u.Username == model.Username);
            if (user == null)
            {
                return View(model);
            }
            else
            {
                if (UserModel.CheckPassword(user.Username, model.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                        new Claim(ClaimTypes.Name, user.Name)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity))
                    .GetAwaiter()
                    .GetResult();
                }
            }

            /*foreach (UserModel um in UserModel.GetDatabase())
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
            } */

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
            return RedirectToAction("Storefront","Home");
        }
        else
        {
            return View(model);
        }
    }

    public IActionResult JSDeleteCard(int id)
    {
        PaymentDetailData? pd = PaymentDetails.GetDatabase().FirstOrDefault(p => p.PaymentDetailID == id);
        if (pd != null)
        {
            PaymentDetails.DatabaseDelete(pd);
            return Json(new {
                success = true,
                error = ""
            });
        }
        else 
        {
            return Json(new {
                success = false,
                error = "Card not found."
            });
        };
    }


}