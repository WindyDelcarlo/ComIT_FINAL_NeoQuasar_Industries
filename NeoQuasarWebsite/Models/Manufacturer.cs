using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore;
using NeoQuasarWebsite.Data;

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

    public Manufacturer(ManufacturerData md) : this(md.Name, md.ManufacturerID, md.DimCode, md.Abbreviation, md.ContactAddress)
    {
        
    }

    public Manufacturer(string name, int id, string dimCode, string abbr, string contact)
    {
        Name = name;
        ID = id;
        DimCode = dimCode;
        abbreviation = abbr;
        ContactAddress = contact;
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

    public ManufacturerData DatabaseThis()
    {
        return GetDatabase().FirstOrDefault(m => m.ManufacturerID == ID);
    }
    public static int ManufacturerByString(string name)
    {
        return GetDatabase().FirstOrDefault(m => m.Name == name || m.Abbreviation == name).ManufacturerID;
    }

    public static DbSet<ManufacturerData> GetDatabase()
    {
        return Program.Database().Manufacturers;
    }

    public static void DatabaseAdd(string name, string dimCode, string abbreviation,     string contactAddress)
    {
        string abb = abbreviation;
        if (abb.Length > 2)
        {
            abb = abb.Substring(0,2);
        }
        abb = abb.ToUpper();
        int id;
        if (GetDatabase().Count() > 0){
            id = GetDatabase().ToList().Last().ManufacturerID + 1;
        }
        else
        {
            id = 0;
        }
        GetDatabase().Add(new ManufacturerData
        {
            ManufacturerID = id,
            Name = name,
            Abbreviation = abb,
            DimCode = dimCode,
            ContactAddress = contactAddress
        });
        Program.Database().SaveChanges();
    }
}