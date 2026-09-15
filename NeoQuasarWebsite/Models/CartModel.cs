namespace NeoQuasarWebsite.Models;

public class CartModel
{
    public UserModel user;

    public int CartTotal()
    {
        int i = 0;
        foreach (CartItem ci in user.Cart)
        {
            i += ci.Item.Price * ci.Quantity;
        }
        return i;
    }

    public double GetTax()
    {
        return 0;
    }
}