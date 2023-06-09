namespace Trivister.Core.Entities;

public class RolesPermission
{
    public Guid RoleId { get; set; }
    public int PermissionId { get; set; }
    
    public ApplicationRole Role { get; set; }
    public Permission Permission { get; set; }
}