using System.Text;

namespace NeoQuasarIndustries
{
    public class UserDetailsScreen : AbstractScreen
    {
        
        public UserDetailsScreen(User user): base(user)
        {
        
        }
        
        public bool Editing = false;
        public void changePassword()
        {
            
        }
        public void ToggleEditing()
        {
            Editing = !Editing;
        }

        public void addPaymentDetail(PaymentDetails.Type type, string cardName, string cardNumber)
        {
            PaymentDetails details = new PaymentDetails(type,cardName,cardNumber);
            bool check = PaymentDetails.ValidateCard(details); 
            if (check)
            {
                User.AddPaymentMethod(details);
            }
        }

        public void RemovePaymentDetail(PaymentDetails details)
        {
            User.RemovePaymentMethod(details);
        }

        public void ChangeSN(string sNName)
        {
            try {
            User.StellarNet = sNName;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Error changing sN Address: " + e);
            }
        }

        public void ChangeSling(string sling)
        {
            try {
            User.SlingName = sling;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Error changing Sling Name: " + e);
            }
        }

    }
}
