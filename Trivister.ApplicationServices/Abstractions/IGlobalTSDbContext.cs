using Microsoft.EntityFrameworkCore;
using Trivister.Core.Entities;

namespace Trivister.ApplicationServices.Abstractions;

public interface IGlobalTSDbContext
{
    DbSet<Permission>? Permissions { get; set; }
    DbSet<UsersRole>? UsersRole { get; set; }
    DbSet<RolesPermission>? RolesPermissions { get; set; }
    
    DbSet<OTPStore>? OTPStore { get; set; }
    // DbSet<ApplicationUser>? ApplicationUsers { get; set; }
    // DbSet<ApplicationRole>? ApplicationRoles { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}