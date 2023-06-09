using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trivister.Core.Entities;

namespace Trivister.DataStore.DbConfigurations;

public class RoleConfiguration: IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        var roleId = Guid.Parse("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d");
        var role = ApplicationRole.Factory.Create("SuperAdmin", "");
        role.NormalizedName = "SUPERADMIN";
        role.Id = roleId;
        role.ConcurrencyStamp = Guid.NewGuid().ToString();
        
        var customerRoleId = Guid.Parse("0834d4fc-a976-4428-b6f8-d47b832fad1a");
        var customerRole = ApplicationRole.Factory.Create("Customer", "");
        customerRole.NormalizedName = "Customer";
        customerRole.Id = customerRoleId;
        customerRole.ConcurrencyStamp = Guid.NewGuid().ToString();

        var staffRoleId = Guid.Parse("bcf0f8de-c8c3-44ee-9c67-df972d604cf2");
        var staffRole = ApplicationRole.Factory.Create("Staff", "");
        staffRole.NormalizedName = "Staff";
        staffRole.Id = staffRoleId;
        staffRole.ConcurrencyStamp = Guid.NewGuid().ToString();
        
        builder.HasData(role);
        builder.HasData(customerRole);
        builder.HasData(staffRole);
        
        builder.Property(x => x.Name).HasColumnType("nvarchar(200)").IsRequired();
        builder.Property(x => x.Description).HasColumnType("nvarchar(400)");
    }
}