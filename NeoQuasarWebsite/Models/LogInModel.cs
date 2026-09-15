using System.ComponentModel.DataAnnotations;

public class LogInModel
{
    [Required()]
    [Display(Name = "Username")]
    public string Username {get;set;} = string.Empty;

    [Required()]
    [Display(Name = "Password")]
    public string Password {get;set;} = string.Empty;
    
    [Display(Name="Remember Me")]
    public bool RememberMe {get;set;}
}