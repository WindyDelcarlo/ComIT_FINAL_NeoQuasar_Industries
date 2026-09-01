using System.Diagnostics.Contracts;

namespace NeoQuasarIndustries;

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

}