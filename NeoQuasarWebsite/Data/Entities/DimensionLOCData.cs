using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NeoQuasarWebsite.Data;

[PrimaryKey(nameof(LOCCode))]
public class DimensionLOCData
{
    public string LOCCode {get;set;} = string.Empty;

    [ForeignKey(nameof(DimensionData.ID))]
    public int DimensionID {get;set;}

}