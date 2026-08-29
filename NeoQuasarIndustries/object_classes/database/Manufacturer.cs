namespace NeoQuasarIndustries;

public class Manufacturer
{
    private string abbreviation;
    public string Name {get;set;}
    public int ID {get;set;}
    public string DimCode{get;set;}
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

    public Manufacturer(string name, int id, string dimCode, string abbr)
    {
        Name = name;
        ID = id;
        DimCode = dimCode;
        abbreviation = abbr;
    }

    public static List<Manufacturer> dummyManufacturers()
    {
        List<Manufacturer> list = new List<Manufacturer>()
        {
          new Manufacturer("NeoQuasar",0,".nqi","NQ"), 
          new Manufacturer("Martin Industries", 1, "ven0","MI"),
          new Manufacturer("Stillmetal Laboratories",2,"oria","SL"),
          new Manufacturer("Corry Industrial",3,"ven0","CI"),
          new Manufacturer("EsterTech",4,"ven0","ET"),
          new Manufacturer("Silweave",5,"ven0","SW"),
          new Manufacturer("StarTamers",6,"tri0","ST"),
          new Manufacturer("Levin Starlight",7,"ark0","LS"),
          new Manufacturer("EnTech",8,".egd","ET"),
          new Manufacturer("Karisou Industries",9,"ven0","KI")  
        };

        return list;
    }

    public override string ToString()
    {
        return Name;
    }

}