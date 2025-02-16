using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TallyUp.Application.Dtos;
using TallyUp.Application.Interfaces;
using TallyUp.Domain.Models.Auth;
using TallyUp.Domain.Responses;

namespace TallyUp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResult))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(AuthResult))]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request.Username, request.Password);

        return result.IsSuccess
            ? Ok(result)
            : Unauthorized(result);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request.Email, request.Password);

        return result.IsSuccess
            ? Ok(AuthResult.Success("User is registered!"))
            : BadRequest(result);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("test");
    }
}