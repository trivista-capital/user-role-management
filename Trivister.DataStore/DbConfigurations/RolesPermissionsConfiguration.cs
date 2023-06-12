using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trivister.Core.Entities;

namespace Trivister.DataStore.DbConfigurations;

public class RolesPermissionsConfiguration: IEntityTypeConfiguration<RolesPermission>
{
    public void Configure(EntityTypeBuilder<RolesPermission> builder)
    {
        builder.HasData(new RolesPermission()
            { 
                RoleId = Guid.Parse("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"), 
                PermissionId = 1
            },
            new { 
                RoleId = Guid.Parse("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"), 
                PermissionId = 2
            },
            new { 
                RoleId = Guid.Parse("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d"), 
                PermissionId = 3
            });
        builder.HasKey(e => new { e.RoleId, e.PermissionId });
    }
}