using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Domain.Entities.RealEstate;
using System.Collections.Generic;

namespace RetirementTime.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subdivision>(entity =>
        {
            entity.ToTable("subdivision");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10);

            entity.HasOne(s => s.Country)
                .WithMany(c => c.Subdivisions)
                .HasForeignKey(s => s.CountryId);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("country");
            entity.HasKey(e => e.Id);

            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(c => c.CountryCode)
                .IsRequired()
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

        modelBuilder.Entity<RealEstate>(entity =>
        {
            entity.ToTable("real_estate");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Price)
                .IsRequired();
            entity.Property(e => e.MonthlyElectricityCosts)
                .IsRequired();
            entity.Property(e => e.MonthlyInsuranceCosts)
                .IsRequired();
            entity.Property(e => e.PercentYearlyExpenses)
                .IsRequired();
            entity.Property(e => e.YearlyTaxesPercent)
                .IsRequired();
            entity.Property(e => e.YearlyAppreciation)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
        });

        modelBuilder.Entity<Mortgage>(entity =>
        {
            entity.ToTable("mortgage");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.InterestRate)
                .IsRequired();
            entity.Property(e => e.TermInYears)
                .IsRequired();
            entity.Property(e => e.DownPayment)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.RealEstate)
                .WithMany()
                .HasForeignKey(e => e.RealEstateId);
        });

        modelBuilder.Entity<Rent>(entity =>
        {
            entity.ToTable("rent");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.MonthlyRent)
                .IsRequired();
            entity.Property(e => e.YearlyRentIncrease)
                .IsRequired();
            entity.Property(e => e.MonthlyElectricityCosts);
            entity.Property(e => e.MonthlyInsuranceCosts);
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
        });

        modelBuilder.Entity<BuyOrRent>(entity =>
        {
            entity.ToTable("buy_or_rent");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.PercentDifferenceReinvested)
                .IsRequired();
            entity.Property(e => e.PercentDifferenceReinvestedGrowthRate)
                .IsRequired();
            entity.Property(e => e.ComparisonTimeframeInYears)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.Rent)
                .WithMany()
                .HasForeignKey(e => e.RentId);
            entity.HasOne(e => e.Mortgage)
                .WithMany()
                .HasForeignKey(e => e.MortgageId);
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
            entity.Property(e => e.LanguageCode)
                .IsRequired()
                .HasMaxLength(10)
                .HasDefaultValue("en");
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
            entity.HasOne(e => e.Language)
                .WithMany()
                .HasForeignKey(e => e.LanguageCode);
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("language");
            entity.HasKey(e => e.LanguageCode);

            entity.Property(e => e.LanguageCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.LanguageName)
                .IsRequired()
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Frequency>(entity =>
        {
            entity.ToTable("common_frequencies");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.FrequencyPerYear)
                .IsRequired();
        });

        SeedRoleData(modelBuilder);
        SeedLocationData(modelBuilder);
        SeedLanguageData(modelBuilder);
        SeedFrequencyData(modelBuilder);
    }

    private static void SeedRoleData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "User" },
            new Role { Id = 2, Name = "Admin" }
        );
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
        return new List<Subdivision>
        {
            new Subdivision { Id = 1, Name = "Alberta", Code = "AB", CountryId = canada.Id },
            new Subdivision { Id = 2, Name = "British Columbia", Code = "BC", CountryId = canada.Id },
            new Subdivision { Id = 3, Name = "Manitoba", Code = "MB", CountryId = canada.Id },
            new Subdivision { Id = 4, Name = "New Brunswick", Code = "NB", CountryId = canada.Id },
            new Subdivision { Id = 5, Name = "Newfoundland and Labrador", Code = "NL", CountryId = canada.Id },
            new Subdivision { Id = 6, Name = "Nova Scotia", Code = "NS", CountryId = canada.Id },
            new Subdivision { Id = 7, Name = "Ontario", Code = "ON", CountryId = canada.Id },
            new Subdivision { Id = 8, Name = "Prince Edward Island", Code = "PE", CountryId = canada.Id },
            new Subdivision { Id = 9, Name = "Quebec", Code = "QC", CountryId = canada.Id },
            new Subdivision { Id = 10, Name = "Saskatchewan", Code = "SK", CountryId = canada.Id },
            new Subdivision { Id = 11, Name = "Northwest Territories", Code = "NT", CountryId = canada.Id },
            new Subdivision { Id = 12, Name = "Nunavut", Code = "NU", CountryId = canada.Id },
            new Subdivision { Id = 13, Name = "Yukon", Code = "YT", CountryId = canada.Id }
        };
    }

    private static void SeedLanguageData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>().HasData(
            new Language { LanguageCode = "en", LanguageName = "English" },
            new Language { LanguageCode = "fr", LanguageName = "French" }
        );
    }

    private static void SeedFrequencyData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Frequency>().HasData(
            new Frequency { Id = 1, Name = "Weekly", FrequencyPerYear = 52 },
            new Frequency { Id = 2, Name = "Bi-Weekly", FrequencyPerYear = 26 },
            new Frequency { Id = 3, Name = "Monthly", FrequencyPerYear = 12 },
            new Frequency { Id = 4, Name = "Bi-Monthly", FrequencyPerYear = 6 },
            new Frequency { Id = 5, Name = "Quarterly", FrequencyPerYear = 4 },
            new Frequency { Id = 6, Name = "Semi-Annually", FrequencyPerYear = 2 },
            new Frequency { Id = 7, Name = "Annually", FrequencyPerYear = 1 }
        );
    }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Subdivision> Subdivisions { get; set; }
    public DbSet<RealEstate> RealEstates { get; set; }
    public DbSet<Mortgage> Mortgages { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<BuyOrRent> BuyOrRents { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Frequency> Frequencies { get; set; }

}