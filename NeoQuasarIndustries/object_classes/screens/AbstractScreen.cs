using System.Text;
using System.Text.RegularExpressions;

namespace NeoQuasarIndustries;
public abstract class AbstractScreen : AbstractHTMLGen, IConsoleScreen
{
    protected bool Sidebar {get;set;}
    public User User {get;set;}
    protected static string SearchString = ""; // this needs to be carried between screens

    protected AbstractScreen(User user)
    {
        Sidebar = false;
        User = user;
    }

    protected AbstractScreen() : this(new DefaultUser())
    {
        
    }

    public void ToggleSidebar()
    {
        Sidebar = Sidebar ? false : true;
    }

    public void SearchCatalog(string input)
    {
        List<string> errors;
        Dictionary<string,List<string>> searchTerms = SearchHandler.ParseSearch(input,out errors);
        // Dictionary will have several terms that look like
            // name --> :Condensor
            // p --> =150
            // manufacturer --> >EsterTech



        // name: or name= searches for names and descriptions. This is the default.
        // m:, m=, manufacturer:, or manufacturer= searches for manufacturers
        // p<, p=, p>, p<=, p>=, or all the same for price searches for price

        SearchString = ""; // This'll be where the screen tells someone what they just searched for
    } 

    public string DisplayUser()
    {
        string userRole = User.UserRole switch
        {
            User.Role.USER => "U: ",
            User.Role.SHIPPER => "SR: ",
            User.Role.ADMIN => "A: ",
            _ => ""
        };
        return userRole + User.Username;
    }

    // errors is here so that when users see their search results, they know why certain terms weren't used
    // List<string> becaause if someone types p>50 p<200 they should be able to find results 50 < p < 200

    public override string CreateHeader()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<div><!-- Top Buffer --></div>");
        sb.AppendLine("<div><!-- NeoQuasar Logo --></div>");
        sb.AppendLine("<div><!-- Sidebar Button --></div>");
        sb.AppendLine("<div><!-- User Bar -->");
        sb.AppendLine("    <div><!-- Logged In As --></div>");
        sb.AppendLine("    <div><!-- The rest --></div>");
        sb.AppendLine("</div>");
        sb.AppendLine("<div> <!-- Search Bar -->");
        sb.AppendLine("    <div><!-- Search Box --></div>");
        sb.AppendLine("    <div><!-- Search Button --></div>");
        sb.AppendLine("</div>");
        sb.AppendLine("    <div><!-- Sidebar -->");
        sb.AppendLine("    <div><!-- Account Details --></div>");
        sb.AppendLine("    <div><!-- Log Out --></div>");
        sb.AppendLine("    <div><!-- Home --></div>");
        if (User.UserRole == User.Role.ADMIN ||  User.UserRole == User.Role.SHIPPER){
        sb.AppendLine("    <div><!-- Pending Orders --></div>");
        sb.AppendLine("    <div><!-- Order History --></div>");
        }
        if (User.UserRole == User.Role.ADMIN) {
        sb.AppendLine("    <div><!-- Stock Management --></div>");
        sb.AppendLine("    <div><!-- User Management --></div>");
        }
        sb.AppendLine("    <div><!-- About Us --></div>");
        sb.AppendLine("    <div><!-- Contact Us --></div>");
        sb.AppendLine("</div>");
        sb.AppendLine("<div><!-- Bottom Buffer --></div>");

        return sb.ToString();
    }

    public override string CreateFooter()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div><!-- Bar along the bottom --></div>");
        return base.CreateFooter();
    }


    public abstract void ConsoleClick(out bool running);

    public virtual void ConsoleRun()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Enter anything to continue.");
            Console.ReadLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            ConsoleDisplay();
            ConsoleClick(out running);  
        }
    }
    public abstract void ConsoleDisplay();

    public void ConsoleSidebarClick()
    {
        List<string> buttons = new List<string> {"Home"};
        if (User.UserRole == User.Role.SHIPPER || User.UserRole == User.Role.ADMIN)
        {
            buttons.Add("Pending Orders");
            buttons.Add("Order History");
        }
        if (User.UserRole == User.Role.ADMIN)
        {
            buttons.Add("Stock Management");
            buttons.Add("Role Management");
        }
        if (User.ID == 0)
        {
            buttons.Add("Log In");
        }
        buttons.Add("Cancel");
        int click = ClickEmulator.Click(buttons);

        switch (click)
        {
            case 0:
                {
                    new StorefrontScreen(User).ConsoleRun();
                    break;
                }
            case 1:
                {
                    if (User.ID == 0)
                    {
                        new LogInScreen(User).ConsoleRun();
                    }
                    else {
                    new OrderHistoryScreen(User).ConsoleRun();
                    }
                    break;
                }
            case 2:
                {
                    new PendingOrderScreen(User).ConsoleRun();
                    break;
                }
            case 3:
                {
                    new StockManagementScreen(User).ConsoleRun();
                    break;
                }
            case 4:
                {
                    new RoleManagementScreen(User).ConsoleRun();
                    break;
                }
            default: break;
        }

    }
}