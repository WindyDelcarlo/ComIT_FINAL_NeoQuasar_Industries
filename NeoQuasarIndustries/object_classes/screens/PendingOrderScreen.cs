using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class PendingOrderScreen : AbstractScreen
{
    
    public PendingOrderScreen(User user): base(user)
    {
        
    }
    
    private void ShowOutgoing()
    {
        
    }
    
    private void ShowIncoming()
    {
        
    }

    private void NewReceipt()
    {
        
    }

    private void SubmitReceipt()
    {
        
    }

    private void SaveChanges()
    {
        
    }

    private LineItem GetListItemDetails()
    {
        StockItem tempItem = new StockItem("","","","",StockItem.StockCategory.MISC,"",1);
        LineItem item = new LineItem(tempItem,1);
        return item;
    }

    private void ModifyLineItem()
    {
        
    }
}
