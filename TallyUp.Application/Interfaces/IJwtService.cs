using System.Security.Claims;

namespace TallyUp.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(string userId, string username, List<string> roles, List<string> permissions);
    ClaimsPrincipal? ValidateToken(string token);
}