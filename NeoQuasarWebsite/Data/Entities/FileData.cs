using Microsoft.EntityFrameworkCore;

namespace NeoQuasarWebsite.Data;

[PrimaryKey(nameof(FileID))]
public class FileData
{
    public int FileID {get;set;}
    public string Name {get;set;} = string.Empty;
    public string MIMEType {get;set;} = string.Empty;
    public long Size {get;set;}
}