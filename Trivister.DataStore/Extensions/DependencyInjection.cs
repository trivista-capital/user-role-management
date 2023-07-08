using System.Data.SqlTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Trivister.ApplicationServices.Abstractions;
using Trivister.Core.Entities;
using Trivister.DataStore.Context;
using Trivister.DataStore.Repositories;

namespace Trivister.DataStore.Extensions;

public static class DependencyInjection
{
    public static void InjectPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<IdpDbContext>(x => x.UseSqlServer(connectionString));
        services.AddTransient<IGlobalTSDbContext>(services => services?.GetService<IdpDbContext>());
        services.AddIdentityCore<ApplicationUser>(option =>
        {
            option.Tokens.EmailConfirmationTokenProvider = "emailConfirmation";
        }).AddRoles<ApplicationRole>().AddEntityFrameworkStores<IdpDbContext>()
            .AddEntityFrameworkStores<IdpDbContext>()
            .AddDefaultTokenProviders().AddTokenProvider<EmailConfirmationTokenProvider<ApplicationUser>>("emailConfirmation");
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredUniqueChars = 1;
            options.Password.RequiredLength = 8;
        });
        services.AddScoped<IIdentityService, IdentityService>();
    }
}