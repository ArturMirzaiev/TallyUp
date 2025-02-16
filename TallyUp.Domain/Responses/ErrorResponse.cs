namespace TallyUp.Domain.Responses;

public class ErrorResponse
{
    public int StatusCode { get; }
    public string? SubCode { get; }
    public string Description { get; }

    public ErrorResponse(int statusCode, string? subCode = null, string description = "An error occurred")
    {
        StatusCode = statusCode;
        SubCode = subCode;
        Description = description;
    }
}