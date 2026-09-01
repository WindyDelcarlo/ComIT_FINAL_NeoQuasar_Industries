namespace NeoQuasarIndustries;

public class DefaultUser : User
{
    public DefaultUser() : base(0,"Logged Out","LoggedOut","","","","",new List<PaymentDetails>(),User.Role.NONE)
    {
        
    }
}