using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trivister.ApplicationServices.Abstractions;
using Trivister.ApplicationServices.Common.Behaviour;
using Trivister.ApplicationServices.Features.Account;
using Trivister.Common.Options;

namespace Trivister.ApplicationServices.Extentions;

public static class DependencyInjection
{
    public static void InjectApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(typeof(LoginCommand).Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
      
    }
}