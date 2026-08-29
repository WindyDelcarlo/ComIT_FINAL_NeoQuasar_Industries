namespace NeoQuasarIndustries;

public class LogInScreen : AbstractScreen
{
    public void forgottenPassword()
    {   
    }

    private User LogIn(string username, string password, out bool success)
    {
        User user;
        try {
            user = LookUpHelpers.GetUser(username,password);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            success = false;
            return null;
        }
        success = true;
        return user;

    }

}