using Microsoft.EntityFrameworkCore;
using TallyUp.Domain.Entities;

namespace TallyUp.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Permission> Permissions { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
}