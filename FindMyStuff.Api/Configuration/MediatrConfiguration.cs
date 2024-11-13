using FindMyStuff.Api.Shared;

namespace FindMyStuff.Api.Configuration
{
    public static class MediatrConfiguration
    {
        public static IServiceCollection AddMediatorConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssemblyContaining<Program>();
                cfg.AddOpenRequestPreProcessor(typeof(ValidationBehaviour<>));
            });

            return services;
        }
    }
}
