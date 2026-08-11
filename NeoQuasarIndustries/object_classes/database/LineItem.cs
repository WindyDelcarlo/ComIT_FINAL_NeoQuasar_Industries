namespace NeoQuasarIndustries.object_classes.database;

public class LineItem
{
    private StockItem Item {get;set;}
    private int AmountOrdered {get;}

    private int BackOrdered {get;set;}

    private int Shipped {get;set;}

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