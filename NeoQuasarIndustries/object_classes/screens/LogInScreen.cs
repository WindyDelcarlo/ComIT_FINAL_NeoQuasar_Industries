using System.Text;

namespace NeoQuasarIndustries;

public class LogInScreen : AbstractScreen
{
    private string ConsoleUsername = "";
    private string ConsolePassword = "";

    public LogInScreen(User user): base(user)
    {
        
    }

    public void ForgottenPassword()
    {   
    }

    private static User? LogIn(string username, string password)
    {
        User user;
        try {
            user = LookUpHelpers.UserLogIn(username,password);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
        return user;

    }

    public override void ConsoleClick(out bool running)
    {
        running = true;
        List<string> buttons = new List<string>
        {
            "Sidebar", "Enter Username", "Enter Password", "Log In", "Reset Password", "Back"
        };
        int click = ClickEmulator.Click(buttons);

        switch (click)
        {
            case 0: 
                {
                    ConsoleSidebarClick();
                    break;
                }
            case 1:
                {
                    Console.WriteLine("Enter username.");
                    string? usernameEntry = Console.ReadLine();
                    if (usernameEntry != null)
                    {
                        ConsoleUsername = usernameEntry;
                    }
                    break;
                }
            case 2:
                {
                    Console.WriteLine("Enter password.");
                    string? passwordEntry = Console.ReadLine();
                    if (passwordEntry != null)
                    {
                        ConsolePassword = passwordEntry;
                    }
                    break;
                }
            case 3:
                {
                    Console.WriteLine("Checking username against password...");
                    User? user = LogIn(ConsoleUsername,ConsolePassword);
                    if (user != null)
                    {
                        Console.WriteLine("Username and password confirmed. Logging in...");
                        User = user;
                        new StorefrontScreen(User).ConsoleRun();
                    }
                    break;
                }
            case 4:
                {
                    ForgottenPassword();
                    break;
                }
            default:
                {
                    running = false;
                    break;
                }
        }
    }

    public override void ConsoleDisplay()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("|" + ConsoleLibrary.CenterText($"USERNAME: {ConsoleUsername}",62) + "|");
        sb.AppendLine("|" + ConsoleLibrary.CenterText($"PASSWORD: {ConsoleUsername}",62) + "|");
    }

}