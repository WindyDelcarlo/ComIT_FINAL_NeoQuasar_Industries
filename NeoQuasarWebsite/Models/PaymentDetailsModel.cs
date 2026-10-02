using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public class PaymentDetailsModel
{
    public UserModel User {get;set;}
    public Type CardType {get;set;}
    public string CardName {get;set;}
    public string CardNumber {get;set;} // These card numbers may not be All Numbers, so they can't be ints

}