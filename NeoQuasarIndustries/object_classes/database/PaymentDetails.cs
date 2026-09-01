using System.Text.RegularExpressions;

namespace NeoQuasarIndustries;
public class PaymentDetails
{
    public Type CardType {get;set;}
    public string CardName {get;set;}
    public string CardNumber {get;set;} // These card numbers may not be All Numbers, so they can't be ints

    public PaymentDetails (Type cardType, string cardName, string cardNumber)
    {
        CardType = cardType;
        CardName = cardName;
        CardNumber = cardNumber; 
    }

    public static bool ValidateCard(PaymentDetails details)
    {
        // Just checking to make sure the format is correct, because there's no database to check against
        string pattern = details.CardType switch
        {
            // two lowercase letters + 4 numbers + hyphen + any number of other numbers
            // example -- as1441-258
            Type.STELLARNET_DEBIT => @"^[a-z]{2}[0-9]{4}-[0-9]{2,}$",

            // 4-alphanumeric + colon + 10-Digit Number, possibly split up by whitespace
            // example -- ast0:123 2449 797
            Type.CREDIT => @"^[a-zA-Z0-9]{4}:(?:\s?[0-9]){10}$",

            // Any number of all-caps + hyphen + two all-caps + colon + numbers;
            // example -- RASTERA-ST:1223
            Type.REQUISITIONS_ACCOUNT => @"^[A-Z]{2,}-[A-Z]{2}:[0-9]+$",
            _ => "[^.]"
        };
        return Regex.IsMatch(details.CardNumber,pattern);    
    }

    public enum Type
    {
        STELLARNET_DEBIT,
        CREDIT,
        REQUISITIONS_ACCOUNT
    }
}