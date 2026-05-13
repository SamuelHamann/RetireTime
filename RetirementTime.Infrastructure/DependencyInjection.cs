using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Interfaces.Repositories;
using RetirementTime.Domain.Interfaces.Services;
using RetirementTime.Domain.Services;
using RetirementTime.Infrastructure.Repositories;
using RetirementTime.Infrastructure.Services;

namespace RetirementTime.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddRepositories();
        services.AddScoped<INetWorthSnapshotService, NetWorthSnapshotService>();
        services.AddScoped<INetWorthCalculationService, NetWorthCalculationService>();
        services.AddScoped<ICashflowCalculationService, CashflowCalculationService>();
        services.AddScoped<IDebtPayoffCalculationService, DebtPayoffCalculationService>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ISubdivisionRepository, SubdivisionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFrequencyRepository, FrequencyRepository>();
        services.AddScoped<IOnboardingPersonalInfoRepository, OnboardingPersonalInfoRepository>();
        services.AddScoped<IOnboardingAssetsRepository, OnboardingAssetsRepository>();
        services.AddScoped<IOnboardingDebtRepository, OnboardingDebtRepository>();
        services.AddScoped<IOnboardingEmploymentRepository, OnboardingEmploymentRepository>();
        services.AddScoped<IDashboardScenarioRepository, DashboardScenarioRepository>();
        services.AddScoped<IDashboardAssumptionsRepository, DashboardAssumptionsRepository>();
        services.AddScoped<IEmploymentIncomeRepository, EmploymentIncomeRepository>();
        services.AddScoped<ISelfEmploymentIncomeRepository, SelfEmploymentIncomeRepository>();
        services.AddScoped<IPensionDefinedBenefitsRepository, PensionDefinedBenefitsRepository>();
        services.AddScoped<IPensionDefinedContributionRepository, PensionDefinedContributionRepository>();
        services.AddScoped<IDefinedProfitSharingRepository, DefinedProfitSharingRepository>();
        services.AddScoped<IGroupRrspRepository, GroupRrspRepository>();
        services.AddScoped<ISharePurchasePlanRepository, SharePurchasePlanRepository>();
        services.AddScoped<IOasCppIncomeRepository, OasCppIncomeRepository>();
        services.AddScoped<IOtherIncomeOrBenefitsRepository, OtherIncomeOrBenefitsRepository>();
        services.AddScoped<IPropertyIncomeRepository, PropertyIncomeRepository>();
        services.AddScoped<IAssetsHomeRepository, AssetsHomeRepository>();
        services.AddScoped<IAssetsInvestmentPropertyRepository, AssetsInvestmentPropertyRepository>();
        services.AddScoped<IAssetsInvestmentAccountRepository, AssetsInvestmentAccountRepository>();
        services.AddScoped<IAssetsPhysicalAssetRepository, AssetsPhysicalAssetRepository>();
        services.AddScoped<IGenericDebtRepository, GenericDebtRepository>();
        services.AddScoped<INetWorthHistoryRepository, NetWorthHistoryRepository>();
        services.AddScoped<ISpendingRepository, SpendingRepository>();
        services.AddScoped<IRetirementTimelineRepository, RetirementTimelineRepository>();
        services.AddScoped<IOasConstantsRepository, OasConstantsRepository>();

        return services;
    }
}