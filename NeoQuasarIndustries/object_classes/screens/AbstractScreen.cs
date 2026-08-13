using System.Text.RegularExpressions;

namespace NeoQuasarIndustries;
public abstract class AbstractScreen
{
    protected bool Sidebar {get;set;}
    protected static string SearchString = ""; // this needs to be carried between screens

    protected AbstractScreen()
    {
        Sidebar = false;
    }

    public void ToggleSidebar()
    {
        Sidebar = Sidebar ? false : true;
    }

    public void SearchCatalog(string input)
    {
        List<string> errors;
        Dictionary<string,List<string>> searchTerms = ParseSearch(input,out errors);
        // Dictionary will have several terms that look like
            // name --> :Condensor
            // p --> =150
            // manufacturer --> >EsterTech



        // name: or name= searches for names and descriptions. This is the default.
        // m:, m=, manufacturer:, or manufacturer= searches for manufacturers
        // p<, p=, p>, p<=, p>=, or all the same for price searches for price

        SearchString = ""; // This'll be where the screen tells someone what they just searched for
    } 

    public string DisplayUser(User user)
    {
        string userRole = user.UserRole switch
        {
            User.Role.USER => "U: ",
            User.Role.SHIPPER => "SR: ",
            User.Role.ADMIN => "A: ",
            _ => ""
        };
        return userRole + user.Username;
    }

    // errors is here so that when users see their search results, they know why certain terms weren't used
    // List<string> becaause if someone types p>50 p<200 they should be able to find results 50 < p < 200
    public Dictionary<string,List<string>> ParseSearch(string input,out List<string> errors)
    {
        Dictionary<string,List<string>> searchCats = new Dictionary<string, List<string>>();
        errors = new List<string>();

        // Take the search and split it into a pile of terms that look like m:EsterTech and price<1500
        bool errorFound = false;
        string errorCode = "";
        string[] searchTerms = termSplitter(input,out errorFound,out errorCode);
        errors.Add(errorCode);

        string pattern = @"[a-z]{1,}[:<>=(>=)(<=)]""[a-zA-Z0-9\s]{1,}"""; // Search term looks like m:"Ester Tech"
        string pattern2 = @"[a-z]{1,}[:<>=(>=)(<=)][a-zA-Z0-9]{1,}"; // Search term looks like m:EsterTech
        string alphanumericPattern = @"""*[a-zA-Z0-9]*""*"; // Search term looks like Adaptor

        foreach (string s in searchTerms)
        {
            if (Regex.IsMatch(s, pattern) || Regex.IsMatch(s,pattern2)) // m:"text"
            {
                s.Replace("\"",""); // no longer need the quotation marks
                string search = ""; // This will be the indicator
                string term = ""; // This will be the thing we actually search for
                for (int i = 0; i < s.Length; i++)
                {
                    if (!Regex.IsMatch(s[i].ToString(), alphanumericPattern))
                    {
                        search = s.Substring(0,i);
                        term = s.Substring(i); // We aren't using split here because I need to read this symbol later
                        break;
                    }
                }
                if (!searchCats.ContainsKey(search))
                {
                    searchCats.Add(search,new List<string>());
                }
                searchCats.GetValueOrDefault(search,new List<string>()).Add(term); // There's probably a better way to do that
            }
            else if (Regex.IsMatch(s, alphanumericPattern))
            {
                s.Replace("\"","");
                if (!searchCats.ContainsKey("name"))
                {
                    searchCats.Add("name",new List<string>());
                }
                searchCats.GetValueOrDefault("name",new List<string>()).Add(s);
            }
            else
            {
                errors.Add("Search term" + s + "ignored: Improper Format");
            }
        }

        return searchCats;
    }

    private string[] termSplitter(string input, out bool errorFound, out string error)
    {
        string[] intermediate = input.Split(' ');
        List<string> termList = new List<string>();

        // We don't want to split at spaces that were in the middle of a quotation, so we're rejoining those here.
        string storageString = "";
        string pattern = @"[a-zA-Z0-9:]*""[a-zA-Z0-9:]*";

        foreach (string s in intermediate)
        {
            if (Regex.IsMatch(s, pattern))
            {
                if (storageString == "")
                {
                    storageString += s + " ";
                }
                else
                {
                    storageString += s;
                    termList.Add(storageString);
                    storageString = "";
                }
            }
            else if (storageString != "")
            {
                storageString += s + " ";
            }
            else
            {
                termList.Add(s);
            }
        }

        if (storageString != "")
        {
            errorFound = true;
            error = $"Search term {storageString} ignored: Unclosed quotation marks.";
        }
        else
        {
            errorFound = false;
            error = "";
        }

        string[] output = new string[termList.Count];
        for (int i = 0; i < termList.Count; i++)
        {
            output[i] = termList[i];
        }

        return output;

    }

}