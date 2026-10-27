using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class CardsViewModel
{
    [Required]
    public string CardName{get;set;} = string.Empty;

    [Required]
    public string CardNumber{get;set;} = string.Empty;

    [Required]
    public string CardType{get;set;} = string.Empty;

    public int UserID {get;set;} = 0;
    public UserDTO User {get;set;} = new UserDTO{UserID = 0};
    public List<OrderDTO> orderData;
    public List<PaymentDTO> paymentData;
    public int OrderNumber {get;set;} = 0;

    public List<SelectListItem> typeList = new List<SelectListItem>
    {
        new SelectListItem { Value = "STELLARNET_DEBIT", Text = "StellarNet Debit" },
        new SelectListItem { Value = "CREDIT", Text = "Credit" },
        new SelectListItem { Value = "REQUISITIONS_ACCOUNT", Text = "Requisitions Account" }
    };
}