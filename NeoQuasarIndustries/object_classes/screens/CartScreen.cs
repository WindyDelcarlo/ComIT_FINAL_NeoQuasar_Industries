using System.Text;
using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries
{
    public class CartScreen : AbstractScreen
    {

        private CartItem? selectedItem;

        public CartScreen(User user): base(user)
        {
        
        }
        public void ShowItem(CartItem item)
        {

        }

        public void IncreaseAmount(CartItem item)
        {
            if (item != null){
                item.Quantity++;
            }
        }

        public void DecreaseAmount(CartItem item)
        {
            if (item != null)
            {
                item.Quantity--;
                if (item.Quantity <= 0)
                {
                    User.Cart.Remove(item);
                }
            }
        }

        public void ListCart()
        {
            foreach (CartItem item in User.Cart)
            {
                ShowItem(item);
            }
        }

        public int CalcSubtotal()
        {
            int subtotal = 0;
            foreach(CartItem item in User.Cart)
            {
                subtotal += item.Item.Price * item.Quantity;
            }
            return subtotal;
        }

        public int CalcTotal(out int shipping)
        {
            int total = CalcSubtotal();
            shipping = 0;
            List<Manufacturer> manufacturersChecked = new List<Manufacturer>();
            foreach (CartItem item in User.Cart)
            {
                shipping += TaxesHandler.ShippingByDimCode(item.Item.Manufacturer.DimCode);
                manufacturersChecked.Add(item.Item.Manufacturer);
            }
            shipping += TaxesHandler.ShippingByDimCode(TaxesHandler.LocCodeToDimCode(User.LocatorCode));
            total += shipping;
            return total;
        }

        public void SelectItem()
        {
            bool running = true;
            int lineNumbers = User.Cart.Count;
            int page = 0;
            int pageCount = (lineNumbers-1)/7;
            while (running){
                List<string> buttons = new List<string>();
                for (int i = 7*page; i < 7*(1+page); i++)
                {
                    if (i < lineNumbers)
                    {
                        buttons.Add(User.Cart[i].Item.Name);
                    }
                    else
                    {
                        buttons.Add("");
                    }
                }
                buttons.Add("Next Page");
                buttons.Add("Previous Page");
                buttons.Add("Cancel");

                int click = ClickEmulator.Click(buttons);
                switch (click)
                {
                    case 0 or 1 or 2 or 3 or 4 or 5 or 6: {
                        if (7*page + click < lineNumbers)
                            {
                                selectedItem = User.Cart[7*page+click];
                                running = false;
                            }
                            break;
                        }
                    case 7:
                        {
                            if(page < pageCount)
                            {
                                page++;
                            }
                            else
                            {
                                page = 0;
                            }
                            break;
                        }
                    case 8:
                        {
                            if (page > 0)
                            {
                                page--;
                            }
                            else
                            {
                                page = pageCount;
                            }
                            break;
                        }
                    case 9:
                        {
                            running = false;
                            break;
                        }
                }
            }
        }



        public override void ConsoleClick(out bool running)
        {
            running = true;
            List<string> buttons = new List<string>
            {
                "Sidebar", "Select Item", "Increase Item", "Decrease Item", "Check Out", "Back"
            };
            int click = ClickEmulator.Click(buttons);

            switch (click)
            {
                case 0: 
                    {
                        ConsoleSidebarClick();
                        break;
                    }
                case 1:
                    {
                        SelectItem();
                        break;
                    }
                case 2:
                    {   if (selectedItem != null){
                            IncreaseAmount(selectedItem);
                        }
                        break;
                    }
                case 3:
                    {
                        if (selectedItem != null){
                            DecreaseAmount(selectedItem);
                        }
                        break;
                    }
                case 4:
                    {
                        new CheckOutScreen(User).ConsoleRun();
                        running = false;
                        break;
                    }
                default:
                    {
                        running = false;
                        break;
                    }
            }
        }

        public override void ConsoleDisplay()
        {
            List<string> prices = new List<string>();
            List<string> items = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (CartItem ci in User.Cart)
            {
                items.Add(ConsoleLibrary.VariableSpacing(ci.Item.Name, ci.Quantity.ToString(), 48));
                items.Add(ConsoleLibrary.VariableSpacing(ci.Item.NeoSKU + " -- " + ci.Item.Category.ToString(),"",48));
                items.Add("------------------------------------------------");
                prices.Add(ci.Item.Price.ToString() + " C");
            }
            int subtotal = CalcSubtotal();
            int shipping;
            int total = CalcTotal(out shipping);

            for (int i = 0; i < prices.Count; i++)
            {
                if (User.Cart[i/3] == selectedItem)
                {
                    sb.Append("|>");
                }
                else
                {
                    sb.Append("| ");
                }
                sb.Append(items[i]);
                sb.Append(" |");
                sb.Append(ConsoleLibrary.CenterText(prices[i],9));
                sb.AppendLine("|");
            }
            for (int i = prices.Count; i < items.Count; i++)
            {
                if (User.Cart[i/3] == selectedItem)
                {
                    sb.Append("|>");
                }
                else
                {
                    sb.Append("| ");
                }
                sb.Append(ConsoleLibrary.VariableSpacing(items[i],"",48));
                sb.AppendLine(" |         |");
            }
            sb.AppendLine($"|{ConsoleLibrary.VariableSpacing("","Subtotal: ",50)}|{ConsoleLibrary.CenterText(subtotal + " C",9)}|");
            sb.AppendLine($"|{ConsoleLibrary.VariableSpacing("","Subtotal: ",50)}|{ConsoleLibrary.CenterText(shipping + " C",9)}|");
            sb.AppendLine($"|{ConsoleLibrary.VariableSpacing("","Subtotal: ",50)}|{ConsoleLibrary.CenterText(total + " C",9)}|");
            sb.AppendLine("----------------------------------------------------------------");
        }
    }


}