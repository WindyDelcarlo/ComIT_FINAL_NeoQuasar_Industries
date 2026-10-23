using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;
using NeoQuasarWebsite.Models;
using NeoQuasarWebsite.Services;

namespace NeoQuasarWebsite.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly NeoQuasarDBContext _dbContext;
    private readonly IPasswordHasher<UserData> _passwordHasher;
    private readonly IAccountService _accountService;
    private readonly IDataService _dataService;

    public AccountController(NeoQuasarDBContext dbContext,IPasswordHasher<UserData> passwordHasher, IAccountService accountService, IDataService dataService){
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _accountService = accountService;
        _dataService = dataService;
    }

    public IActionResult AccountDetails()
    {
        AccountDetailsModel model = new AccountDetailsModel();
        UserData user = _accountService.GetCurrentUser(User);
        model.UserID = user.UserID;
        model.User = new UserDTO
        {
            Name = user.Name,
            StellarNet = user.StellarNet,
            SlingName = user.SlingName
        };
        model.orderData = new List<OrderDTO>();
        model.paymentData = new List<PaymentDTO>();

        foreach (OrderData od in _dataService.GetUserOrders(model.UserID))
        {
            List<OrderLineData> lines = _dataService.GetOrderLines(od.OrderID);
            List<LineDTO> lineDTO = new List<LineDTO>();
            int totalPrice = 0;
            foreach (OrderLineData old in lines)
            {
                int price = _dataService.GetStockItem(old.ItemSKU).Price;
                totalPrice += price;
                lineDTO.Add(new LineDTO
                {
                    Ordered = old.AmountOrdered,
                    NeoSKU = old.ItemSKU,
                    Description = _dataService.GetStockItem(old.ItemSKU).Name,
                    Price = price
                });
            }
            model.orderData.Add(new OrderDTO
            {
                ID = od.OrderID,
                Date = od.Date,
                TotalPrice = totalPrice,
                lines = lineDTO
            });
        }
        foreach (PaymentDetailData pdd in _dataService.GetUserPayments(user.UserID))
        {
            model.paymentData.Add(new PaymentDTO
            {
                CardType = pdd.CardType.ToString(),
                CardName = pdd.CardName,
                CardNumber = pdd.CardNumber,
                Image = pdd.GetImagePath(),
                Selected = pdd.PaymentDetailID == user.SelectedCard
            });
        }
        model.orderData.Reverse();
        return View(model);
    }

    [HttpGet]    
    public IActionResult Update()
    {
        AccountDetailsModel model = new AccountDetailsModel();
        UserData user = _accountService.GetCurrentUser(User);
        model.UserID = user.UserID;
        model.User = new UserDTO
        {
            Name = user.Name,
            StellarNet = user.StellarNet,
            SlingName = user.SlingName
        };
        model.orderData = new List<OrderDTO>();
        model.paymentData = new List<PaymentDTO>();

        foreach (OrderData od in _dataService.GetUserOrders(model.UserID))
        {
            List<OrderLineData> lines = _dataService.GetOrderLines(od.OrderID);
            List<LineDTO> lineDTO = new List<LineDTO>();
            int totalPrice = 0;
            foreach (OrderLineData old in lines)
            {
                int price = _dataService.GetStockItem(old.ItemSKU).Price;
                totalPrice += price;
                lineDTO.Add(new LineDTO
                {
                    Ordered = old.AmountOrdered,
                    NeoSKU = old.ItemSKU,
                    Description = _dataService.GetStockItem(old.ItemSKU).Name,
                    Price = price
                });
            }
            model.orderData.Add(new OrderDTO
            {
                ID = od.OrderID,
                Date = od.Date,
                TotalPrice = totalPrice,
                lines = lineDTO
            });
        }
        foreach (PaymentDetailData pdd in _dataService.GetUserPayments(user.UserID))
        {
            model.paymentData.Add(new PaymentDTO
            {
                CardType = pdd.CardType.ToString(),
                CardName = pdd.CardName,
                CardNumber = pdd.CardNumber,
                Image = pdd.GetImagePath(),
                Selected = pdd.PaymentDetailID == user.SelectedCard
            });
        }
        model.orderData.Reverse();
        return View(model);
    }

    [HttpPost]
    public IActionResult Update(UserUpdateModel uum)
    {
        
        AccountDetailsModel model = new AccountDetailsModel();
        UserData user = _accountService.GetCurrentUser(User);
        model.UserID = user.UserID;
        model.User = new UserDTO
        {
            Name = user.Name,
            StellarNet = user.StellarNet,
            SlingName = user.SlingName
        };
        model.orderData = new List<OrderDTO>();
        model.paymentData = new List<PaymentDTO>();

        foreach (OrderData od in _dataService.GetUserOrders(model.UserID))
        {
            List<OrderLineData> lines = _dataService.GetOrderLines(od.OrderID);
            List<LineDTO> lineDTO = new List<LineDTO>();
            int totalPrice = 0;
            foreach (OrderLineData old in lines)
            {
                int price = _dataService.GetStockItem(old.ItemSKU).Price;
                totalPrice += price;
                lineDTO.Add(new LineDTO
                {
                    Ordered = old.AmountOrdered,
                    NeoSKU = old.ItemSKU,
                    Description = _dataService.GetStockItem(old.ItemSKU).Name,
                    Price = price
                });
            }
            model.orderData.Add(new OrderDTO
            {
                ID = od.OrderID,
                Date = od.Date,
                TotalPrice = totalPrice,
                lines = lineDTO
            });
        }
        foreach (PaymentDetailData pdd in _dataService.GetUserPayments(user.UserID))
        {
            model.paymentData.Add(new PaymentDTO
            {
                CardType = pdd.CardType.ToString(),
                CardName = pdd.CardName,
                CardNumber = pdd.CardNumber,
                Image = pdd.GetImagePath(),
                Selected = pdd.PaymentDetailID == user.SelectedCard
            });
        }
        if (uum.Validate())
        {

            _accountService.EditAccountDetails(new UserDTO
            {
                Name = uum.Name,
                SlingName = uum.SlingName,
                StellarNet = uum.StellarNet
            }, user);

            ViewData["SuccessMessage"] = "Details edited successfully!";
            return RedirectToAction("AccountDetails");
        }
        else
        {
            TempData["ErrorMessage"] = uum.ErrorMessage;
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Cards()
    {
        AddingPaymentsModel model = new AddingPaymentsModel();
        UserData user = _accountService.GetCurrentUser(User);
        model.UserID = user.UserID;
        model.User = new UserDTO
        {
            Name = user.Name,
            StellarNet = user.StellarNet,
            SlingName = user.SlingName
        };
        model.orderData = new List<OrderDTO>();
        model.paymentData = new List<PaymentDTO>();

        foreach (OrderData od in _dataService.GetUserOrders(model.UserID))
        {
            List<OrderLineData> lines = _dataService.GetOrderLines(od.OrderID);
            List<LineDTO> lineDTO = new List<LineDTO>();
            int totalPrice = 0;
            foreach (OrderLineData old in lines)
            {
                int price = _dataService.GetStockItem(old.ItemSKU).Price;
                totalPrice += price;
                lineDTO.Add(new LineDTO
                {
                    Ordered = old.AmountOrdered,
                    NeoSKU = old.ItemSKU,
                    Description = _dataService.GetStockItem(old.ItemSKU).Name,
                    Price = price
                });
            }
            model.orderData.Add(new OrderDTO
            {
                ID = od.OrderID,
                Date = od.Date,
                TotalPrice = totalPrice,
                lines = lineDTO
            });
        }
        foreach (PaymentDetailData pdd in _dataService.GetUserPayments(user.UserID))
        {
            model.paymentData.Add(new PaymentDTO
            {
                CardID = pdd.PaymentDetailID,
                CardType = pdd.CardType.ToString(),
                CardName = pdd.CardName,
                CardNumber = pdd.CardNumber,
                Image = pdd.GetImagePath(),
                Selected = pdd.PaymentDetailID == user.SelectedCard
            });
        }
        model.orderData.Reverse();
        return View(model);
    }

    [HttpPost]
    public IActionResult Cards(AddingPaymentsModel apm)
    {
        UserData user = _accountService.GetCurrentUser(User);
        apm.UserID = user.UserID;
        apm.User = new UserDTO
        {
            Name = user.Name,
            StellarNet = user.StellarNet,
            SlingName = user.SlingName
        };
        apm.orderData = new List<OrderDTO>();
        apm.paymentData = new List<PaymentDTO>();

        foreach (OrderData od in _dataService.GetUserOrders(apm.UserID))
        {
            List<OrderLineData> lines = _dataService.GetOrderLines(od.OrderID);
            List<LineDTO> lineDTO = new List<LineDTO>();
            int totalPrice = 0;
            foreach (OrderLineData old in lines)
            {
                int price = _dataService.GetStockItem(old.ItemSKU).Price;
                totalPrice += price;
                lineDTO.Add(new LineDTO
                {
                    Ordered = old.AmountOrdered,
                    NeoSKU = old.ItemSKU,
                    Description = _dataService.GetStockItem(old.ItemSKU).Name,
                    Price = price
                });
            }
            apm.orderData.Add(new OrderDTO
            {
                ID = od.OrderID,
                Date = od.Date,
                TotalPrice = totalPrice,
                lines = lineDTO
            });
        }
        foreach (PaymentDetailData pdd in _dataService.GetUserPayments(user.UserID))
        {
            apm.paymentData.Add(new PaymentDTO
            {
                CardID = pdd.PaymentDetailID,
                CardType = pdd.CardType.ToString(),
                CardName = pdd.CardName,
                CardNumber = pdd.CardNumber,
                Image = pdd.GetImagePath(),
                Selected = pdd.PaymentDetailID == user.SelectedCard
            });
        }
        apm.orderData.Reverse();
        if (!ModelState.IsValid)
        { 
            apm.UserID = _accountService.GetCurrentUserID(User);
            return View(apm);
        }
        apm.UserID = _accountService.GetCurrentUserID(User);
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
        PaymentDTO pd = new PaymentDTO
        {
            CardType = pdType,
            CardName = apm.CardName,
            CardNumber = apm.CardNumber
        };
        string errorMessage = string.Empty;
        if (_accountService.ValidateCard(pd,out errorMessage))
        {
            _accountService.AddPaymentDetails(pd,apm.UserID);
            TempData["SuccessMessage"] = "Order created successfully!";
            return RedirectToAction("AccountDetails");
        }

        TempData["ErrorMessage"] = errorMessage;
        return View(apm);
    }

    [HttpGet]    
    public IActionResult Password()
    {
        PasswordUpdateModel model = new PasswordUpdateModel();
        UserData user = _accountService.GetCurrentUser(User);
        model.UserID = user.UserID;
        model.User = new UserDTO
        {
            Name = user.Name,
            StellarNet = user.StellarNet,
            SlingName = user.SlingName
        };
        model.orderData = new List<OrderDTO>();
        model.paymentData = new List<PaymentDTO>();

        foreach (OrderData od in _dataService.GetUserOrders(model.UserID))
        {
            List<OrderLineData> lines = _dataService.GetOrderLines(od.OrderID);
            List<LineDTO> lineDTO = new List<LineDTO>();
            int totalPrice = 0;
            foreach (OrderLineData old in lines)
            {
                int price = _dataService.GetStockItem(old.ItemSKU).Price;
                totalPrice += price;
                lineDTO.Add(new LineDTO
                {
                    Ordered = old.AmountOrdered,
                    NeoSKU = old.ItemSKU,
                    Description = _dataService.GetStockItem(old.ItemSKU).Name,
                    Price = price
                });
            }
            model.orderData.Add(new OrderDTO
            {
                ID = od.OrderID,
                Date = od.Date,
                TotalPrice = totalPrice,
                lines = lineDTO
            });
        }
        foreach (PaymentDetailData pdd in _dataService.GetUserPayments(user.UserID))
        {
            model.paymentData.Add(new PaymentDTO
            {
                CardType = pdd.CardType.ToString(),
                CardName = pdd.CardName,
                CardNumber = pdd.CardNumber,
                Image = pdd.GetImagePath(),
                Selected = pdd.PaymentDetailID == user.SelectedCard
            });
        }
        model.orderData.Reverse();
        return View(model);
    }

        [HttpPost]
    public IActionResult Update(PasswordUpdateModel pum)
    {
        
        AccountDetailsModel model = new AccountDetailsModel();
        UserData user = _accountService.GetCurrentUser(User);
        model.UserID = user.UserID;
        model.User = new UserDTO
        {
            Name = user.Name,
            StellarNet = user.StellarNet,
            SlingName = user.SlingName
        };
        model.orderData = new List<OrderDTO>();
        model.paymentData = new List<PaymentDTO>();

        foreach (OrderData od in _dataService.GetUserOrders(model.UserID))
        {
            List<OrderLineData> lines = _dataService.GetOrderLines(od.OrderID);
            List<LineDTO> lineDTO = new List<LineDTO>();
            int totalPrice = 0;
            foreach (OrderLineData old in lines)
            {
                int price = _dataService.GetStockItem(old.ItemSKU).Price;
                totalPrice += price;
                lineDTO.Add(new LineDTO
                {
                    Ordered = old.AmountOrdered,
                    NeoSKU = old.ItemSKU,
                    Description = _dataService.GetStockItem(old.ItemSKU).Name,
                    Price = price
                });
            }
            model.orderData.Add(new OrderDTO
            {
                ID = od.OrderID,
                Date = od.Date,
                TotalPrice = totalPrice,
                lines = lineDTO
            });
        }
        foreach (PaymentDetailData pdd in _dataService.GetUserPayments(user.UserID))
        {
            model.paymentData.Add(new PaymentDTO
            {
                CardType = pdd.CardType.ToString(),
                CardName = pdd.CardName,
                CardNumber = pdd.CardNumber,
                Image = pdd.GetImagePath(),
                Selected = pdd.PaymentDetailID == user.SelectedCard
            });
        }

        
        if (_accountService.GetUser(new UserDTO {Username = user.Username, Password = pum.Password}) == null){
            ModelState.AddModelError(nameof(pum.Password), "Incorrect password.");
            return View(model);
        }

        if (ModelState.IsValid)
        {
            _accountService.UpdatePassword(User,pum.NewPassword);
            
            ViewData["SuccessMessage"] = "Details edited successfully!";
            return RedirectToAction("AccountDetails");
        }
        else
        {
            TempData["ErrorMessage"] = pum.ErrorMessage;
            return View(model);
        }
    }
    
    [AllowAnonymous]
    [HttpGet]
    public IActionResult LogIn(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> LogIn(LogInModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        UserDTO user = _accountService.GetUser(new UserDTO { Username = model.Username, Password = model.Password });

        if (user == null){
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }

        if (model.Password == "password")
        {
            TempData["ErrorMessage"] = "Your password is insecured! Update your password!";
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

    public IActionResult Delete(){
        if (_accountService.DeleteUser(User))
        {
            TempData["SuccessMessage"] = "Successfully deleted account. You have been logged out.";
        }
        else
        {
            TempData["ErrorMessage"] = "Issue deleting account. You have been logged out.";
        }

        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme)
        .GetAwaiter()
        .GetResult();

        return LocalRedirect("/home/storefront");
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Register()
    {
        RegisterModel model = new RegisterModel();
        return View(model);
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterModel model)
    {
        UserDTO newUser = new UserDTO
        {
            Username = model.Username,
            Name = model.Name,
            SlingName = model.SlingName,
            StellarNet = model.StellarNet,
            Address = model.DimAddress,
            LocatorCode = model.LocatorCode,
            Password = model.Password
        };

        if (!_accountService.VerifyUniqueUsernameAndContact(newUser, out int error))
        {   
            switch (error)
            {
                case 0:
                    {
                        ModelState.AddModelError(nameof(model.Username),"Username is already in use!");
                        return View(model);
                    }
                case 1:
                    {
                        ModelState.AddModelError(nameof(model.SlingName),"ContactAddress is already in use!");
                        return View(model);
                    }
                case 2:
                    {
                        ModelState.AddModelError(nameof(model.StellarNet),"Contact Address is already in use!");
                        return View(model);
                    }
            }
        }

        if (ModelState.IsValid)
        {
            ViewData["SuccessMessage"] = "Registration completed successfully!";
            _accountService.Register(newUser);
            return RedirectToAction("Storefront","Home");
        }
        else
        {
            return View(model);
        }
    }

    public IActionResult JSDeleteCard(int id)
    {
        PaymentDetailData? pd = _dataService.GetPaymentDetail(id) ;
        UserData user = _accountService.GetCurrentUser(User);
        if (pd != null && pd.UserID == user.UserID)
        {
            _accountService.RemoveCard(pd.PaymentDetailID,user.UserID);
            return Json(new {
                success = true,
                error = ""
            });
        }
        else 
        {
            return Json(new {
                success = false,
                error = "Card not found or doesn't belong to the user."
            });
        };
    }

    public IActionResult JSSelectCard(int id)
    {
        PaymentDetailData? pd = _dataService.GetPaymentDetail(id);
        int userID = _accountService.GetCurrentUserID(User);

        if (pd != null && pd.UserID == userID)
        {
            _accountService.SelectCard(pd.PaymentDetailID,userID);
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
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
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