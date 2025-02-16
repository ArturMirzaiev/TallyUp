public static class RolePermissionsConfig
{
    public static readonly string[] Roles = { "Admin", "Moderator", "User" };
    public static readonly string[] Permissions = { "can-edit-poll", "can-read-poll", "can-delete-poll" };

    public static readonly Dictionary<string, string[]> RolePermissions = new()
    {
        { "Admin", Permissions },
        { "Moderator", new[] { "can-edit-poll", "can-read-poll" } },
        { "User", new[] { "can-read-poll" } }
    };
}