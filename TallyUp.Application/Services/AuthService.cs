using Microsoft.AspNetCore.Identity;
using TallyUp.Application.Interfaces;
using TallyUp.Domain.Entities;
using TallyUp.Domain.Models.Auth;

namespace TallyUp.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;
    private readonly IPermissionService _permissionService;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService, IPermissionService permissionService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _permissionService = permissionService;
    }

    public async Task<AuthResult> LoginAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            return AuthResult.Failed("Invalid username or password");

        var roles = await _userManager.GetRolesAsync(user);
        var permissions = _permissionService.GetPermissionsForRoles(roles.ToList());

        var token = _jwtService.GenerateToken(user.Id.ToString(), user.UserName!, roles.ToList(), permissions);
        return AuthResult.Success(token);
    }

    public async Task<AuthResult> RegisterAsync(string email, string password)
    {
        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return AuthResult.Failed(errors);
        }

        await _userManager.AddToRoleAsync(user, "User");
        return AuthResult.Success();
    }
}