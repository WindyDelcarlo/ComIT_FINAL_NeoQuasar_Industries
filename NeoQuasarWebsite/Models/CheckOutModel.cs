using System.ComponentModel.DataAnnotations;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class CheckOutModel
{
    [Required(ErrorMessage = "Name Required")]
    [RegularExpression(@"^[a-zA-Z0-9 \-]*$",ErrorMessage = "Only letters, numbers, hyphens, and spaces are permitted in names.")]
    [Display(Name = "Name")]
    public string AddressName{get;set;} = string.Empty;

    [Required(ErrorMessage = "Address is required.")]
    [RegularExpression(@"^[a-zA-Z0-9_=<>.,&%$ :\/\-]*$",ErrorMessage = "Address uses unrecognized characters.")]
    [Display(Name = "Address")]
    public string AddressStreet{get;set;} = string.Empty;

    [Required(ErrorMessage = "LOC Code is required.")]
    [RegularExpression(@"^[A-Z0-9\-]*$",ErrorMessage = "Locator Codes use only capital letters, numbers, and hyphens.")]
    [Display(Name = "LOC Code")]
    public string LocatorCode{get;set;} = string.Empty;

    [Required(ErrorMessage = "Card Required")]
    [RegularExpression(@"^[a-z]{2}[0-9]{4}-[0-9]{2,}$|^[a-zA-Z0-9]{4}:(?:\s?[0-9]){10}$|^[A-Z]{2,}-[A-Z]{2}:[0-9]+$",ErrorMessage = "Invalid card number.")]
    public string CardNumber{get;set;} = string.Empty;

    [Required(ErrorMessage = "Contact Required")]
    [RegularExpression(@"^[a-z0-9.]{4}\/[a-z0-9]*\.[a-z0-9]*$|^[a-zA-Z0-9_=<>.%$]*$",ErrorMessage = "Not recognized as a valid contact address.")]
    [Display(Name="Contact Address")]
    public string ContactAddress{get;set;} = string.Empty;

    public ShippingOptions ShippingOption{get;set;} = ShippingOptions.STANDARD;
    public UserDTO user;

    public bool Authenticated {get;set;} = false;

}

public enum ShippingOptions
{
    [Display(Name="Standard Shipping - 50 C")]
    STANDARD,
    [Display(Name="Expedited Shipping - 200 C")]
    EXPEDITED,
    [Display(Name="Emergency Shipping - 1000 C")]
    EMERGENCY
}