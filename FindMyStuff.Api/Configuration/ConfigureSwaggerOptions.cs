using Asp.Versioning.ApiExplorer;
using FindMyStuff.Api.Shared;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FindMyStuff.Api.Configuration;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider = apiVersionDescriptionProvider;

    public void Configure(SwaggerGenOptions options)
    {
        //Generate a swagger doc per version
        foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion))
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        //remove the version from the API url as it is being handled at the server level
        options.DocumentFilter<RemoveVersionDocumentFilter>();
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = "Minimal API Example",
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}
