using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trivister.Core.Entities;

namespace Trivister.DataStore.DbConfigurations;

public class PermissionConfiguration: IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasData(new Permission() { 
                Name = "CanAddUser", 
                Description = "Can add user to the system", 
                Id = 1
            },
            new Permission(){ 
                Name = "CanDeleteUser", 
                Description = "Can delete user",
                Id = 2
            },
            new Permission(){ 
                Name = "CanEditUser", 
                Description = "Can edit user",
                Id = 3
            });
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnType("nvarchar(100)").IsRequired();
        builder.Property(x => x.Description).HasColumnType("nvarchar(400)");
    }
}