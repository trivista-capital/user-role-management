using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trivister.Core.Entities;

namespace Trivister.DataStore.DbConfigurations;

public class UsersRoleConfiguration: IEntityTypeConfiguration<UsersRole>
{
    public void Configure(EntityTypeBuilder<UsersRole> builder)
    {
        var userId = Guid.Parse("363b37a0-c306-4472-a405-4b576334cca0");
        var roleId = Guid.Parse("3e7d9440-48d7-4174-b9c5-0ea5be7d9e7d");
        var usersRole = UsersRole.Factory.Create(userId, roleId);
        builder.HasData(usersRole);
    }
}