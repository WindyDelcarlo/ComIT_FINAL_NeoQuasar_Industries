using System.Text;
using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class StorefrontScreen : AbstractScreen
{
    
    public StorefrontScreen(User user): base(user)
    {
        
    }

    public string getSearchText()
    {
        return SearchString;
        // Showing 2 results for search: ~~~~~
    }

    public void arrangeSearchResults()
    {
        // This makes the grid resulting from the database query
    }
    
    public void addToLine(StockItem stockItem)
    {
        this.User.AddToCart(stockItem,1);
    }  

    public void removeFromLine(StockItem stockItem)
    {
        this.User.RemoveFromCart(stockItem,1);
    }

    public override string CreateBody()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<div><!-- Search Text--></div>");
        sb.Append("<div><!-- Search Results -->");
        sb.Append("    <div> <!-- Stock QTY -->");
        sb.Append("        <div><!-- Plus Button--></div>");
        sb.Append("        <div><!-- Number Box --></div>");
        sb.Append("        <div><!-- Minus Button --></div>");
        sb.Append("    </div>");
        sb.Append("</div>");
        return sb.ToString();
    }
}