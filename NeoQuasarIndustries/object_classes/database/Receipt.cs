using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class Receipt
{
    private DIRECTION Direction {get;}
    private List<LineItem> LineItems{get;}
    private 
    enum DIRECTION
    {
        OUTGOING, INCOMING
    }
}