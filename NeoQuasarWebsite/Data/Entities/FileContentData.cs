using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NeoQuasarWebsite.Data;

[PrimaryKey(nameof(ContentID))]
public class FileContentData
{
    public int ContentID {get;set;}

    [ForeignKey(nameof(FileID))]
    public int FileID {get;set;}
    public byte[] Content {get;set;} = Array.Empty<byte>();
}