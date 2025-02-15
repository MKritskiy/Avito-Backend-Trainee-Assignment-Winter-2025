using Infrastucture.Data;
using Infrastucture.Repositories;
using Infrastucture.General;
using Infrastucture.Services;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;

namespace Infrastucture;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrustructureServices(this IServiceCollection services, IConfigurationManager config, ILogger logger)
    {
        string? connectionString = config.GetConnectionString("ConnectionString");
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
        services.AddScoped<ICoinTransferRepository, CoinTransferRepository>();

        services.AddScoped<IEncrypt, Encrypt>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICoinService, CoinService>();

        logger.LogInformation("{Project} services registered", "Infrastructure");

        return services;
    }
}
