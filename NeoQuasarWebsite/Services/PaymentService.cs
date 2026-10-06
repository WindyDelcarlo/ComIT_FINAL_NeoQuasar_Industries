using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Services;

public interface IPaymentService
{
    public bool ProcessPayment(PaymentDTO paymentDto);
}

public class PaymentService : IPaymentService
{
    public bool ProcessPayment(PaymentDTO paymentDto)
    {
        Console.WriteLine("Dummy payment processed for " + paymentDto.Amount + "from method" + paymentDto.CardNumber);
        return true;
    }
}