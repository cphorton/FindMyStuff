using Asp.Versioning;
using FindMyStuff.Api.Shared;
using Serilog;

namespace FindMyStuff.Api.Configuration;

public static class ApiConfiguration
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
    {

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1.0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VV";
            options.SubstituteApiVersionInUrl = true;
            options.SubstitutionFormat = "VV";  // This needs to be there to format the URL within the swagger doc properly
        });

        services.AddEndpointsApiExplorer();
        services.AddSingleton<AvailableVersions>();

        return services;
    }

    public static void ListEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetServices<EndpointDataSource>()
            .SelectMany(x => x.Endpoints)
            .ToArray();

        foreach (var endpoint in endpoints)
        {
            Log.Debug(endpoint.DisplayName ?? "");
        }
    }
}
