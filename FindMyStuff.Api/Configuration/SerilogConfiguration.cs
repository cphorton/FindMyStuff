using FindMyStuff.Api.Shared;
using Serilog;
using Serilog.Events;

namespace FindMyStuff.Api.Configuration;

public static class SerilogConfiguration
{
    public static IServiceCollection AddSerilogConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        return services;
    }

    public static IApplicationBuilder UseSerilogConfigurationForRequestLogging(this IApplicationBuilder app)
    {
        app.UseSerilogRequestLogging(opts =>
        {
            opts.GetLevel = ExcludeOptions;
        });
        return app;
    }

    public static LogEventLevel ExcludeOptions(HttpContext ctx, double _, Exception? ex)
    {
        //If the exception type is a Validation Exception set the log level to verbose
        //Validation Exception logging is handled in ValidationExceptionHandlerMiddleware
        if (ex != null && ex.GetType() == typeof(ValidationException))
            return LogEventLevel.Verbose;

        return ex != null
            ? LogEventLevel.Error
            : ctx.Response.StatusCode > 499
                ? LogEventLevel.Error
                    : LogEventLevel.Information;
    }
}
