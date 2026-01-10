using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Interfaces;
using RetirementTime.Infrastructure.Repositories;

namespace RetirementTime.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureServices(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseNpgsql(connectionString));

            services.AddRepositories();
        
            return services;
        }

        private IServiceCollection AddRepositories()
        {
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ISubdivisionRepository, SubdivisionRepository>();
            return services;
        }
    }
}