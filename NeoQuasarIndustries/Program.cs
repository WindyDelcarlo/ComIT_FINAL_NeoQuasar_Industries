using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

class Program
{
    static void Main(string[] args)
    {
        TempUserDatabase.Initialize();
        string searchTerm = "m:\"Martin Industries\" p<=10000";
        
        List<string> searchTermList;
        int count;
        List<StockItem> stockItems = SearchHandler.StockSearch(searchTerm, out searchTermList, out count);
        Console.WriteLine($"Search Text: {string.Join(",",searchTermList)} (Found {count} results.)");
        Console.WriteLine("------------");
        foreach (StockItem si in stockItems)
        {
            Console.WriteLine(si.ToString());
            Console.WriteLine("-----");
        }

        Console.ReadLine();
    }
}
