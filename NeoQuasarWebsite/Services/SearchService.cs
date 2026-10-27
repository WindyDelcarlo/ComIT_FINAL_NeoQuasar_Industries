using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;
using NeoQuasarWebsite.Models;

namespace NeoQuasarWebsite.Services;

public interface ISearchService
{
    public List<StockItemDTO> StockSearch(string input, out List<string> searchTerms, out int resultCount);
    public string StockSearchJSON(string input, out List<string> error);
    public List<StockItemDTO> NoFilterSearch();
}

public class SearchService : ISearchService
{
    private NeoQuasarDBContext database;

    public SearchService(NeoQuasarDBContext dBContext)
    {
        database = dBContext;
    } 
    public Dictionary<string,List<string>> ParseSearch(string input,out List<string> errors)
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

    private string[] TermSplitter(string input, out bool errorFound, out string error)
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

    public List<StockItemDTO> StockSearch(string input, out List<string> searchTerms, out int resultCount)
    {
        searchTerms = new List<string>();
        List<StockItemData> inputList = database.StockItems.ToList();
        List<StockItemData> outputList = [..database.StockItems.ToList()];
        List<string> errorList;
        var searchCats = ParseSearch(input, out errorList);

        foreach (StockItemData si in inputList){
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
                else if (term == "manufacturer:" || term == "manufacturer:" || term == "m:" || term == "m=")
                {
                    List<string>? intermediateList = searchCats.GetValueOrDefault(term,null);
                    if (intermediateList != null)
                    {
                        foreach (string s in intermediateList)
                        {
                            
                            if (!searchTerms.Contains(term + s)){
                                searchTerms.Add(term + s);
                            }
                            if (!database.Manufacturers.FirstOrDefault(m => m.ManufacturerID == si.ManufacturerID).Name.ToLower().Contains(s) && database.Manufacturers.FirstOrDefault(m => m.ManufacturerID == si.ManufacturerID).Abbreviation != s && outputList.Contains(si))
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
                            if (!si.NeoSKU.ToLower().Contains(s) && !si.ManufacturerSKU.ToLower().Contains(s) && outputList.Contains(si))
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
                            if (si.StockCategory.ToString().ToLower().Replace("_"," ") != s)
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

        List<StockItemDTO> finalOutput = new List<StockItemDTO>();
        foreach(StockItemData sid in outputList)
        {
            int stock = sid.Stock;
            // This accounts for orders that haven't been shipped yet, which won't have had their numbers removed from the system.
            // Ideally customers don't think therre's stock when really there isn't.
            foreach(OrderLineData old in database.OrderLines.Where(ol => ol.ItemSKU == sid.NeoSKU))
            {
                stock -= old.AmountOrdered - old.Shipped;
            }
            // No reason to show a negative number to the customer, as amusing as it would be to see that there are -700 star plushies in the system.
            if (stock < 0)
            {
                stock = 0;
            }

            finalOutput.Add(new StockItemDTO
            {
                Name = sid.Name,
                Image = sid.ImageLocation,
                NeoSKU = sid.NeoSKU,
                Description = sid.Description,
                ManufacturerName = database.Manufacturers.FirstOrDefault(m => m.ManufacturerID == sid.ManufacturerID).Name,
                ManufacturerSKU = sid.ManufacturerSKU,
                Price = sid.Price,
                Stock = stock,
                NoRestock = sid.DontRestock
            });
        }
        finalOutput.RemoveAll(si => si.NoRestock && si.Stock <= 0);
        resultCount = finalOutput.Count();
        return finalOutput;
    }

    public List<StockItemDTO> NoFilterSearch()
    {
        List<StockItemDTO> stockList = new List<StockItemDTO>();
            foreach (StockItemData sid in database.StockItems.ToList())
            {
                int stock = sid.Stock;
                // This accounts for orders that haven't been shipped yet, which won't have had their numbers removed from the system.
                // Ideally customers don't think therre's stock when really there isn't.
                foreach(OrderLineData old in database.OrderLines.Where(ol => ol.ItemSKU == sid.NeoSKU))
                {
                    stock -= old.AmountOrdered - old.Shipped;
                }
                // No reason to show a negative number to the customer, as amusing as it would be to see that there are -700 star plushies in the system.
                if (stock < 0)
                {
                    stock = 0;
                }

                stockList.Add(new StockItemDTO
                {
                Name = sid.Name,
                Image = sid.ImageLocation,
                NeoSKU = sid.NeoSKU,
                Description = sid.Description,
                ManufacturerName = database.Manufacturers.FirstOrDefault(m => m.ManufacturerID == sid.ManufacturerID).Name,
                ManufacturerSKU = sid.ManufacturerSKU,
                Price = sid.Price,
                Stock = stock,
                NoRestock = sid.DontRestock
                });
            }
            stockList.RemoveAll(si => si.NoRestock && si.Stock <= 0);
            return stockList;
    }

    public string StockSearchJSON(string input, out List<string> error)
    {
        int resultCount;
        List<StockItemDTO> stockItems = StockSearch(input,out error, out resultCount);



        return JsonSerializer.Serialize(stockItems);
    }
}