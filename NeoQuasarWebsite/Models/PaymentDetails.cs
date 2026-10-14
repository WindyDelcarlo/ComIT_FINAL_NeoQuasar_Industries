using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public class PaymentDetails
{
    public Type CardType {get;set;}
    public string CardName {get;set;}
    public string CardNumber {get;set;} // These card numbers may not be All Numbers, so they can't be ints

    public PaymentDetails (PaymentDetailData pd) : this((Type)Enum.Parse(typeof(Type),pd.CardType), pd.CardName, pd.CardNumber)
    {
        
    }
    public PaymentDetails (Type cardType, string cardName, string cardNumber)
    {
        CardType = cardType;
        CardName = cardName;
        CardNumber = cardNumber; 
    }

    public enum Type
    {
        STELLARNET_DEBIT,
        CREDIT,
        REQUISITIONS_ACCOUNT
    }

}