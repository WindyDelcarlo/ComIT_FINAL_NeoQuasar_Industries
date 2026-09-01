namespace NeoQuasarIndustries;

public class RoleManagementScreen : AbstractScreen
{
    private UserSorter.SortCategory SortCategory;

    
    public RoleManagementScreen(User user): base(user)
    {
        
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
}