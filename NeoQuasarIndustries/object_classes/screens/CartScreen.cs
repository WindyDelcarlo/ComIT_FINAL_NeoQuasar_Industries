using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries
{
    public class CartScreen : AbstractScreen
    {

        public void ShowItem(CartItem item)
        {

        }

        public void IncreaseAmount(CartItem item)
        {
            item.Quantity++;
        }

        public void DecreaseAmount(CartItem item)
        {
            item.Quantity--;
            if (item.Quantity <= 0)
            {
                User.Cart.Remove(item);
            }
        }

        public void ListCart()
        {
            foreach (CartItem item in User.Cart)
            {
                ShowItem(item);
            }
        }

        public int calcSubtotal()
        {
            int subtotal = 0;
            foreach(CartItem item in User.Cart)
            {
                subtotal += item.Item.Price * item.Quantity;
            }
            return subtotal;
        }

        public int calcTotal()
        {
            int total = calcSubtotal();
            List<Manufacturer> manufacturersChecked = new List<Manufacturer>();
            foreach (CartItem item in User.Cart)
            {
                total += TaxesHandler.ShippingByDimCode(item.Item.Manufacturer.DimCode);
                manufacturersChecked.Add(item.Item.Manufacturer);
            }
            total += TaxesHandler.ShippingByDimCode(TaxesHandler.DimensionToDimCode(User.Address));
            return total;
        }

        public void toCheckOut()
        {
            
        }


    }

}
