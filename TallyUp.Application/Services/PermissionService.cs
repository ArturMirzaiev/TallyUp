using System.Security.Claims;
using TallyUp.Application.Interfaces;

namespace TallyUp.Application.Services;

public class PermissionService : IPermissionService
{
    private static readonly Dictionary<string, List<string>> RolePermissions = new()
    {
        { "User", new List<string> { "can-read-poll" } },
        { "Moderator", new List<string> { "can-read-poll", "can-edit-poll", "can-create-poll" } },
        { "Admin", new List<string> { "can-read-poll", "can-edit-poll", "can-delete-poll" } }
    };
    
    public List<string> GetPermissionsForRoles(List<string> roles)
    {
        return roles.SelectMany(role => RolePermissions.GetValueOrDefault(role, new List<string>())).Distinct().ToList();
    }
    
    public bool HasPermission(ClaimsPrincipal user, string permission)
    {
        return user.Claims
            .Where(c => c.Type == "permissions")
            .Select(c => c.Value)
            .Contains(permission);
    }
}