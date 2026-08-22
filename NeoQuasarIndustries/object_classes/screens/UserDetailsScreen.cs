namespace NeoQuasarIndustries
{
    public class UserDetailsScreen : AbstractScreen
    {
        public void changePassword()
        {
            
        }

        public void updateDetails()
        {
            
        }

        public void addPaymentDetail(PaymentDetails.Type type, string cardName, string cardNumber)
        {
            User.PaymentDetails.Add(new PaymentDetails(type,cardName,cardNumber));
        }
    }
}
