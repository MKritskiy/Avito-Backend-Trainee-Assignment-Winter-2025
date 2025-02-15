using Serilog;

namespace Avito_Backend_Trainee_Assignment_Winter_2025.Configurations;

public static class LoggerConfigs
{
    public static WebApplicationBuilder AddLoggerConfigs(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
        return builder;
    }
}
