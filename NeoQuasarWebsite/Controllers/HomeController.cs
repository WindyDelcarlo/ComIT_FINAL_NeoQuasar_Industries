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
    private readonly IHttpContextAccessor _httpContext;
    private readonly IAnonService _anonService;
    private readonly IPaymentService _paymentService;

    public HomeController(NeoQuasarDBContext dbContext, IAccountService accountService, ISearchService searchService, IDataService dataService, IOrderService orderService, IHttpContextAccessor httpContext, IAnonService anonService, IPaymentService paymentService)
    {
        _dbContext = dbContext;
        _accountService = accountService;
        _searchService = searchService;
        _dataService = dataService;
        _orderService = orderService;
        _httpContext = httpContext;
        _anonService = anonService;
        _paymentService = paymentService;
    }

    public IActionResult Storefront(string? search)
    {
        int userID;
        UserDTO dto;
        if (User.Identity.IsAuthenticated)
        {
            userID = _accountService.GetCurrentUserID(User);
            dto = new UserDTO
            {
                UserID = userID,
                Cart = _dataService.GetUserCart(userID)
            };
        }
        else
        {
            userID = 0;
            if (_httpContext.HttpContext.Session.Get<AnonUserDTO>("User") == null)
            {
                AnonUserDTO anonUser = new AnonUserDTO
                {
                    Key = Program.Date()
                };
                _httpContext.HttpContext.Session.Set<AnonUserDTO>("User", anonUser);
            }
            AnonUserDTO user = _httpContext.HttpContext.Session.Get<AnonUserDTO>("User");
            dto = new UserDTO
            {
                UserID = -1,
                Cart = _anonService.GetCart(user)
            };

        }
        StorefrontModel model = new StorefrontModel();
        model.User = dto;
        List<string> searchTerms;
        int searchResultCount;
        if (search != null)
        {
            model.SearchResults = _searchService.StockSearch(search, out searchTerms, out searchResultCount);
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

        // since this method uses the CartLineData directly, the stock-handling part of this screen is in the JSGetCart method.
        // Not refactoring the entire DTO now.

        if (User.Identity.IsAuthenticated)
        {
            int userID = _accountService.GetCurrentUserID(User);

            model.user = new UserDTO
            {
                UserID = userID,
                Cart = _dataService.GetUserCart(userID),
            };
        }
        else if (AnonUser())
        {
            AnonUserDTO user = _httpContext.HttpContext.Session.Get<AnonUserDTO>("User");
            model.user = new UserDTO
            {
                UserID = -1,
                Cart = _anonService.GetCart(user)
            };
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult CheckOut()
    {
        CheckOutModel model = new CheckOutModel();
        if (User.Identity.IsAuthenticated)
        {
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
            if (_dataService.GetUserPayments(userID).Count() > 0 && _dataService.GetPaymentDetail(user.SelectedCard) != null)
            {
                activeCard = new PaymentDTO
                {
                    CardNumber = _dataService.GetUserPaymentDetail(userID).CardNumber
                };
            }
            model.CardNumber = activeCard != null ? activeCard.CardNumber : "No Card Selected";
            model.ContactAddress = user.ContactAddress();
            model.LocatorCode = user.LocatorCode;
            model.Authenticated = true;
        }
        else if (AnonUser())
        {
            AnonUserDTO user = _httpContext.HttpContext.Session.Get<AnonUserDTO>("User");
            model.user = new UserDTO
            {
                UserID = -1,
                Cart = _anonService.GetCart(user)
            };
        }
        if (model.user != null && model.user.Cart.Count() > 0)
        {
            return View(model);
        }
        return RedirectToAction("Cart");
    }

    [HttpPost]
    public IActionResult CheckOut(CheckOutModel model)
    {

        if (User.Identity.IsAuthenticated)
        {
            int userID = _accountService.GetCurrentUserID(User);
            model.user = new UserDTO
            {
                UserID = userID,
                Cart = _dataService.GetUserCart(userID),
                Name = _dataService.GetUser(userID).Name
            };
            model.ContactAddress = _dataService.GetUserContact(userID);
            model.AddressStreet = _dataService.GetUserAddress(userID);
            model.LocatorCode = _dataService.GetUserLOCCode(userID);
            model.Authenticated = true;
        }
        else if (AnonUser())
        {
            AnonUserDTO user = _httpContext.HttpContext.Session.Get<AnonUserDTO>("User");
            model.user = new UserDTO
            {
                UserID = -1,
                Cart = _anonService.GetCart(user),
                Name = model.AddressName,
                
            };
        }
        
        if (_dataService.GetDimension(model.LocatorCode.Substring(0,4).ToLower()) == null)
        {
            ModelState.AddModelError(nameof(model.LocatorCode),"LOC Code doesn't have a valid DimCode! (The first four characters should match the dimension of the listed address.");
            TempData["ErrorMessage"] = "LOC Code doesn't match a known dimension. If this LOC Code exists, please contact an administrator.";
            return View(model);
        }

        if (ModelState.IsValid)
        {

            PaymentDTO payment = new PaymentDTO
            {
                CardName = model.user.Name,
                CardNumber = model.CardNumber,
                Amount = _paymentService.TotalCost(model.user,model.ShippingOption.ToString(),model.LocatorCode)
            };
            if (!_paymentService.ProcessPayment(payment))
            {
                TempData["ErrorMessage"] = "Payment failed.";
                return LocalRedirect("~/Home/Storefront");
            }

            string orderID = _orderService.CreateOrder(model.user.UserID, "OUTGOING", model.ShippingOption.ToString(), model.user.Name, model.ContactAddress, model.AddressStreet, model.LocatorCode);
            foreach (CartLineData cld in model.user.Cart)
            {
                _orderService.AddLine(orderID, cld);
                if (AnonUser())
                {
                    AnonUserDTO user = _httpContext.HttpContext.Session.Get<AnonUserDTO>("User");
                    _anonService.ResetCart(user);
                    _httpContext.HttpContext.Session.Set<AnonUserDTO>("User",user);
                }
                else
                {
                    _accountService.RemoveFromCart(cld.ItemSKU, model.user.UserID, cld.Quantity);
                }
            }
            Console.WriteLine("Order created successfully!");
            TempData["SuccessMessage"] = "Order created successfully!";
            return LocalRedirect("~/Home/Storefront");
        }
        else
        {
            TempData["ErrorMessage"] = "Order couldn't be completed.";
            return View(model);
        }
    }

    public IActionResult Item(string id)
    {
        StockModel model = new StockModel();
        StockItemData sid = _dataService.GetStockItem(id);
        int stock = sid.Stock;
            // This accounts for orders that haven't been shipped yet, which won't have had their numbers removed from the system.
            // Ideally customers don't think there's stock when really there isn't.
            foreach(OrderLineData old in _dataService.GetItemOrderLines(sid.NeoSKU))
            {
                stock -= old.AmountOrdered - old.Shipped;
            }
            // No reason to show a negative number to the customer, as amusing as it would be to see that there are -700 star plushies in the system.
            if (stock < 0)
            {
                stock = 0;
            }
        model.Item = new StockItemDTO
        {
            Name = sid.Name,
            NeoSKU = sid.NeoSKU,
            ManufacturerSKU = sid.ManufacturerSKU,
            ManufacturerName = _dataService.GetManufacturer(sid.ManufacturerID).Name,
            Price = sid.Price,
            Stock = stock,
            Category = sid.StockCategory,
            Image = sid.ImageLocation,
            Description = sid.Description,
            NoRestock = sid.DontRestock
        };

        if (User.Identity.IsAuthenticated)
        {
            int userID = _accountService.GetCurrentUserID(User);
            model.User = new UserDTO
            {
                UserID = userID,
                Cart = _dataService.GetUserCart(userID)
            };
        }
        else if (AnonUser())
        {
            AnonUserDTO user = _httpContext.HttpContext.Session.Get<AnonUserDTO>("User");
            model.User = new UserDTO
            {
                UserID = -1,
                Cart = _anonService.GetCart(user)
            };
        }

        return View(model);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // HELPER METHODS

    public bool AnonUser()
    {
        // This shouldn't be able to happen, but if it does, I'd rather fail a request than crash the program.
        return _httpContext.HttpContext.Session.Get<AnonUserDTO>("User") != null;
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
                // Lines here to show an accurate amount of stock to the customer.
                int stock = item.Stock;
                foreach(OrderLineData old in _dataService.GetItemOrderLines(item.NeoSKU))
                {
                    stock -= old.AmountOrdered - old.Shipped;
                }
                cartList.Add(new
                {
                    SKU = cld.ItemSKU,
                    ManufacturerSKU = item.ManufacturerSKU,
                    Image = item.ImageLocation,
                    Quantity = cld.Quantity,
                    Name = item.Name,
                    Price = item.Price,
                    Stock = stock
                });
            }

            return Json(new
            {
                success = true,
                error = "",
                items = cartList
            });
        }
        else if (AnonUser())
        {
            AnonUserDTO user = _httpContext.HttpContext.Session.Get<AnonUserDTO>("User");
           
           List<object> cartList = new List<object>();
            foreach (CartLineData cld in _anonService.GetCart(user))
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
        else
        {
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
        if (User.Identity.IsAuthenticated)
        {
            userID = _accountService.GetCurrentUserID(User);

            _accountService.AddToCart(id, userID, 1);
            CartLineData? cld = _dataService.GetCartLine(id, userID);
            if (cld != null)
            {
                return Json(new
                {
                    success = true,
                    count = cld.Quantity
                });
            }
        }
        else if (AnonUser())
        {
            AnonUserDTO user = _httpContext.HttpContext.Session.Get<AnonUserDTO>("User");
            if (_anonService.AddToCart(user, id, 1))
            {
                _httpContext.HttpContext.Session.Set<AnonUserDTO>("User", user);
                return Json(new
                {
                    success = true,
                    count = user.lines.FirstOrDefault(l => l.NeoSKU == id).Quantity
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    error = "Line couldn't be added."
                });
            }
        }
        return Json(new
        {
            success = false,
            error = "Line couldn't be added."
        });
    }

    public IActionResult JSRemoveFromCart(string id)
    {
        int userID;
        if (User.Identity.IsAuthenticated)
        {
            userID = _accountService.GetCurrentUserID(User);
            bool removed = _accountService.RemoveFromCart(id, userID, 1);
            CartLineData? cld = _dataService.GetCartLine(id, userID);
            if (cld != null && removed)
            {
                return Json(new
                {
                    success = true,
                    count = cld.Quantity
                });
            }
            else if (removed)
            {
                return Json(new
                {
                    success = true,
                    count = 0
                });
            }
        }
        else if (AnonUser())
        {
            AnonUserDTO user = _httpContext.HttpContext.Session.Get<AnonUserDTO>("User");
            bool lineZeroed = false;
            if (_anonService.RemoveFromCart(user, id, 1, out lineZeroed))
            {
                _httpContext.HttpContext.Session.Set<AnonUserDTO>("User", user);
                if (lineZeroed){
                return Json(new
                    {
                        success = true,
                        count = 0
                    });
                }
                else
                {
                return Json(new {
                    success = true,
                    count = user.lines.FirstOrDefault(l => l.NeoSKU == id).Quantity
                });      
                }
            }
            else
            {
                return Json(new
                {
                    success = false,
                    error = "Line couldn't be added."
                });
            }
        }

        return Json(new
        {
            success = false,
            error = "Line couldn't be removed."
        });
    }

    public IActionResult JSCheckTax(string? locCode)
    {
        if (User.Identity.IsAuthenticated)
        {
            int userID = _accountService.GetCurrentUserID(User);
            string dimCode = _dataService.GetDimCodeFromLOC(_dataService.GetUserLOCCode(userID).Substring(0,4).ToLower());
            double taxPercent = _paymentService.GetTax(dimCode);
            return Json(new
            {
                success = true,
                error = "",
                tax = taxPercent
            });
        }
        
        if (locCode != null)
        {
            string dimCode = _dataService.GetDimCodeFromLOC(locCode);
            if (_dataService.GetDimension(dimCode) != null)
            {
                double taxPercent = _paymentService.GetTax(dimCode);
                return Json(new
                {
                    success = true,
                    error = "",
                    tax = taxPercent
                });
            }
        }


        return Json(new
        {
            success = false,
            error = "Dimension Not Found",
            tax = 0
        });
    }
}
