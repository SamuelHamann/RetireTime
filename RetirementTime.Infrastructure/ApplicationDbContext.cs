using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Entities.Location;

namespace RetirementTime.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Subdivision>(entity =>
        {
            entity.ToTable("subdivision");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name)
                .HasMaxLength(100);
            entity.Property(s => s.Code)
                .HasMaxLength(10);

            entity.HasOne(s => s.Country)
                .WithMany(c => c.Subdivisions)
                .HasForeignKey(s => s.CountryId);
        });
        
        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("country");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name)
                .HasMaxLength(100);
            entity.Property(c => c.CountryCode)
                .HasMaxLength(10);
            
            entity.HasMany(c => c.Subdivisions)
                .WithOne(s => s.Country)
                .HasForeignKey(s => s.CountryId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Password)
                .IsRequired();
            entity.Property(e => e.FirstName)
                .IsRequired();
            entity.Property(e => e.LastName)
                .IsRequired();
            entity.Property(e => e.IsActive)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(e => e.RoleId);
            entity.HasOne(e => e.Country)
                .WithMany()
                .HasForeignKey(e => e.CountryId);
            entity.HasOne(e => e.Subdivision)
                .WithMany()
                .HasForeignKey(e => e.SubdivisionId);
            entity.HasOne(e => e.Spouse)
                .WithMany()
                .HasForeignKey(e => e.SpouseId);
        });
        
        SeedLocationData(modelBuilder);

    }

    private static void SeedLocationData(ModelBuilder modelBuilder)
    {
        var canada = new Country { Id = 1, Name = "Canada", CountryCode = "CA" };
        var usa = new Country { Id = 2, Name = "United States", CountryCode = "US" };
    
        modelBuilder.Entity<Country>().HasData(canada, usa);
        modelBuilder.Entity<Subdivision>().HasData(GetCanadianProvinces(canada));
    }
    
    private static List<Subdivision> GetCanadianProvinces(Country canada)
    {
        return
        [
            new Subdivision { Id = 1, Name = "Alberta", Code = "AB", CountryId =  canada.Id },
            new Subdivision { Id = 2, Name = "British Columbia", Code = "BC", CountryId =  canada.Id },
            new Subdivision { Id = 3, Name = "Manitoba", Code = "MB", CountryId =  canada.Id },
            new Subdivision { Id = 4, Name = "New Brunswick", Code = "NB", CountryId =  canada.Id },
            new Subdivision { Id = 5, Name = "Newfoundland and Labrador", Code = "NL", CountryId =  canada.Id },
            new Subdivision { Id = 6, Name = "Nova Scotia", Code = "NS", CountryId =  canada.Id },
            new Subdivision { Id = 7, Name = "Ontario", Code = "ON", CountryId =  canada.Id },
            new Subdivision { Id = 8, Name = "Prince Edward Island", Code = "PE", CountryId =  canada.Id },
            new Subdivision { Id = 9, Name = "Quebec", Code = "QC", CountryId =  canada.Id },
            new Subdivision { Id = 10, Name = "Saskatchewan", Code = "SK", CountryId =  canada.Id },
            new Subdivision { Id = 11, Name = "Northwest Territories", Code = "NT", CountryId =  canada.Id },
            new Subdivision { Id = 12, Name = "Nunavut", Code = "NU", CountryId =  canada.Id },
            new Subdivision { Id = 13, Name = "Yukon", Code = "YT", CountryId =  canada.Id }
        ];
    }
    
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Subdivision> Subdivisions { get; set; }
}