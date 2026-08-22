namespace NeoQuasarIndustries;

public class Manufacturer
{
    public string Name {get;set;}
    public int ID {get;set;}
    public string DimCode{get;set;}

    public Manufacturer(string name, int id, string dimCode)
    {
        Name = name;
        ID = id;
        DimCode = dimCode;
    }

}