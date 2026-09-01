namespace NeoQuasarIndustries;

public static class ReceiptFactory
{
    public static Receipt Create(Order order)
    {
        string id;
        string? shipper = order.Shipper == "" ? null : order.Shipper;
        string? recipient = order.Recipient == "" ? null : order.Recipient;
        if (shipper == null)
        {
            id = "OUT" + TempReceiptDatabase.receiptList.Count;
        }
        else
        {
            id = "IN" + TempReceiptDatabase.receiptList.Count;
        }
        string date = DateHandler.GetDate();
        return new Receipt(id,order.ID,order.User,shipper,recipient,date,order.ShippingAddress,order.ContactAddress,order.LocationCode,order.Courier,order.TrackingNumber,order.LineItems);
    }
}