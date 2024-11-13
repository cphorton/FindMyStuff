using FluentValidation;

namespace FindMyStuff.Api.Configuration;

public static class FluentValidationConfiguration
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Transient);
        return services;
    }
}
