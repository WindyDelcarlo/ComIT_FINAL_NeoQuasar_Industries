using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public static class TempOrderDatabase
{
    public static List<Order> orderList = new List<Order>();

    public static void AddOrder(Order.Direction direction, User user, List<LineItem> lineItems, Manufacturer? manufacturer)
    {
        orderList.Add(OrderFactory.Create(direction,user,lineItems,manufacturer));
    }
    
    public static void AddOrder(Order.Direction direction, User user, List<LineItem> lineItems)
    {
        AddOrder(direction,user,lineItems,null);
    }
}