using Microsoft.AspNetCore.Authorization;

namespace PolicyManagement;

public sealed class AuthorizationPolicies
{
    public static AuthorizationPolicy CanCreateRole()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("permission", "CanCreateRole")
            .Build();
    }
    public static AuthorizationPolicy CanEditRole()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("permission", "CanEditRole")
            .Build();
    }
    public static AuthorizationPolicy CanDeleteRole()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim("permission", "CanDeleteRole")
            .Build();
    }
}