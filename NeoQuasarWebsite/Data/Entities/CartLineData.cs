using System.ComponentModel.DataAnnotations;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Data;

public class CartLineData{
    
    [Key]
    public int CartLineID {get;set;}
    public int UserID{get;set;}
    public UserData User {get;set;}
    public string ItemSKU{get;set;} = string.Empty;
    public StockItemData LineItem {get;set;}
    public int Quantity{get;set;}
}