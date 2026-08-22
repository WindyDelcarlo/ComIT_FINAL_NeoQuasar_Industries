using System.Text.RegularExpressions;

namespace NeoQuasarIndustries;

public class User
{

    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string Username {get;set;}
    public string SlingName {get;set;}
    public string StellarNet { get;set;}
    public string Address {get; set;}
    public List<PaymentDetails> PaymentDetails = new List<PaymentDetails>();
    public List<CartItem> Cart = new List<CartItem>();

    public Role UserRole {get;set;}


    public User(string firstName, string lastName, string username) : this(firstName, lastName, username, "", "", "", new List<PaymentDetails>())
    {
        
    }

    public User(string firstName, string lastName, string username, string dimAddress, string sling, string sN, List<PaymentDetails> paymentDetails)
    {
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Address = dimAddress;
        SlingName = sling;
        StellarNet = sN;
        PaymentDetails.AddRange(paymentDetails);
        UserRole = Role.USER;
    }

    public enum Role
    {
        USER,
        SHIPPER,
        ADMIN
    }
    
}