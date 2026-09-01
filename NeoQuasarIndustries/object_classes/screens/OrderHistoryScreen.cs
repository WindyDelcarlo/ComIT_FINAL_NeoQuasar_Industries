namespace NeoQuasarIndustries;

public class OrderHistoryScreen : AbstractScreen
{
    
    public OrderHistoryScreen(User user): base(user)
    {
        
    }
    private void CancelReceipt()
    {
        
    }

    private List<Receipt> SelectReceipt(string? id)
    {
        List<Receipt> list = new List<Receipt>();
        if (id != null)
        {
            foreach (Receipt r in TempReceiptDatabase.receiptList)
            {
                if (r.ID == id)
                {
                    list.Add(r);
                }
            }
        }
        if (list.Count == 0)
        {
            list.Add(TempReceiptDatabase.receiptList[TempReceiptDatabase.receiptList.Count]);
        }
        return list;
    }
}
