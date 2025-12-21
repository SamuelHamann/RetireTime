using Microsoft.EntityFrameworkCore;
using RetimrementTime.Domain.Entities;
using RetimrementTime.Domain.Entities.Location;

namespace RetirementTime.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Subdivision> Subdivisions { get; set; }
    public DbSet<LocationInfo> LocationInfos { get; set; }
}