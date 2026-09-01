using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public static class TempReceiptDatabase
{
    public static List<Receipt> receiptList = new List<Receipt>();

    public static void AddReceipt(Order order)
    {
        Receipt newReceipt = ReceiptFactory.Create(order);
        order.Receipts.Add(newReceipt);
        order.CheckCompletion();
        receiptList.Add(newReceipt);
    }
}