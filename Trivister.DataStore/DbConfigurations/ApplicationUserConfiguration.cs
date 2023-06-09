using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trivister.Core.Entities;

namespace Trivister.DataStore.DbConfigurations;

public class ApplicationUserConfiguration: IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        var userId = Guid.Parse("363b37a0-c306-4472-a405-4b576334cca0");
        var appuser = ApplicationUser.Factory.Create(userId, "Babafemi", "Oluwaseyi", "Ibitolu", 
            "femi.ibitolu@gmail.com", "08122310370", "Ajah");
        appuser.EmailConfirmed = false;
        appuser.NormalizedUserName = "femi.ibitolu@gmail.com".ToUpper();
        appuser.Address = "No 1 Jango street, wild wild west, Texas";
        appuser.RoleId = 1;
        //set user password
        PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
        appuser.PasswordHash = ph.HashPassword(appuser, "Password@123");
        builder.Property(x => x.FirstName).HasColumnType("nvarchar(300)").IsRequired(true);
        builder.Property(x => x.MiddleName).HasColumnType("nvarchar(300)").IsRequired(false);
        builder.Property(x => x.LastName).HasColumnType("nvarchar(300)").IsRequired(true);
        builder.Property(x => x.Address).HasColumnType("nvarchar(500)").IsRequired(false);
        builder.Property(x => x.Email).HasColumnType("nvarchar(300)").IsRequired(true);
        builder.Property(x => x.PhoneNumber).HasColumnType("nvarchar(50)").IsRequired(false);
        builder.HasData(appuser);
    }
}