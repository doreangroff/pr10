namespace pr10.Entitites;

public class Role
{
    public Role(int roleId, string roleName)
    {
        RoleId = roleId;
        RoleName = roleName;
    }

    public int RoleId { get; set; }
    public string RoleName { get; set; }
}