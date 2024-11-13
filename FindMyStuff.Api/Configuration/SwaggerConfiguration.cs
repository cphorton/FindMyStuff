using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FindMyStuff.Api.Configuration
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            //Configure swagger options.  This is done using the IConfigureOptions pattern to allow injection of some dependencies
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            return services;
        }

        public static WebApplication UseSwaggerConfiguration(this WebApplication app)
        {
            app.UseSwagger(
                options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        swagger.Servers = [new OpenApiServer 
                            { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/v{swagger.Info.Version}" }
                        ];
                    });
                });

            app.UseSwaggerUI(
                options =>
                {
                    var descriptions = app.DescribeApiVersions();

                    // build a swagger endpoint for each discovered API version
                    foreach (var groupName in descriptions.Select(x => x.GroupName))
                    {
                        var url = $"/swagger/{groupName}/swagger.json";
                        var name = groupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });

            return app;
        }
    }
}
