using Microsoft.AspNetCore.Identity;

namespace TallyUp.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<Role> Roles { get; set; } = new List<Role>();
}