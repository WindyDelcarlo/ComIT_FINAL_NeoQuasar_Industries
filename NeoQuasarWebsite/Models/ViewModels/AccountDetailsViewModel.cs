using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;
public class AccountDetailsViewModel
{
    public int UserID {get;set;} = 0;
    public UserDTO User {get;set;}
    public List<OrderDTO> orderData;
    public List<PaymentDTO> paymentData;
    public int OrderNumber {get;set;} = 0;
}