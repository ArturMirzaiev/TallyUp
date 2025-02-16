
namespace TallyUp.Domain.Models.Auth;

public class AuthResult
{
    public bool IsSuccess { get; }
    public string? Token { get; }
    public string? ErrorMessage { get; }

    private AuthResult(bool isSuccess, string? token = null, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        Token = token;
        ErrorMessage = errorMessage;
    }

    public static AuthResult Success(string token) => new AuthResult(true, token);
    public static AuthResult Success() => new AuthResult(true);
    public static AuthResult Failed(string errorMessage) => new AuthResult(false, errorMessage: errorMessage);
}