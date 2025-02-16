using System.Security.Claims;

namespace TallyUp.Application.Interfaces;

public interface IPermissionService
{
    bool HasPermission(ClaimsPrincipal user, string permission);
    List<string> GetPermissionsForRoles(List<string> roles);
}
