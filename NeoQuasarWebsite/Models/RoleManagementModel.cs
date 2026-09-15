using Microsoft.AspNetCore.Mvc.Rendering;

public class RoleManagementModel
{
    public List<UserModel> userList;
    public string SelectedRole;

    public string SelectRole(UserModel u)
    {
        return u.UserRole switch 
        {
            UserRole.USER => "user",
            UserRole.SHIPPER => "shipper",
            UserRole.ADMIN => "admin",
            _ => "none"
        };
    }

    public List<SelectListItem> roleList = new List<SelectListItem>
    {
        new SelectListItem { Value = UserRole.USER.ToString(), Text = "User" },
        new SelectListItem { Value = UserRole.SHIPPER.ToString(), Text = "Shipper" },
        new SelectListItem { Value = UserRole.ADMIN.ToString(), Text = "Admin" }
    };
}