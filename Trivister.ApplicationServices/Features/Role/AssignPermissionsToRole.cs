using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Exceptions;
using Trivister.Common.Model;

namespace Trivister.ApplicationServices.Features.Role;

public static class AssignPermissionsToRoleController
{
  public static void AssignPermissionsToRoleEndpoint(this WebApplication app)
  {
    app.MapPost("/assignPermissionToRole", async([FromBody]RolePermissionsDto model, IMediator mediator) =>
    {
        var result = await mediator.Send(new AssignPermissionsToRoleCommand(model));
       return Results.Ok(result);
    }).WithName("AssignPermissionToRole")
      .WithTags("Role Management")
      .Produces<ErrorResult<bool>>(StatusCodes.Status200OK)
      .Produces<ErrorResult<bool>>(StatusCodes.Status400BadRequest)
      .Produces<ErrorResult<bool>>(StatusCodes.Status500InternalServerError)
      .RequireCors("AllowSpecificOrigins");
  }
}

public sealed record RolePermissionsDto(Guid RoleId, List<PermissionsDto> Permissions);

public sealed record PermissionsDto(int Id, string  Name);

public sealed record AssignPermissionsToRoleCommand(RolePermissionsDto RolePermissions): IRequest<ErrorResult<bool>>;

public sealed class AssignPermissionsToRoleCommandHandler : IRequestHandler<AssignPermissionsToRoleCommand, ErrorResult<bool>>
{
  private readonly IIdentityService _identityService;
  public AssignPermissionsToRoleCommandHandler(IIdentityService identityService)
  {
    _identityService = identityService;
  }
  
  public async Task<ErrorResult<bool>> Handle(AssignPermissionsToRoleCommand request, CancellationToken cancellationToken)
  {
    if(!request.RolePermissions.Permissions.Any())
      throw new BadRequestException("No permission was selected. Please select at least one and try again");
    var result = await _identityService.AssignPermissionsToRole(request.RolePermissions.RoleId,
        request.RolePermissions.Permissions);
    if (!result.IsSuccess)
      throw new BadRequestException(result.Error);
    return result;
  }
}

