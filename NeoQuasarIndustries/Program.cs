using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

class Program
{
    static void Main(string[] args)
    {
        string searchTerm = "p>=5000 m:EsterTech Gorgon";
        
        List<string> searchTermList;
        List<StockItem> stockItems = SearchHandler.StockSearch(searchTerm, out searchTermList);
        Console.WriteLine("Search Text: " + string.Join(",",searchTermList));
        Console.WriteLine("------------");
        foreach (StockItem si in stockItems)
        {
            Console.WriteLine(si.ToString());
            Console.WriteLine("-----");
        }

        Console.ReadLine();
    }
}
