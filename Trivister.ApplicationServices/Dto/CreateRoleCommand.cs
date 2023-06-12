namespace Trivister.ApplicationServices.Dto;

public sealed record CreateRoleCommand(Guid Id, string RoleName, string Description);