using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Core.Entities;

namespace Trivister.DataStore.Context;

public class IdpDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IGlobalTSDbContext
{
    public IdpDbContext(DbContextOptions<IdpDbContext> options) : base(options) { }

    public DbSet<Permission>? Permissions { get; set; }
    public DbSet<UsersRole>? UsersRole { get; set; }
    public DbSet<RolesPermission>? RolesPermissions { get; set; }
    
    public DbSet<OTPStore>? OTPStore { get; set; }
    
    //public DbSet<OTPLogger>? OTPLogger { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}