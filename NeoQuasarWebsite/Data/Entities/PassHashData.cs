using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Data;

public class PassHashData
{
    [Key]
    public int UserID {get;set;}
    public string PassHash {get;set;}

    public UserData User {get;set;}
}