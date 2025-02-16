using System.Security.Claims;
using FluentAssertions;
using TallyUp.Application.Services;
using Xunit;

namespace TallyUp.Tests.Services;

public class PermissionServiceTests
{
    private readonly PermissionService _permissionService = new();

    [Fact]
    public void HasPermission_UserWithEditPollPermission_ShouldReturnTrue()
    {
        // Arrange
        var claims = new List<Claim> { new Claim("permissions", "can-edit-poll") };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = _permissionService.HasPermission(user, "can-edit-poll");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasPermission_UserWithoutEditPollPermission_ShouldReturnFalse()
    {
        // Arrange
        var claims = new List<Claim> { new Claim("permissions", "can-read-poll") };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = _permissionService.HasPermission(user, "can-delete-poll");

        // Assert
        result.Should().BeFalse();
    }

    [Fact] public void HasPermission_UserWithDeletePollPermission_ShouldReturnTrue()
    {
        // Arrange
        var claims = new List<Claim> { new Claim("permissions", "can-delete-poll") };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);

        // Act
        var result = _permissionService.HasPermission(user, "can-delete-poll");

        // Assert
        result.Should().BeTrue();
    }
}
