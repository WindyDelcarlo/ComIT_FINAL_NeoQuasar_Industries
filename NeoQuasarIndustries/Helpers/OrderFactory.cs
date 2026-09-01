using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public static class OrderFactory
{
    public static Order Create(Order.Direction direction,User user,List<LineItem> lineItems,Manufacturer? manufacturer)
    {
        string shipper;
        string id;
        string date = DateHandler.GetDate();
        string comm;

        if (direction == Order.Direction.OUTGOING)
        {

            id = "PO" + TempOrderDatabase.orderList.Count;
            if (user.StellarNet != "")
            {
                comm = user.StellarNet;
            }
            else
            {
                comm = user.SlingName;
            }

            Order order = new Order(Order.Direction.OUTGOING,user,id,date,lineItems,user.Name,user.Address,user.LocatorCode,comm);
            TempOrderDatabase.orderList.Add(order);
            return order;
        }

        if (direction == Order.Direction.INCOMING)
        {
            if (manufacturer != null)
            {
                id = "PR" + TempOrderDatabase.orderList.Count;
                shipper = manufacturer.Name;
                comm = manufacturer.ContactAddress;
                Order order = new Order(Order.Direction.INCOMING,user,id,date,lineItems,shipper,comm);
                TempOrderDatabase.orderList.Add(order);
                return order;
            }
            else throw new Exception("Manufacturer is required for incoming shipments.");
        }

        throw new ArgumentException("Order needs to be either coming or going.");
    }
}