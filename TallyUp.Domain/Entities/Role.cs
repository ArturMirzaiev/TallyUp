using Microsoft.AspNetCore.Identity;

namespace TallyUp.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}