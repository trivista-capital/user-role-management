using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Core.Entities;

namespace Trivister.Infrastructure.IdentityConfig;

public sealed class ProfileService : IProfileService
{
    private readonly IGlobalTSDbContext _idpDbContext;
    private readonly IIdentityService _identityService;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(IGlobalTSDbContext idpDbContext, IIdentityService identityService, ILogger<ProfileService> logger)
    {
        _idpDbContext = idpDbContext;
        _identityService = identityService;
        _logger = logger;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        try
        {
            _logger.LogInformation("Getting the subject Id");
            var subjectId = context.Subject.GetSubjectId();
            _logger.LogInformation("Got the subject Id");
            var user = await _identityService.GetUserById(Guid.Parse(subjectId));
            _logger.LogInformation("Got the user Id");
            var role = await _identityService.GetUsersRole(Guid.Parse(subjectId));
            _logger.LogInformation("Got the users role");
            var permissions = await _idpDbContext!.RolesPermissions!
                              .Include(x=>x.Permission).Include(x=>x.Role)
                              .Where(x => x.RoleId == role.Id).ToListAsync();
            _logger.LogInformation("Got the permissions");
            _logger.LogInformation("Setting the claims");
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                new Claim("RoleName", role.Name),
                new Claim("RoleId", role.Id.ToString())
            };
            if (permissions.Any())
            {
                foreach (var permission in permissions)
                {
                    var claim = new Claim("permission", permission.Permission.Name);
                    context.IssuedClaims.Add(claim);
                }
            }
            context.IssuedClaims = claims;
            _logger.LogInformation("Set the claims");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while getting users claims in ProfileService class");
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _identityService.GetUserById(Guid.Parse(context.Subject.GetSubjectId()));
        var userIsEnabled = await _identityService.IsUserDisabled(user.UserName);
        context.IsActive = (!string.IsNullOrEmpty(user.UserName)) && userIsEnabled.Value;
    }
}