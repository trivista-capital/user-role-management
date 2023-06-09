using Trivister.IDP;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Trivister.ApplicationServices.Extentions;
using Trivister.Common.Model;
using Trivister.DataStore.Extensions;
using Trivister.Infrastructure;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    var connectionString = builder.Configuration.GetConnectionString("TravisterDbConnection");

    if (!builder.Environment.IsDevelopment())
    {
        // var azureAppConfigConnectionString = "Endpoint=https://himsbackendappconfig.azconfig.io;Id=DU2q-ly-s0:zxCuonWONKo+FDi3xCHl;Secret=dTbXlp2lA6c/gnfJvgXIghAv3hR+TxGfOGKX0sSMhFk=";
        // Load configuration from Azure App Configuration
        // builder.Configuration.AddAzureAppConfiguration(options =>
        // {
        //     options.Connect(azureAppConfigConnectionString).ConfigureRefresh((refreshOptions) =>
        //     {
        //         // indicates that all configuration should be refreshed when the given key has changed.
        //         refreshOptions.Register(key: "sentinel", refreshAll: true);
        //         refreshOptions.SetCacheExpiration(TimeSpan.FromSeconds(5));
        //     }).UseFeatureFlags();
        // });
    }
    builder.Services.AddHttpContextAccessor();
    builder.Services.InjectApplicationServices(builder.Configuration);
    builder.Services.InjectPersistence(connectionString!);
    builder.Services.ConfigureInfrastructure(builder.Configuration);
    builder.Services.Configure<ApiBehaviorOptions>(option =>
    {
        option.InvalidModelStateResponseFactory = ErrorResult.GenerateErrorResponse;
    });
    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(ctx.Configuration));

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}