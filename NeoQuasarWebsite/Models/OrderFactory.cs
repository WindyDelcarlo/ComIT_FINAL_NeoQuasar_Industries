using System.Text;
using NeoQuasarWebsite.Data;

namespace NeoQuasarWebsite.Models;

public static class OrderFactory
{
    public static Order Create(Order.Direction direction,int userID,List<LineItem> lineItems,ManufacturerData? manufacturer, ShippingOptions options)
    {
        string shipper;
        string id;
        string date = "Y0011M01D01";
        string comm;

        if (direction == Order.Direction.OUTGOING)
        {   
            UserModel user = new UserModel(UserModel.GetDatabase().Find(userID));
            int newIdNumber = Order.GetDatabase().ToList().Count;
            StringBuilder sb = new StringBuilder();
            sb.Append("PO");
            AddZeros(sb,newIdNumber);
            sb.Append(newIdNumber);
            id = sb.ToString();
            if (user.StellarNet != "")
            {
                comm = user.StellarNet;
            }
            else
            {
                comm = user.SlingName;
            }

            Order order = new Order(Order.Direction.OUTGOING,userID,id,date,user.Name,user.Address,user.LocatorCode,comm,options);
            Order.DatabaseAdd(order);
            foreach (LineItem li in lineItems)
            {
                LineItem.AddOrderDatabase(id,li);
            }
            return order;
        }

        if (direction == Order.Direction.INCOMING)
        {
            if (manufacturer != null)
            {
                int newIdNumber = Order.GetDatabase().ToList().Count();
                StringBuilder sb = new StringBuilder();
                sb.Append("PR");
                AddZeros(sb,newIdNumber);
                sb.Append(newIdNumber);
                id = sb.ToString();
                shipper = manufacturer.Name;
                comm = manufacturer.ContactAddress;
                Order order = new Order(Order.Direction.INCOMING,userID,id,date,shipper,comm);
                Order.DatabaseAdd(order);
                return order;
            }
            else throw new Exception("Manufacturer is required for incoming shipments.");
        }

        throw new ArgumentException("Order needs to be either coming or going.");
    }

    private static void AddZeros(StringBuilder sb, int newIdNumber){
        if (newIdNumber / 10000 < 1)
            {
                sb.Append("0");
            }
            if (newIdNumber / 1000 < 1)
            {
                sb.Append("0");
            }
            if (newIdNumber / 100 < 1)
            {
                sb.Append("0");
            }
            if (newIdNumber / 10 < 1)
            {
                sb.Append("0");
            }
    }
}