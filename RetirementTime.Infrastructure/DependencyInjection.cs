using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Interfaces;
using RetirementTime.Domain.Interfaces.Repositories;
using RetirementTime.Infrastructure.Repositories;

namespace RetirementTime.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(connectionString));

        services.AddRepositories();
        
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ISubdivisionRepository, SubdivisionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFrequencyRepository, FrequencyRepository>();
        services.AddScoped<IMainResidenceRepository, MainResidenceRepository>();
        services.AddScoped<IAccountTypeRepository, AccountTypeRepository>();
        services.AddScoped<IBeginnerGuideAssetsInvestmentAccountRepository, BeginnerGuideAssetsInvestmentAccountRepository>();
        services.AddScoped<IBeginnerGuideAssetsStockDataRepository, BeginnerGuideAssetsStockDataRepository>();
        services.AddScoped<IOtherAssetRepository, OtherAssetRepository>();
        services.AddScoped<IAssetTypeRepository, AssetTypeRepository>();
        services.AddScoped<IInvestmentPropertyRepository, InvestmentPropertyRepository>();
        services.AddScoped<IBeginnerGuideDebtRepository, BeginnerGuideDebtRepository>();
        services.AddScoped<IEmploymentRepository, EmploymentRepository>();
        services.AddScoped<ISelfEmploymentRepository, SelfEmploymentRepository>();
        return services;
    }
}