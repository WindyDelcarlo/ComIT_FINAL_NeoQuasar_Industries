using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class StockManagementScreen : AbstractScreen
{
    public List<StockItem> AllStockItems()
    {
        // this'll be a database lookup
        List<StockItem> list = StockItem.dummyList();
        return list;
    }
}