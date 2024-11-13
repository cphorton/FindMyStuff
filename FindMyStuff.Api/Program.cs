using FindMyStuff.Api.Configuration;
using FindMyStuff.Api.Shared;
using FindMyStuff.Api.Utilities;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft.Identity.Web", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddApiConfiguration();
    builder.Services.AddSwaggerConfiguration();
    builder.Services.AddFluentValidation();
    builder.Services.AddMediatorConfiguration();
    builder.Services.AddSerilogConfiguration(builder.Configuration);

    await DbSetup.Setup();

    var app = builder.Build();

    app.ConfigureEndpoints<Program>();

    app.UseSwaggerConfiguration();
    app.UseValidationExceptionHandlingMiddleware();
    var task = app.RunAsync();

    var endpoints = app.Services.GetServices<EndpointDataSource>()
        .SelectMany(x => x.Endpoints)
        .ToArray();

    foreach (var endpoint in endpoints)
    {
        Console.WriteLine(endpoint.DisplayName);
    }

    await task;
}
catch (Exception ex)
{
    Log.Fatal(ex, "***** Application terinated unexpectedly *****");
}
finally
{
    Log.Information("****** Application shutting down ******");
    Log.CloseAndFlush();
}
