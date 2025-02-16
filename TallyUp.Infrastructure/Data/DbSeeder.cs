using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TallyUp.Domain.Entities;
using TallyUp.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            await SeedRolesAndPermissions(context, roleManager);
            await SeedUsers(userManager);

            await transaction.CommitAsync();
            Console.WriteLine("✅ Данные успешно засеяны!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Console.WriteLine($"❌ Ошибка при сидинге базы: {ex.Message}");
        }
    }

    private static async Task SeedRolesAndPermissions(ApplicationDbContext context, RoleManager<Role> roleManager)
    {
        // 1️⃣ Создаём роли, если их нет
        foreach (var role in RolePermissionsConfig.Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                var newRole = new Role { Name = role };
                var result = await roleManager.CreateAsync(newRole);
                if (!result.Succeeded)
                {
                    throw new Exception($"Ошибка при создании роли {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
        await context.SaveChangesAsync(); // 💾 Фиксируем роли

        // 2️⃣ Добавляем пермишены
        var existingPermissions = await context.Permissions.Select(p => p.Name).ToListAsync();
        var newPermissions = RolePermissionsConfig.Permissions.Except(existingPermissions)
            .Select(p => new Permission { Name = p }).ToList();

        if (newPermissions.Any()) await context.Permissions.AddRangeAsync(newPermissions);
        await context.SaveChangesAsync(); // 💾 Фиксируем пермишены

        // 3️⃣ Привязываем роли к пермишенам
        var roleEntities = await context.Roles.ToDictionaryAsync(r => r.Name, r => r.Id);
        var permissionEntities = await context.Permissions.ToDictionaryAsync(p => p.Name, p => p.Id);

        var existingRolePermissions = await context.RolePermissions
            .Select(rp => new { rp.RoleId, rp.PermissionId })
            .ToListAsync();

        var newRolePermissions = RolePermissionsConfig.RolePermissions
            .SelectMany(rp => rp.Value
                .Select(p => new RolePermission { RoleId = roleEntities[rp.Key], PermissionId = permissionEntities[p] }))
            .Where(rp => !existingRolePermissions.Any(e => e.RoleId == rp.RoleId && e.PermissionId == rp.PermissionId))
            .ToList();

        if (newRolePermissions.Any()) await context.RolePermissions.AddRangeAsync(newRolePermissions);
        await context.SaveChangesAsync(); // 💾 Фиксируем связи
    }

    private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
    {
        var users = new List<(ApplicationUser user, string password, string role)>
        {
            (new ApplicationUser { UserName = "Admin@123", Email = "admin@example.com", EmailConfirmed = true }, "Admin@123", "Admin"),
            (new ApplicationUser { UserName = "Moderator@123", Email = "moderator@example.com", EmailConfirmed = true }, "Moderator@123", "Moderator"),
            (new ApplicationUser { UserName = "User@123", Email = "user@example.com", EmailConfirmed = true }, "User@123", "User")
        };

        foreach (var (user, password, role) in users)
        {
            if (await userManager.FindByEmailAsync(user.Email) == null)
            {
                var createResult = await userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    throw new Exception($"Ошибка при создании пользователя {user.Email}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }

                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
