namespace NeoQuasarIndustries;

public static class TaxesHandler
{
    public static int TaxByDimCode(string dimCode)
    {
        return dimCode switch
        {
            "ven0" => 0,
            _ => throw new ArgumentException("No Matching DimCode Found") 
        };
    }

    public static int ShippingByDimCode(string dimCode)
    {
        return dimCode switch
        {
            "ven0" => 50,
            _ => throw new ArgumentException("No Matching DimCode Found") 
        };
    }

    public static string DimensionToDimCode(string dimension)
    {
        string dim = dimension.ToLower();
        return dimension switch
        {
            "venari" => "ven0",
            _ => throw new ArgumentException("No Matching DimCode Found.")
        };
    }

}