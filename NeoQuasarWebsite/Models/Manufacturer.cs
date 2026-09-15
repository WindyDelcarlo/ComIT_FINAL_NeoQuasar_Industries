using System.Diagnostics.Contracts;

namespace NeoQuasarWebsite.Models;

public class Manufacturer
{
    private string abbreviation;
    public string Name {get;set;}
    public int ID {get;set;}
    public string DimCode{get;set;}
    public string ContactAddress{get;set;}
    public string Abbreviation{get
        {
            return abbreviation;
        } 
        set
        {
            if (value.Length > 2)
            {
                value.Substring(0,2);
            }
            abbreviation = value.ToUpper();
        }
    }

    public Manufacturer(string name, int id, string dimCode, string abbr, string contact)
    {
        Name = name;
        ID = id;
        DimCode = dimCode;
        abbreviation = abbr;
        ContactAddress = contact;
    }

    public static List<Manufacturer> dummyManufacturers()
    {
        List<Manufacturer> list = new List<Manufacturer>()
        {
          new Manufacturer("NeoQuasar",0,".nqi","NQ",".nqi/neo.marianne"), 
          new Manufacturer("Martin Industries", 1, "ven0","MI","ven0/martin.erikas"),
          new Manufacturer("Stillmetal Laboratories",2,"oria","SL","FL-022-1334"),
          new Manufacturer("Corry Industrial",3,"ven0","CI","ven0/kari.ci.sarrik"),
          new Manufacturer("EsterTech",4,"ven0","ET","ven0/estert.mnevva"),
          new Manufacturer("Silweave",5,"ven0","SW","ven0/kari.sw.mordire"),
          new Manufacturer("StarTamers",6,"tri0","ST","StarTamers:LAmar281"),
          new Manufacturer("Levin Starlight",7,"ark0","LS","ark0/ryl.cormigni"),
          new Manufacturer("EnTech",8,".egd","ET","1.734.2233-001"),
          new Manufacturer("Karisou Industries",9,"ven0","KI","ven0/kari.virela")  
        };

        return list;
    }

    public override string ToString()
    {
        return Name;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        Manufacturer? man = obj as Manufacturer;
        if (man == null)
        {
            return false;
        }
        if (man.Name != Name)
        {
            return false;
        }
        else return true;
    }

    public static Manufacturer ManufacturerByString(string name)
    {
        List<Manufacturer> list = dummyManufacturers();
        // Method should get the manufacturer that matches the name
        foreach (Manufacturer m in list)
        {
            if (m.Name.ToLower() == name.ToLower())
            {
                return m;
            }
        }
        List<Manufacturer> hits = new List<Manufacturer>();
        foreach (Manufacturer m in list)
        {
            if (m.Abbreviation.ToLower() == name.ToLower())
            {
                hits.Add(m);
            }
        }

        if (hits.Count > 1)
        {
            throw new Exception("More than one manufacturer matches that abbreviation.");
        }
        else if (hits.Count < 1)
        {
            throw new Exception("No manufacturer matching that description found!");
        }
        else return hits[0];
    }
}