using System.Text;

namespace NeoQuasarIndustries;

public static class TaxesHandler
{
    public static double TaxByDimCode(string dimCode)
    {
        return dimCode switch
        {
            "ven0" => 0,
            "ark0" => 0.2,
            "oria" => 0.12,
            _ => throw new ArgumentException("No Matching DimCode Found") 
        };
    }

    public static int ShippingByDimCode(string dimCode)
    {
        return dimCode switch
        {
            "ven0" => 50,
            "ark0" => 70,
            "oria" => 750,
            ".tsa" => 200,
            _ => throw new ArgumentException("No Matching DimCode Found") 
        };
    }

    public static string LocCodeToDimCode(string locCode)
    {
        StringBuilder sb = new StringBuilder();
        foreach(char c in locCode)
        {
            if (c != '-')
            {
                sb.Append(c);
            }
            else
            {
                break;
            }
        }

        string dim = sb.ToString().ToLower();
        if (dim.Length == 3)
        {
            return "." + dim;
        } 
        else if (dim.Length == 4)
        {
            return dim;
        }
        else throw new ArgumentException("No Matching DimCode Found.");
    }

}