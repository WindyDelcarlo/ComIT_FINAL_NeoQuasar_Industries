using System.ComponentModel.DataAnnotations;
using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Models;

public class ManufacturersViewModel
{
    [Required]
    public string Name {get;set;}

    [Required]
    public string Abbreviation {get;set;}

    [Required]
    public string ContactAddress {get;set;}

    [Required]
    public string DimCode {get;set;}

    public ManufacturerDTO NewManufacturer()
    {
        return new ManufacturerDTO
        {
            Name = Name,
            Abbreviation = Abbreviation,
            ContactAddress = ContactAddress,
            DimCode = DimCode
        };
    }
}