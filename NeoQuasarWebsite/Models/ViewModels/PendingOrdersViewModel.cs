using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NeoQuasarWebsite.Models;

public class PendingOrdersViewModel{
    public List<OrderModel> orderList = new List<OrderModel>();
    public OrderModel.Direction selectedDirection = OrderModel.Direction.INCOMING;

    public string OrderID {get;set;}

    [Required]
    public string Courier {get;set;} = string.Empty;
    
    [Required]
    public string TrackingNumber {get;set;} = string.Empty;
    
    public int[] Received {get;set;}


    public void ToggleDirection()
    {
        if (selectedDirection == OrderModel.Direction.INCOMING)
        {
            selectedDirection = OrderModel.Direction.OUTGOING;
        }
        else
        {
            selectedDirection = OrderModel.Direction.INCOMING;
        }
    }

    public List<OrderModel> GetOutgoingOrders()
    {
        List<OrderModel> outgoingList = new List<OrderModel>();
        foreach (OrderModel o in orderList)
        {
            if (o.ShippingDirection == OrderModel.Direction.OUTGOING && o.Outstanding)
            {
                outgoingList.Add(o);
            }
        }
        return outgoingList;
    }

    public List<OrderModel> GetIncomingOrders()
    {
        
        List<OrderModel> incomingList = new List<OrderModel>();
        foreach (OrderModel o in orderList)
        {
            if (o.ShippingDirection == OrderModel.Direction.INCOMING && o.Outstanding)
            {
                incomingList.Add(o);
            }
        }
        return incomingList;
    }

    public string PONumber()
    {
        return JsonSerializer.Serialize(new {
           orderID = OrderID 
        });
    }
}