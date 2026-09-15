using System.Text;
using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class PendingOrderScreen : AbstractScreen
{
    private Order selectedOrder;
    private LineItem selectedLine;
    
    public PendingOrderScreen(User user): base(user)
    {
        int listSize = TempOrderDatabase.orderList.Count;
        if (listSize > 0){
            selectedOrder = TempOrderDatabase.orderList[listSize];
        }
        else {
            selectedOrder = OrderFactory.Create(Order.Direction.OUTGOING,User,[],null);
        }
        if (selectedOrder.LineItems.Count > 0)
        {
            selectedLine = selectedOrder.LineItems[0];
        }
        else
        {
            selectedLine = new LineItem(new StockItem("","","","",StockItem.StockCategory.MISC,"",0),0);
        }
    }
    
    private void ShowOutgoing()
    {
        
    }
    
    private void ShowIncoming()
    {
        
    }

    private void NewReceipt()
    {
        
    }

    private void SubmitReceipt()
    {
        Receipt receipt = ReceiptFactory.Create(selectedOrder);
        TempReceiptDatabase.receiptList.Add(receipt);
        foreach (LineItem li in selectedOrder.LineItems)
        {
            li.BackOrdered = selectedOrder.GetOutstandingLine(li);
        }
    }

    private void SaveChanges()
    {
        
    }

    private LineItem GetListItemDetails()
    {
        StockItem tempItem = new StockItem("","","","",StockItem.StockCategory.MISC,"",1);
        LineItem item = new LineItem(tempItem,1);
        return item;
    }

    private void ModifyLineItem()
    {
        
    }

    public override void ConsoleClick(out bool running)
    {
        running = true;
        List<string> buttons = new List<string>
        {
            "Sidebar", "Enter Courier", "Enter Tracking Number", "Select Line", "Enter Line", "Select Order", "Submit Receipt", "Cancel"
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
                    string courier = Console.ReadLine() ?? "";
                    selectedOrder.Courier = courier;
                    break;
                }
            case 2:
                {
                    string tracking = Console.ReadLine() ?? "";
                    selectedOrder.TrackingNumber = tracking;
                    break;
                }
            case 3:
                {
                    selectedLine = SelectLine(selectedOrder);
                    break;
                }
            case 4:
                {
                    int received;
                    string intReceived = Console.ReadLine() ?? "";
                    int.TryParse(intReceived, out received);
                    if (received >= 0)
                    {
                        selectedLine.Shipped = received;
                    }
                    break;
                }
            case 5:
                {
                    Console.WriteLine("Enter the order number you would like to open:");
                    string id = Console.ReadLine() ?? "";
                    foreach (Order o in TempOrderDatabase.orderList)
                    {
                        if (o.ID == id)
                        {
                            selectedOrder = o;
                            break;
                        }
                    }
                    break;
                }
            case 6:
                {
                    try
                    {  
                        SubmitReceipt();
                        running = false;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                }
            default:
                {
                    running = false;
                    break;
                }
        }

    }

    public LineItem SelectLine(Order order)
    {   
        bool running = true;    
        int lineNumbers = order.LineItems.Count;
        int page = 0;
        int pageCount = (lineNumbers-1)/7;
        while (running){
            List<string> buttons = new List<string>();
            for (int i = 7*page; i < 7*(1+page); i++)
            {
                if (i < lineNumbers)
                {
                    buttons.Add(order.LineItems[i].Item.Name);
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
                case 0 or 1 or 2 or 3 or 4 or 5 or 6: 
                {
                        if (7*page + click < lineNumbers)
                        {
                            return order.LineItems[7*page+click];
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
                        return selectedLine;
                    }
            }
        }
        throw new Exception("Line item selection failed, somehow.");
    }

    public override void ConsoleDisplay()
    {
        List<string> orders = new List<string>();
        List<string> info = new List<string>();
        StringBuilder sb = new StringBuilder();
        foreach (Order o in TempOrderDatabase.orderList)
        {
            orders.Add(o.ID);
            if (o.ShippingDirection == Order.Direction.OUTGOING)
            {
                orders.Add(o.Recipient);
            }
            else
            {
                orders.Add(o.Shipper);    
            }
            orders.Add(ConsoleLibrary.Line("-",10));
        }
        info.Add(selectedOrder.ID);
        info.Add(selectedOrder.Date);
        if (selectedOrder.ShippingDirection == Order.Direction.OUTGOING)
        {
            info.Add("Recipient: " + selectedOrder.Recipient);
            info.Add("Address:" + selectedOrder.ShippingAddress);
            info.Add("LOC Code:" + selectedOrder.LocationCode);
        }
        else
        {
            info.Add("Shipper: " + selectedOrder.Shipper);
        }
        info.Add("Comm Address:" + selectedOrder.ContactAddress);
        info.Add("Courier: " + selectedOrder.Courier);
        info.Add("Tracking No: " + selectedOrder.TrackingNumber);
        info.Add("| RECD. | ORDER | BORDR | NEO SKU | DESCRIPTIN |");
        for (int i = 0; i < info.Count; i++)
        {
            sb.Append("| ");
            sb.Append(ConsoleLibrary.VariableSpacing(info[i], "", 48));
            sb.Append(" |");
            if (orders.Count >= info.Count - 1)
            {
                if (orders[i] == selectedOrder.ID)
                {
                    sb.Append(">");
                } 
                else
                {
                    sb.Append(" ");
                }
                sb.Append(ConsoleLibrary.CenterText(orders[i],8));
            }
            else
            {
                sb.Append(ConsoleLibrary.Line(" ",9));
            }
            sb.AppendLine("|");
        } 
        foreach (LineItem li in selectedOrder.LineItems)
        {
            if (selectedLine == li)
            {
                sb.Append("|>");
            }
            else
            {
                sb.Append("| ");
            }
            sb.Append(ConsoleLibrary.CenterText(li.Shipped.ToString(),5));
            sb.Append(" | ");
            sb.Append(ConsoleLibrary.CenterText(li.AmountOrdered.ToString(),5));
            sb.Append(" | ");
            sb.Append(ConsoleLibrary.CenterText(li.BackOrdered.ToString(),5));
            sb.Append(" | ");
            sb.Append(ConsoleLibrary.CenterText(li.Item.NeoSKU,7));
            sb.Append(" | ");
            if (li.Item.Description.Length < 10)
            {
                sb.Append(ConsoleLibrary.CenterText(li.Item.Description,10));
            }
            else
            {
                sb.Append(ConsoleLibrary.CenterText(li.Item.Description.Substring(0,10),10));
            }
            sb.Append(ConsoleLibrary.CenterText(li.Item.Description.Substring(0,10),10));
            sb.Append(" |");
            if (orders.Count >= info.Count - 1)
            {
                if (orders[info.Count] == selectedOrder.ID)
                {
                    sb.Append(">");
                }
                else
                {
                    sb.Append(" ");
                }
                sb.Append(ConsoleLibrary.CenterText(orders[info.Count],8));
            }
            else
            {
                sb.Append(ConsoleLibrary.Line(" ",9));
            }
            sb.AppendLine("|");
        }
        sb.AppendLine(ConsoleLibrary.Line("-", 80));
    }
}
