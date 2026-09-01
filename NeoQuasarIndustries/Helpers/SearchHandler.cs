using System.Text.RegularExpressions;
using NeoQuasarIndustries.object_classes.database;
using static NeoQuasarIndustries.object_classes.database.StockItem;

namespace NeoQuasarIndustries;

public static class SearchHandler
{
        public static Dictionary<string,List<string>> ParseSearch(string input,out List<string> errors)
    {
        Dictionary<string,List<string>> searchCats = new Dictionary<string, List<string>>();
        errors = new List<string>();

        // Take the search and split it into a pile of terms that look like m:EsterTech and price<1500
        bool errorFound;
        string errorCode;
        string[] searchTerms = TermSplitter(input,out errorFound,out errorCode);
        errors.Add(errorCode);

        string pattern = @"[a-z]{1,}(:|<|>|=|>=|<=)""[a-zA-Z0-9\s]{1,}"""; // Search term looks like m:"Ester Tech"
        string pattern2 = @"[a-z]{1,}(:|<|>|=|>=|<=)[a-zA-Z0-9]{1,}"; // Search term looks like m:EsterTech
        string alphanumericPattern = @"""*[a-zA-Z0-9]*""*"; // Search term looks like Adaptor

        foreach (string s in searchTerms)
        {
            if (Regex.IsMatch(s, pattern) || Regex.IsMatch(s,pattern2)) // m:"text"
            {
                string sNew = s.Replace("\"",""); // no longer need the quotation marks
                string search = ""; // This will be the indicator
                string term = ""; // This will be the thing we actually search for
                for (int i = 0; i < sNew.Length; i++)
                {
                    // This was supposed to be a regex check, but for some frigging reason the alphanumeric regex doesn't frigging work here.
                    if (sNew[i] == ':' || sNew[i] == '<' || sNew[i] == '>' || sNew[i] == '=')
                    {
                        if (sNew[i+1] == '=')
                        {
                            i++;
                        }
                        search = sNew.Substring(0,i+1).ToLower();
                        term = sNew.Substring(i+1).ToLower(); // We aren't using split here because I need to read this symbol later
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
                string sNew = s.Replace("\"","");
                if (!searchCats.ContainsKey("name"))
                {
                    searchCats.Add("name",new List<string>());
                }
                searchCats.GetValueOrDefault("name",new List<string>()).Add(sNew.ToLower());
            }
            else
            {
                errors.Add("Search term" + s + "ignored: Improper Format");
            }
        }

        return searchCats;
    }

    private static string[] TermSplitter(string input, out bool errorFound, out string error)
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

    public static List<StockItem> StockSearch(string input, out List<string> searchTerms, out int resultCount)
    {
        searchTerms = new List<string>();
        List<StockItem> inputList = StockItem.dummyList();
        List<StockItem> outputList = StockItem.dummyList();
        List<string> errorList;
        var searchCats = ParseSearch(input, out errorList);

        foreach (StockItem si in inputList){
            foreach (string term in searchCats.Keys)
            {
                if (term == "name")
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            if (!searchTerms.Contains(term + ":" + s)){
                                searchTerms.Add(term + ":" + s);
                            }
                            if (!si.Name.ToLower().Contains(s) && !si.Description.ToLower().Contains(s) && outputList.Contains(si))
                            {
                                outputList.Remove(si);
                            }
                        }
                    }
                }
                else if (term == "manufacturer" || term == "m:" || term == "m=")
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            
                            if (!searchTerms.Contains(term + s)){
                                searchTerms.Add(term + s);
                            }
                            if (!si.Manufacturer.Name.ToLower().Contains(s) && si.Manufacturer.Abbreviation != s && outputList.Contains(si))
                            {
                                outputList.Remove(si);
                            }
                        }
                    }
                }
                else if (term == "sku:" || term == "sku=")
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            if (!searchTerms.Contains(term + s)){
                                searchTerms.Add(term + s);
                            }
                            if (!si.NeoSKU.ToLower().Contains(s) && !si.ManSKU.ToLower().Contains(s) && outputList.Contains(si))
                            {
                                outputList.Remove(si);
                            }
                        }
                    }
                }
                else if (term == "price=" || term == "price:" || term == "p=" || term == "p:")
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            if (!searchTerms.Contains(term + s)){
                                searchTerms.Add(term + s);
                            }
                            int price;
                            if (int.TryParse(s,out price)) {
                                if (si.Price != price && outputList.Contains(si))
                                {
                                    outputList.Remove(si);
                                }
                            }
                        }
                    }
                }
                else if (term == "price<" || term == "p<")
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            if (!searchTerms.Contains(term + s)){
                                searchTerms.Add(term + s);
                            }
                            int price;
                            if (int.TryParse(s,out price)) {
                                if (si.Price >= price && outputList.Contains(si))
                                {
                                    outputList.Remove(si);
                                }
                            }
                        }
                    }
                }
                else if (term == "price<=" || term == "p<=")
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            if (!searchTerms.Contains(term + s)){
                                searchTerms.Add(term + s);
                            }
                            int price;
                            if (int.TryParse(s,out price)) {
                                if (si.Price > price && outputList.Contains(si))
                                {
                                    outputList.Remove(si);
                                }
                            }
                        }
                    }
                }
                
                else if (term == "price>" || term == "p>")
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            if (!searchTerms.Contains(term + s)){
                                searchTerms.Add(term + s);
                            }
                            int price;
                            if (int.TryParse(s,out price)) {
                                if (si.Price <= price && outputList.Contains(si))
                                {
                                    outputList.Remove(si);
                                }
                            }
                        }
                    }
                }
                else if (term == "price>=" || term == "p>=")
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            if (!searchTerms.Contains(term + s)){
                                searchTerms.Add(term + s);
                            }
                            int price;
                            if (int.TryParse(s,out price)) {
                                if (si.Price < price && outputList.Contains(si))
                                {
                                    outputList.Remove(si);
                                }
                            }
                        }
                    }
                }
                
                else if (term == "category=" || term == "c=" || term == "cat="
                || term == "category:" || term == "c:" || term == "cat:")
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            if (!searchTerms.Contains(term + s)){
                                searchTerms.Add(term + s);
                            }
                            if (si.Category.ToString().ToLower().Replace("_"," ") != s)
                            {
                                outputList.Remove(si);
                            }
                        }
                    }
                }
                else
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            if (!searchTerms.Contains("Unknown term ignored: " + term + s)){
                            searchTerms.Add("Unknown term ignored: " + term + s);
                            }
                            
                        }
                    }
                }
            }
        }
        foreach (var e in errorList)
        {
            Console.WriteLine(e);
        }
        if (outputList.Count == 0)
        {
            Console.WriteLine("No Results Found");
        }

        resultCount = outputList.Count();
        return outputList;
    }
}