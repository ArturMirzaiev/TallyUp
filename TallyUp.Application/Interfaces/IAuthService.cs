using TallyUp.Domain.Models.Auth;

namespace TallyUp.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResult> LoginAsync(string username, string password);
    Task<AuthResult> RegisterAsync(string email, string password);
}