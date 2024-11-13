using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FindMyStuff.Api.Shared
{
    public class RemoveVersionDocumentFilter(string versionTemplate = "v{#version}/") : IDocumentFilter
    {
        private readonly string _versionTemplate = versionTemplate.Replace("{#version}", "{0}");

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var openApiPaths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                path.Deconstruct(out var key, out var value);
                var text = key;
                var value2 = value;
                openApiPaths.Add(text.Replace(string.Format(_versionTemplate, swaggerDoc.Info.Version), "", StringComparison.InvariantCulture), value2);
            }

            swaggerDoc.Paths = openApiPaths;
        }
    }
}
 
