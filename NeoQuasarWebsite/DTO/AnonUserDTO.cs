namespace NeoQuasarWebsite.DTO;

public class AnonUserDTO {
    public string Key {get;set;} = string.Empty;
    public List<CartLineDTO> lines {get;set;} = new List<CartLineDTO>();
    public string Name {get;set;} = string.Empty;
    public string CardNumber {get;set;} = string.Empty;
    public string CardType {get;set;} = string.Empty;
    public string LocatorCode {get;set;} = string.Empty;
    public string Address {get;set;} = string.Empty;
    public string ContactAddress {get;set;} = string.Empty;
}