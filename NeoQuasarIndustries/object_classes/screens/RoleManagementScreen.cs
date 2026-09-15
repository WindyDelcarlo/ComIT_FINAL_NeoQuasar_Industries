using System.Text;

namespace NeoQuasarIndustries;

public class RoleManagementScreen : AbstractScreen
{
    private UserSorter.SortCategory SortCategory;
    private User selectedUser;

    
    public RoleManagementScreen(User user): base(user)
    {
        selectedUser = TempUserDatabase.userList[0];
    }

    public List<User> DisplayList(string? search)
    {
        List<User> userList = [..TempUserDatabase.userList];
        List<User> userListDupe = [.. userList];
        if (search != null){
            foreach (User u in userList)
            {
                if (!u.Name.Contains(search) 
                && !u.Username.Contains(search) 
                && !u.StellarNet.Contains(search)
                && !u.SlingName.Contains(search))
                {
                    userListDupe.Remove(u);
                }
            }
        }
        userListDupe.Sort(new UserSorter(SortCategory));
        return userListDupe;
    }

    public List<User> DisplayList()
    {
        return DisplayList(null);
    }

    public void SetSortCategory(int sortCategory)
    {
        SortCategory = (UserSorter.SortCategory)sortCategory;
    }

    public void SetRole(User user, User.Role role)
    {
        if (User.UserRole == User.Role.ADMIN){
            user.UserRole = role;
        }
    }

    public override void ConsoleClick(out bool running)
    {
        running = true;
        List<string> buttons = new List<string>
        {
            "Sidebar", "Select User", "Set User Role", "Cancel"
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
                    Console.WriteLine("Enter the User ID you would like to update roles for:");
                    string idString = Console.ReadLine() ?? "";
                    int id = -1;
                    int.TryParse (idString, out id);
                    if (id >= 0){}
                    foreach (User u in TempUserDatabase.userList)
                    {
                        if (u.ID == id)
                        {
                            selectedUser = u;
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
        StringBuilder sb = new StringBuilder();
        
        foreach (User u in DisplayList())
        {
            if (u == selectedUser)
            {
                sb.Append("|>");
            }
            else
            {
                sb.Append("| ");
            }
            sb.Append(ConsoleLibrary.CenterText(u.Name,16));
            sb.Append(" | ");
            sb.Append(ConsoleLibrary.CenterText(u.Username,16));
            sb.Append(" | ");
            if (u.StellarNet != "")
            {
                sb.Append(ConsoleLibrary.CenterText(u.StellarNet,20));
            }
            else
            {
                sb.Append(ConsoleLibrary.CenterText(u.SlingName,20));
            }
            sb.Append(" | ");
            sb.Append(ConsoleLibrary.CenterText(u.UserRole.ToString(),16));
        }
        Console.WriteLine(sb.ToString());
    }
}