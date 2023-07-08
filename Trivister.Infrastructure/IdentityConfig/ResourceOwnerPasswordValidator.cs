using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using IdentityModel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Features.Account;

namespace Trivister.Infrastructure.IdentityConfig;

public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly IGlobalTSDbContext _idpDbContext;
    private readonly IIdentityService _identityService;
    private readonly IMediator _mediator;

    public ResourceOwnerPasswordValidator(IMediator mediator, IIdentityService identityService, IGlobalTSDbContext idpDbContext)
    {
        _mediator = mediator;
        _identityService = identityService;
        _idpDbContext = idpDbContext;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var isValidated = await _mediator.Send(new LoginCommand(context.UserName, context.Password));
        var user = await _identityService.GetUserNameByUserNameAsync(context.UserName);
        var role = await _identityService.GetUsersRole(user.Id);
        if (isValidated.Item2.IsSuccess)
        {
            var permissions = await _idpDbContext!.RolesPermissions!.Include(x=>x.Role).Include(x=>x.Permission).Where(x => x.RoleId == role.Id).ToListAsync();
            if (permissions.Any())
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(ClaimTypes.Role, role?.Name)
                };
                foreach (var permission in permissions)
                {
                    var claim = new Claim("permission", permission.Permission.Name);
                    context.Request.ClientClaims.Add(claim);
                }
                foreach (var claim in claims)
                {
                    context.Request.ClientClaims.Add(claim);
                }   
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(ClaimTypes.Role, role?.Name)
                };
                foreach (var claim in claims)
                {
                    context.Request.ClientClaims.Add(claim);
                }      
            }
            context.Result = new GrantValidationResult(user.Id.ToString(), "password", null, "local", null);
            //return Task.FromResult(context.Result);
        }
        else
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The username and password do not match", null);       
        }
        //return Task.FromResult(context.Result);
    }
}