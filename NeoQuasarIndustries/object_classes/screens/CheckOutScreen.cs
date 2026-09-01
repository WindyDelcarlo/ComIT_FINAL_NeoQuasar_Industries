using System.Text;
using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries
{
    public class CheckOutScreen : AbstractScreen
    {

        
        public CheckOutScreen(User user): base(user)
        {
        
        }

        public void ShowLine(CartItem item)
        {
            
        }

        public void ListCart()
        {
            foreach (CartItem item in User.Cart)
            {
                ShowLine(item);
            }
        }
        public List<PaymentDetails> GetPaymentDetails()
        {
            return User.PaymentDetailsList;
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

        public int CalcTotal(out int shipping, out int taxChange, out string tax)
        {
            shipping = 0;
            int total = CalcSubtotal();
            List<Manufacturer> manufacturersChecked = new List<Manufacturer>();
            foreach (CartItem item in User.Cart)
            {
                shipping += TaxesHandler.ShippingByDimCode(item.Item.Manufacturer.DimCode);
                manufacturersChecked.Add(item.Item.Manufacturer);
            }
            shipping += TaxesHandler.ShippingByDimCode(TaxesHandler.LocCodeToDimCode(User.LocatorCode));
            total += shipping;
            taxChange = (int)(total * TaxesHandler.TaxByDimCode(TaxesHandler.LocCodeToDimCode(User.LocatorCode)));
            total += taxChange;
            tax = $"Import Tax - {TaxesHandler.LocCodeToDimCode(User.LocatorCode)} ({TaxesHandler.TaxByDimCode(TaxesHandler.LocCodeToDimCode(User.LocatorCode))}%)";
            return total;
        }

        public void ProcessOrder()
        {
            List<LineItem> lineItems = new List<LineItem>();
            foreach (CartItem c in User.Cart)
            {
                lineItems.Add(new LineItem(c.Item,c.Quantity));
            }
            TempOrderDatabase.AddOrder(Order.Direction.OUTGOING,User,lineItems);
            User.Cart.Clear();
        }

        public void ChangeAddress()
        {
            Console.WriteLine("Enter new address (without the locator code):");
            string? address = Console.ReadLine();
            Console.WriteLine("Enter new locator code:");
            string? locCode = Console.ReadLine();
            try {
                User.ChangeAddress(address,locCode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ChangePaymentMethod()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Select A Payment Method:");
            List<string> buttons = ["Cancel", "Add New Card"];
            if (User.PaymentDetailsList.Count <= 8){
                foreach (PaymentDetails pd in User.PaymentDetailsList)
                {
                    buttons.Add($"{pd.CardType} - {pd.CardNumber} - {pd.CardName}");
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    PaymentDetails pd = User.PaymentDetailsList[i];
                    buttons.Add($"{pd.CardType} - {pd.CardNumber} - {pd.CardName}");
                }
            }

            int click = ClickEmulator.Click(buttons);
            switch (click)
            {
                
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        Console.WriteLine("Card Type?");
                        int click2 = ClickEmulator.Click(new List<string>{"Credit","Debit","Requisition"});
                        PaymentDetails.Type type;
                        switch (click2)
                        {
                            case 1:
                                {
                                    type = PaymentDetails.Type.STELLARNET_DEBIT;
                                    break;
                                }
                            case 2:
                                {
                                    type = PaymentDetails.Type.REQUISITIONS_ACCOUNT;
                                    break;
                                }
                            default:
                                {
                                    type = PaymentDetails.Type.CREDIT;
                                    break;
                                }
                        }
                        Console.WriteLine("Card Name?");
                        string cardName = Console.ReadLine() ?? "";
                        cardName.ToUpper();
                        Console.WriteLine("Card Number?");
                        string cardNumber = Console.ReadLine() ?? "";

                        try
                        {
                        User.AddPaymentMethod(new PaymentDetails(type,cardName,cardNumber));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }
                case 2 or 3 or 4 or 5 or 6 or 7 or 8 or 9: {
                    User.SelectPaymentMethod(click-2);
                    break;   
                }
            }
        }

        public void ChangeShippingMethod()
        {
            Console.WriteLine("Not available yet.");
        }


        public override void ConsoleClick(out bool running)
        {
            running = true;
            List<string> buttons = new List<string>
            {
                "Sidebar", "Change Address", "Change Payment Method", "Select Shipping", "Place Order", "Back"
            };
            while (running){
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
                            ChangeAddress();
                            break;
                        }
                    case 2:
                        {
                            ChangePaymentMethod();
                            break;
                        }
                    case 3:
                        {
                            ChangeShippingMethod();
                            break;
                        }
                    case 4:
                        {
                            ProcessOrder();
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
        }

        public override void ConsoleDisplay()
        {
    
            List<string> prices = new List<string>();
            List<string> lines = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (CartItem ci in User.Cart)
            {
                prices.Add(ci.Item.Price.ToString() + " C");
            }

            int subtotal = CalcSubtotal();
            int shipping;
            int taxChange;
            string tax;
            int total = CalcTotal(out shipping, out taxChange, out tax);
            string contactAddress = User.StellarNet == "" ? User.SlingName : User.StellarNet;

            lines.Add("Shipping Address: ");
            lines.Add(ConsoleLibrary.VariableSpacing(User.Name,User.LocatorCode,48));
            lines.Add(ConsoleLibrary.VariableSpacing(User.Address,contactAddress,48));
            lines.Add(ConsoleLibrary.Line("-",48));
            lines.Add("Payment Details:");
            if (User.PrimaryPaymentMethod != null)
            {
                lines.Add(User.PrimaryPaymentMethod.CardName);
                lines.Add(User.PrimaryPaymentMethod.CardNumber);
                lines.Add(User.PrimaryPaymentMethod.CardType.ToString());
            }
            else
            {
                lines.Add("No card found.");
                lines.Add("Please select or add a payment method.");
                lines.Add("");
            }
            lines.Add(ConsoleLibrary.Line("-",48));
            lines.Add("Shipping Options: ");
            lines.Add("- Interdim Parcel (50 C)");
            lines.Add(ConsoleLibrary.Line("=",48));
            
            for (int i = 0; i < prices.Count; i++)
            {
                sb.Append("| ");
                sb.Append(lines[i]);
                sb.Append(" |");
                sb.Append(ConsoleLibrary.CenterText(prices[i],9));
                sb.AppendLine("|");
            }
            for (int i = prices.Count; i < lines.Count; i++)
            {
                sb.Append("| ");    
                sb.Append(ConsoleLibrary.VariableSpacing(lines[i],"",48));
                sb.AppendLine(" |         |");
            }
            sb.AppendLine($"|{ConsoleLibrary.VariableSpacing("","Subtotal: ",50)}|{ConsoleLibrary.CenterText(subtotal + " C",9)}|");
            sb.AppendLine($"|{ConsoleLibrary.VariableSpacing("","Shipping: ",50)}|{ConsoleLibrary.CenterText(shipping + " C",9)}|");
            sb.AppendLine($"|{ConsoleLibrary.VariableSpacing("",tax + " ",50)}|{ConsoleLibrary.CenterText(taxChange + " C",9)}|");
            sb.AppendLine($"|{ConsoleLibrary.VariableSpacing("","Total: ",50)}|{ConsoleLibrary.CenterText(total + " C",9)}|");
            sb.AppendLine(ConsoleLibrary.Line("=",64));
        }
    }
}
