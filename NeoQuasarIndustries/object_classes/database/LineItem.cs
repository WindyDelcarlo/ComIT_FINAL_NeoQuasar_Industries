namespace NeoQuasarIndustries.object_classes.database;

public class LineItem
{
    public StockItem Item {get;}
    public int AmountOrdered {get;}

    public int BackOrdered {get;set;}

    public int Shipped {get;set;}

    public bool Completed {get;set;}

    public LineItem(StockItem item, int amountOrdered)
    {
        Item = item;
        AmountOrdered = amountOrdered;
        BackOrdered = 0;
        Shipped = 0;
        Completed = false;
    }

}