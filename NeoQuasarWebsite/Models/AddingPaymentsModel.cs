using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class AddingPaymentsModel
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



    public string SelectCategory(PaymentDetails pd)
    {
        return pd.CardType switch 
        {
            PaymentDetails.Type.STELLARNET_DEBIT => "StellarNet Debit",
            PaymentDetails.Type.CREDIT => "Credit",
            _ => "Requisitions Account"
        };
    }

    public List<SelectListItem> typeList = new List<SelectListItem>
    {
        new SelectListItem { Value = PaymentDetails.Type.STELLARNET_DEBIT.ToString(), Text = "StellarNet Debit" },
        new SelectListItem { Value = PaymentDetails.Type.CREDIT.ToString(), Text = "Credit" },
        new SelectListItem { Value = PaymentDetails.Type.REQUISITIONS_ACCOUNT.ToString(), Text = "Requisitions Account" }
    };
}