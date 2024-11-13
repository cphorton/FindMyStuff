using Asp.Versioning;
using Asp.Versioning.Builder;
using Asp.Versioning.Conventions;

namespace FindMyStuff.Api.Shared
{
    public class AvailableVersions
    {
        public ApiVersion Version1_0 { get; } = new ApiVersion(1, 0);
        // public ApiVersion Version1_1 { get; } = new ApiVersion(1, 1);
        // public ApiVersion Version1_2 { get; } = new ApiVersion(1, 2);

        public IEnumerable<ApiVersion> VersionList()
        {
            //Use reflection to get the list of ApiVersion
            //Even though reflection is kind of slow
            //this should be OK here because this code is only used during startup
            return typeof(AvailableVersions).GetProperties()
                .Where(x => x.PropertyType == typeof(ApiVersion))
                .Select(x => (ApiVersion)x.GetValue(this)!);
        }

        public ApiVersionSet ConfigureVersions(WebApplication app)
        {
            var apiVersionSet = app.NewApiVersionSet()
                .HasApiVersions(VersionList())
                .Build();

            return apiVersionSet;
        }
    }
}
