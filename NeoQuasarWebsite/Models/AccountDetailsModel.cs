namespace NeoQuasarWebsite.Models;
public class AccountDetailsModel
{
    public int UserID {get;set;} = 0;
    public List<Order> orderDatabase;
    public int OrderNumber {get;set;} = 0;
}