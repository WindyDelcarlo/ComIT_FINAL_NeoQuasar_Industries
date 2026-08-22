namespace NeoQuasarIndustries
{
    public class CheckOutScreen : AbstractScreen
    {
        public void getPaymentDetails()
        {
            List<PaymentDetails> details = User.PaymentDetails;
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
            total *= TaxesHandler.TaxByDimCode(TaxesHandler.DimensionToDimCode(User.Address));
            return total;
        }
    
    }
}
