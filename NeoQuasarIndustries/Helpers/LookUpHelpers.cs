using System.Runtime;

namespace NeoQuasarIndustries;

public static class LookUpHelpers {
    public static Manufacturer ManufacturerByString(string name)
    {
        List<Manufacturer> list = Manufacturer.dummyManufacturers();
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

    public static User GetUser(string username, string password)
    {
        // Method should check Username and Password and see if they match.
        return new User("","","");
    }
    
}