using System.ComponentModel.DataAnnotations;

namespace NeoQuasarWebsite.Models;

public class PendingOrdersModel{
    public List<Order> orderList = new List<Order>();
    public Order.Direction selectedDirection = Order.Direction.INCOMING;

    public string OrderID {get;set;}

    [Required]
    public string Courier {get;set;} = string.Empty;
    
    [Required]
    public string TrackingNumber {get;set;} = string.Empty;

    public int[] Received {get;set;}


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