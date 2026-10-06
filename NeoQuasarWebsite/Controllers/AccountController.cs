using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite.Controllers;

public class AccountController : Controller
{
    private readonly NeoQuasarDBContext _dbContext;
    private readonly IPasswordHasher<UserData> _passwordHasher;
    private readonly IAccountService _accountService;

    public AccountController(NeoQuasarDBContext dbContext,IPasswordHasher<UserData> passwordHasher, IAccountService accountService){
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _accountService = accountService;
    }

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
            UserModel.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier)).SelectedCard = PaymentDetails.GetDatabase().FirstOrDefault(p => p.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) && p.CardNumber == pd.CardNumber)?.PaymentDetailID ?? 0;
            Program.Database().SaveChanges();
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
    public IActionResult LogIn(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LogInModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        UserDTO user = _accountService.GetUser(new DTO.UserDTO { Username = model.Username, Password = model.Password });

        if (user == null){
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        await SignInAsync(user, model.RememberMe);
        return RedirectToLocal(returnUrl);
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

    public IActionResult JSSelectCard(int id)
    {
        PaymentDetailData? pd = PaymentDetails.GetDatabase().FirstOrDefault(p => p.PaymentDetailID == id);
        if (pd != null)
        {
            UserModel.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier)).SelectedCard = pd.PaymentDetailID;
            Program.Database().SaveChanges();
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


    // Helper Methods
    private async Task SignInAsync(UserDTO user, bool isPersistent)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var properties = new AuthenticationProperties { IsPersistent = isPersistent };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            properties);
    }

     private IActionResult RedirectToLocal(string? returnUrl)
    {
        return Url.IsLocalUrl(returnUrl)
            ? LocalRedirect(returnUrl!)
            : RedirectToAction("Storefront", "Home");
    }

}