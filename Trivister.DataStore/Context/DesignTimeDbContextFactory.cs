using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Trivister.DataStore.Context;

namespace Trivister.DataStore.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdpDbContext>
{
    public IdpDbContext CreateDbContext(string[] args)
    {
        var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("secrets.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddJsonFile($"appsettings.{envName}.json", optional: true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional:true)
            .Build();
        var builder = new DbContextOptionsBuilder<IdpDbContext>();
        var connectionString = configuration.GetConnectionString("TravisterDbConnection");
        builder.UseSqlServer(connectionString);
        return new IdpDbContext(builder.Options);
    }
}