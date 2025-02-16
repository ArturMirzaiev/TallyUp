using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TallyUp.Domain.Entities;

namespace TallyUp.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid>
{
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Poll> Polls { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<RolePermission>().ToTable("RolePermissions");
        builder.Entity<RolePermission>().HasKey(p => 
            new { p.RoleId, p.PermissionId });
    }
}