using System.Text.RegularExpressions;

namespace NeoQuasarIndustries;
public abstract class AbstractScreen
{
    protected bool Sidebar {get;set;}
    public User User {get;set;}
    protected static string SearchString = ""; // this needs to be carried between screens

    protected AbstractScreen()
    {
        Sidebar = false;
        User = new DefaultUser();
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


}