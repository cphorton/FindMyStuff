using System.Reflection;
using FindMyStuff.Api.Shared;

namespace FindMyStuff.Api.Configuration
{
    public static class EndpointConfiguration
    {
        public static WebApplication ConfigureEndpoints<T>(this WebApplication app)
        {
            return app.ConfigureEndpoints(typeof(T));
        }

        public static WebApplication ConfigureEndpoints(this WebApplication app, Type assemblyType)
        {            
            var availaleVersions = app.Services.GetRequiredService<AvailableVersions>();
            //Scan the assembly to find all endpoints
            var assembly = assemblyType.GetTypeInfo().Assembly;
            var apiEndpoints = assembly.DefinedTypes.Where(x => x.BaseType == typeof(ApiEndpoint) && x != typeof(ApiEndpoint));
            foreach (var apiEndpoint in apiEndpoints)
            {
                var endpointConfiguration = Activator.CreateInstance(apiEndpoint, app, availaleVersions)
                    ?? throw (new Exception($"Error creating endpoint {apiEndpoint}"));
            }
            return app;
        }
    }
}
