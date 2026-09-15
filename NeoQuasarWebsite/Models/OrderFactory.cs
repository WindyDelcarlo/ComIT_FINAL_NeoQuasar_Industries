using System.Text;

namespace NeoQuasarWebsite.Models;

public static class OrderFactory
{
    public static Order Create(Order.Direction direction,UserModel user,List<LineItem> lineItems,Manufacturer? manufacturer)
    {
        string shipper;
        string id;
        string date = "Y0011M01D01";
        string comm;

        if (direction == Order.Direction.OUTGOING)
        {   
            int newIdNumber = Order.GetOrderDatabase().Count;
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

            Order order = new Order(Order.Direction.OUTGOING,user,id,date,lineItems,user.Name,user.Address,user.LocatorCode,comm);
            Order.DatabaseAdd(order);
            return order;
        }

        if (direction == Order.Direction.INCOMING)
        {
            if (manufacturer != null)
            {
                int newIdNumber = Order.GetOrderDatabase().Count;
                StringBuilder sb = new StringBuilder();
                sb.Append("PR");
                AddZeros(sb,newIdNumber);
                sb.Append(newIdNumber);
                id = sb.ToString();
                shipper = manufacturer.Name;
                comm = manufacturer.ContactAddress;
                Order order = new Order(Order.Direction.INCOMING,user,id,date,lineItems,shipper,comm);
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