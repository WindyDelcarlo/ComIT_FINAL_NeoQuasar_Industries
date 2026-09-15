using System.Text;
using NeoQuasarIndustries.object_classes.database;

namespace NeoQuasarIndustries;

public class OrderHistoryScreen : AbstractScreen
{
    private Receipt selectedReceipt;
    
    public OrderHistoryScreen(User user): base(user)
    {
        if (TempReceiptDatabase.receiptList.Count > 0)
        {
            selectedReceipt = TempReceiptDatabase.receiptList[0];
        }
        else
        {
            selectedReceipt = new Receipt("","",user,"","","","","","","","",[]);
        }
    }

    private void CancelReceipt(Receipt receipt)
    {
        Order? order = null;
        foreach (Order o in TempOrderDatabase.orderList)
        {
            if (o.ID == receipt.OrderID)
            {
                order = o;
                break;
            }
        }
        if (order == null)
        {
            throw new Exception("Order not found for receipt: " + receipt.ID);
        }
        foreach (LineItem li in order.LineItems)
        {
            foreach (LineItem li2 in receipt.LineItems)
            {
                if (li.Item.NeoSKU == li2.Item.NeoSKU)
                {
                    li.BackOrdered += li2.Shipped;
                }
            }
        }

        TempReceiptDatabase.receiptList.Remove(receipt);
    }

    private List<Receipt> SelectReceipt(string? id)
    {
        List<Receipt> list = new List<Receipt>();
        if (id != null)
        {
            foreach (Receipt r in TempReceiptDatabase.receiptList)
            {
                if (r.ID == id)
                {
                    list.Add(r);
                }
            }
        }
        if (list.Count == 0)
        {
            list.Add(TempReceiptDatabase.receiptList[TempReceiptDatabase.receiptList.Count]);
        }
        return list;
    }

    public override void ConsoleClick(out bool running)
    {
        running = true;
        List<string> buttons = new List<string>
        {
            "Sidebar", "Select Order", "Cancel Receipt", "Cancel"
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
                    Console.WriteLine("Enter the receipt number you would like to open:");
                    string id = Console.ReadLine() ?? "";
                    foreach (Receipt r in TempReceiptDatabase.receiptList)
                    {
                        if (r.ID == id)
                        {
                            selectedReceipt = r;
                            break;
                        }
                    }
                    break;
                }
            case 2:
                {
                    try
                    {  
                        CancelReceipt(selectedReceipt);
                        if (TempReceiptDatabase.receiptList.Count > 0)
                        {
                            selectedReceipt = TempReceiptDatabase.receiptList[0];
                        }
                        else
                        {
                            selectedReceipt = new Receipt("","",User,"","","","","","","","",[]);
                        }
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

    public override void ConsoleDisplay()
    {
        List<string> orders = new List<string>();
        List<string> info = new List<string>();
        StringBuilder sb = new StringBuilder();
        foreach (Receipt r in TempReceiptDatabase.receiptList)
        {
            orders.Add(r.ID);
            if (r.Recipient != "")
            {
                orders.Add(r.Recipient);
            }
            else
            {
                orders.Add(r.Shipper);    
            }
            orders.Add(ConsoleLibrary.Line("-",10));
        }
        info.Add(selectedReceipt.ID);
        info.Add(selectedReceipt.Date);
        info.Add("Recipient: " + selectedReceipt.Recipient);
        info.Add("Address:" + selectedReceipt.ShippingAddress);
        info.Add("LOC Code:" + selectedReceipt.LocationCode);
        info.Add("Shipper: " + selectedReceipt.Shipper);
        info.Add("Comm Address:" + selectedReceipt.ContactAddress);
        info.Add("Courier: " + selectedReceipt.Courier);
        info.Add("Tracking No: " + selectedReceipt.TrackingNumber);
        info.Add("| RECD. | ORDER | BORDR | NEO SKU | DESCRIPTIN |");
        for (int i = 0; i < info.Count; i++)
        {
            sb.Append("| ");
            sb.Append(ConsoleLibrary.VariableSpacing(info[i], "", 48));
            sb.Append(" |");
            if (orders.Count >= info.Count - 1)
            {
                if (orders[i] == selectedReceipt.ID)
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
        foreach (LineItem li in selectedReceipt.LineItems)
        {
            sb.Append("| ");
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
                if (orders[info.Count] == selectedReceipt.ID)
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
