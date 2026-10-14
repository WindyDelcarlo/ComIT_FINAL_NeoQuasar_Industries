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

}