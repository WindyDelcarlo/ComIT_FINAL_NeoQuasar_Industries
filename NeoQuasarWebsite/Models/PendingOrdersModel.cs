namespace NeoQuasarWebsite.Models;

public class PendingOrdersModel{
    public List<Order> orderList = new List<Order>();
    public Order selectedOrder;
    public Order.Direction selectedDirection = Order.Direction.INCOMING;


    public void ToggleDirection()
    {
        if (selectedDirection == Order.Direction.INCOMING)
        {
            selectedDirection = Order.Direction.OUTGOING;
        }
        else
        {
            selectedDirection = Order.Direction.INCOMING;
        }
    }

    public List<Order> GetOutgoingOrders()
    {
        List<Order> outgoingList = new List<Order>();
        foreach (Order o in orderList)
        {
            if (o.ShippingDirection == Order.Direction.OUTGOING && o.Outstanding)
            {
                outgoingList.Add(o);
            }
        }
        return outgoingList;
    }

    public List<Order> GetIncomingOrders()
    {
        
        List<Order> incomingList = new List<Order>();
        foreach (Order o in orderList)
        {
            if (o.ShippingDirection == Order.Direction.INCOMING && o.Outstanding)
            {
                incomingList.Add(o);
            }
        }
        return incomingList;
    }
}