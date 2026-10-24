using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Services;

public interface IPaymentService
{
    public bool ProcessPayment(PaymentDTO paymentDto);
    public int TotalCost(UserDTO user, string shipping, string locCode);
    public double GetTax(string dimCode);
}

public class PaymentService : IPaymentService
{
    private readonly NeoQuasarDBContext database;
    
    public PaymentService(NeoQuasarDBContext dbContext)
    {
        database = dbContext;
    }

    public int TotalCost(UserDTO user, string shipping, string locCode)
    {
        int cost = 0;
        foreach (CartLineData line in user.Cart)
        {
            StockItemData? sid = database.StockItems.FirstOrDefault(si => si.NeoSKU == line.ItemSKU);
            if (sid != null)
            {
                cost += sid.Price * line.Quantity;
            }
        }
        cost += shipping switch
        {
            "Standard" => 50,
            "Expedited" => 200,
            "Emergency" => 1000,
            _ => 0
        };
        return (int) (cost * GetTax(locCode.Substring(0,4)));

    }

    public double GetTax(string dimCode)
    {
        return database.Dimensions.FirstOrDefault(d => d.DimCode == dimCode).ImportTax;
    }

    public bool ProcessPayment(PaymentDTO paymentDto)
    {
        Console.WriteLine("Dummy payment processed for " + paymentDto.Amount + " from method" + paymentDto.CardNumber);
        return true;
    }
}