using Microsoft.AspNetCore.Identity;

namespace TallyUp.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}