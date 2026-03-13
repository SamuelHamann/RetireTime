using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Entities.BeginnerGuide.Benefits;
using RetirementTime.Domain.Entities.BeginnerGuide.Debt;
using RetirementTime.Domain.Entities.BeginnerGuide.Income;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Domain.Entities.RealEstate;

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
            entity.Property(e => e.YearlyHoaCosts);
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
            entity.Property(e => e.MonthlyMortgageInsuranceCosts);
            entity.Property(e => e.ClosingCosts);
            entity.Property(e => e.AdditionalCosts);
            entity.Property(e => e.AdditionalMonthlyCosts);
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
        
        modelBuilder.Entity<BeginnerGuideMainResidence>(entity =>
        {
            entity.ToTable("beginner_guide_assets_main_residence");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.HasMainResidence)
                .IsRequired();
            
            entity.Property(e => e.PurchasePrice)
                .HasPrecision(18, 2);
            
            entity.Property(e => e.MonthlyMortgagePayments)
                .HasPrecision(18, 2);
            
            entity.Property(e => e.MortgageLeft)
                .HasPrecision(18, 2);
            
            entity.Property(e => e.YearlyInsurance)
                .HasPrecision(18, 2);
            
            entity.Property(e => e.MonthlyElectricityCosts)
                .HasPrecision(18, 2);
            
            entity.Property(e => e.MortgageDuration);
            
            entity.Property(e => e.MortgageStartDate)
                .HasColumnType("timestamp with time zone");
            
            entity.Property(e => e.EstimatedValue)
                .HasPrecision(18, 2);
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasIndex(e => e.UserId)
                .IsUnique();
        });
        
        modelBuilder.Entity<BeginnerGuideAccountType>(entity =>
        {
            entity.ToTable("beginner_guide_assets_account_types");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.CountryId)
                .IsRequired();
            entity.Property(e => e.SubdivisionId);
            
            entity.HasOne(e => e.Country)
                .WithMany()
                .HasForeignKey(e => e.CountryId);
            
            entity.HasOne(e => e.Subdivision)
                .WithMany()
                .HasForeignKey(e => e.SubdivisionId);
        });
        
        modelBuilder.Entity<BeginnerGuideAssetsInvestmentAccount>(entity =>
        {
            entity.ToTable("beginner_guide_assets_investment_accounts", t =>
            {
                // Check constraint: ensure bulk amount XOR stocks (not both)
                // If IsBulkAmount is true, BulkAmount must not be null
                // If IsBulkAmount is false, BulkAmount must be null
                t.HasCheckConstraint(
                    "CK_InvestmentAccount_BulkAmount_XOR_Stocks",
                    "(\"IsBulkAmount\" = true AND \"BulkAmount\" IS NOT NULL) OR (\"IsBulkAmount\" = false AND \"BulkAmount\" IS NULL)");
            });
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.UserId)
                .IsRequired();
            entity.Property(e => e.AccountName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.AccountTypeId)
                .IsRequired();
            entity.Property(e => e.IsBulkAmount)
                .IsRequired();
            entity.Property(e => e.BulkAmount);
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
            
            entity.HasOne(e => e.AccountType)
                .WithMany()
                .HasForeignKey(e => e.AccountTypeId);
            
            entity.HasMany(e => e.Stocks)
                .WithOne(s => s.InvestmentAccount)
                .HasForeignKey(s => s.InvestmentAccountId);
        });
        
        modelBuilder.Entity<BeginnerGuideAssetsStockData>(entity =>
        {
            entity.ToTable("beginner_guide_assets_stock_data");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.InvestmentAccountId)
                .IsRequired();
            entity.Property(e => e.TickerSymbol)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.Amount)
                .IsRequired();
            
            entity.HasOne(e => e.InvestmentAccount)
                .WithMany(a => a.Stocks)
                .HasForeignKey(e => e.InvestmentAccountId);
        });
        
        modelBuilder.Entity<BeginnerGuideAssetType>(entity =>
        {
            entity.ToTable("beginner_guide_assets_asset_type");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
        });
        
        modelBuilder.Entity<BeginnerGuideOtherAsset>(entity =>
        {
            entity.ToTable("beginner_guide_assets_other_assets");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.UserId)
                .IsRequired();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.AssetTypeId)
                .IsRequired();
            entity.Property(e => e.CurrentValue)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.PurchasePrice)
                .HasPrecision(18, 2);
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.AssetType)
                .WithMany()
                .HasForeignKey(e => e.AssetTypeId);
        });
        
        modelBuilder.Entity<BeginnerGuideInvestmentProperty>(entity =>
        {
            entity.ToTable("beginner_guide_assets_investment_properties");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.UserId)
                .IsRequired();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.PurchasePrice)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.MonthlyMortgagePayments)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.MortgageLeft)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.YearlyInsurance)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.MonthlyElectricityCosts)
                .HasPrecision(18, 2);
            entity.Property(e => e.MortgageDuration)
                .IsRequired();
            entity.Property(e => e.MortgageStartDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone");
            entity.Property(e => e.EstimatedValue)
                .HasPrecision(18, 2);
            entity.Property(e => e.MonthlyCost)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.MonthlyRevenue)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BeginnerGuideDebt>(entity =>
        {
            entity.ToTable("beginner_guide_debt_debts");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId)
                .IsRequired();
            entity.Property(e => e.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.MonthlyPayment)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.InterestRate)
                .IsRequired()
                .HasPrecision(5, 2);
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BeginnerGuideEmployment>(entity =>
        {
            entity.ToTable("beginner_guide_income_employments");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId)
                .IsRequired();
            entity.Property(e => e.EmployerName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.AnnualSalary)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.AverageAnnualWageIncrease)
                .IsRequired()
                .HasPrecision(5, 2);
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.AdditionalCompensations)
                .WithOne(c => c.Employment)
                .HasForeignKey(c => c.EmploymentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BeginnerGuideAdditionalCompensation>(entity =>
        {
            entity.ToTable("beginner_guide_income_additional_compensations");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.EmploymentId)
                .IsRequired();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.FrequencyId)
                .IsRequired()
                .HasDefaultValue(7); // Default to Annually
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.Employment)
                .WithMany(emp => emp.AdditionalCompensations)
                .HasForeignKey(e => e.EmploymentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Frequency)
                .WithMany()
                .HasForeignKey(e => e.FrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BeginnerGuideSelfEmployment>(entity =>
        {
            entity.ToTable("beginner_guide_income_self_employments");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId)
                .IsRequired();
            entity.Property(e => e.BusinessName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.AnnualSalary)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.AverageAnnualRevenueIncrease)
                .IsRequired()
                .HasPrecision(5, 2);
            entity.Property(e => e.MonthlyDividends)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.AdditionalCompensations)
                .WithOne(c => c.SelfEmployment)
                .HasForeignKey(c => c.SelfEmploymentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BeginnerGuideSelfEmploymentAdditionalCompensation>(entity =>
        {
            entity.ToTable("beginner_guide_income_self_employment_additional_compensations");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.SelfEmploymentId)
                .IsRequired();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.FrequencyId)
                .IsRequired()
                .HasDefaultValue(7); // Default to Annually
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.SelfEmployment)
                .WithMany(emp => emp.AdditionalCompensations)
                .HasForeignKey(e => e.SelfEmploymentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Frequency)
                .WithMany()
                .HasForeignKey(e => e.FrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
        });



        modelBuilder.Entity<BeginnerGuidePensionType>(entity =>
        {
            entity.ToTable("beginner_guide_benefits_pension_types");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
        });

        modelBuilder.Entity<BeginnerGuidePension>(entity =>
        {
            entity.ToTable("beginner_guide_benefits_pensions");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId)
                .IsRequired();
            entity.Property(e => e.PensionTypeId)
                .IsRequired();
            entity.Property(e => e.EmployerName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.PensionType)
                .WithMany()
                .HasForeignKey(e => e.PensionTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BeginnerGuideGovernmentPension>(entity =>
        {
            entity.ToTable("beginner_guide_benefits_government_pension");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId)
                .IsRequired();
            entity.Property(e => e.YearsWorked)
                .IsRequired();
            entity.Property(e => e.HasSpecializedPublicSectorPension)
                .IsRequired();
            entity.Property(e => e.SpecializedPensionName)
                .HasMaxLength(200);
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.UserId)
                .IsUnique();
        });

        modelBuilder.Entity<BeginnerGuideOtherRecurringGain>(entity =>
        {
            entity.ToTable("beginner_guide_benefits_other_recurring_gains");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId)
                .IsRequired();
            entity.Property(e => e.SourceName)
                .IsRequired()
                .HasMaxLength(200);
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasPrecision(18, 2);
            entity.Property(e => e.FrequencyId)
                .IsRequired();
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Frequency)
                .WithMany()
                .HasForeignKey(e => e.FrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        SeedRoleData(modelBuilder);
        SeedLocationData(modelBuilder);
        SeedLanguageData(modelBuilder);
        SeedAccountTypeData(modelBuilder);
        SeedAssetTypeData(modelBuilder);
        SeedFrequencyData(modelBuilder);
        SeedPensionTypeData(modelBuilder);

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
    
    private static void SeedLanguageData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Language>().HasData(
            new Language { LanguageCode = "en", LanguageName = "English" },
            new Language { LanguageCode = "fr", LanguageName = "French" }
        );
    }
    
    private static void SeedAccountTypeData(ModelBuilder modelBuilder)
    {
        // Canadian account types
        modelBuilder.Entity<BeginnerGuideAccountType>().HasData(
            new BeginnerGuideAccountType { Id = 1, Name = "TFSA (Tax-Free Savings Account)", CountryId = 1, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 2, Name = "RRSP (Registered Retirement Savings Plan)", CountryId = 1, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 3, Name = "FHSA (First Home Savings Account)", CountryId = 1, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 4, Name = "RESP (Registered Education Savings Plan)", CountryId = 1, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 5, Name = "RRIF (Registered Retirement Income Fund)", CountryId = 1, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 6, Name = "LIRA (Locked-In Retirement Account)", CountryId = 1, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 7, Name = "Non-Registered Investment Account", CountryId = 1, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 8, Name = "Cash Account", CountryId = 1, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 9, Name = "Margin Account", CountryId = 1, SubdivisionId = null },
            
            // US account types
            new BeginnerGuideAccountType { Id = 10, Name = "401(k)", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 11, Name = "Traditional IRA", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 12, Name = "Roth IRA", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 13, Name = "SEP IRA", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 14, Name = "SIMPLE IRA", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 15, Name = "403(b)", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 16, Name = "457 Plan", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 17, Name = "Thrift Savings Plan (TSP)", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 18, Name = "529 Education Savings Plan", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 19, Name = "HSA (Health Savings Account)", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 20, Name = "Brokerage Account (Taxable)", CountryId = 2, SubdivisionId = null },
            new BeginnerGuideAccountType { Id = 21, Name = "Cash Management Account", CountryId = 2, SubdivisionId = null }
        );
    }
    
    private static void SeedAssetTypeData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BeginnerGuideAssetType>().HasData(
            new BeginnerGuideAssetType { Id = 1, Name = "Precious Metals", Description = "Gold, silver, platinum, and other precious metals" },
            new BeginnerGuideAssetType { Id = 2, Name = "Vehicles", Description = "Cars, motorcycles, boats, RVs, etc." },
            new BeginnerGuideAssetType { Id = 3, Name = "Jewelry", Description = "Fine jewelry, watches, and collectible pieces" },
            new BeginnerGuideAssetType { Id = 4, Name = "Cash", Description = "Physical cash holdings" },
            new BeginnerGuideAssetType { Id = 5, Name = "Cryptocurrency", Description = "Bitcoin, Ethereum, and other digital currencies" },
            new BeginnerGuideAssetType { Id = 6, Name = "Art & Collectibles", Description = "Paintings, sculptures, rare collectibles" },
            new BeginnerGuideAssetType { Id = 7, Name = "Antiques", Description = "Antique furniture, vintage items" },
            new BeginnerGuideAssetType { Id = 8, Name = "Wine & Spirits", Description = "Collectible wines and rare spirits" },
            new BeginnerGuideAssetType { Id = 9, Name = "Equipment & Tools", Description = "Professional or recreational equipment" },
            new BeginnerGuideAssetType { Id = 10, Name = "Other", Description = "Other valuable assets not listed above" }
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

    private static void SeedPensionTypeData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BeginnerGuidePensionType>().HasData(
            new BeginnerGuidePensionType { Id = 1, Name = "Defined Benefit Pension Plan (DBPP)", Description = "Employer-funded plan that pays a fixed monthly benefit at retirement based on salary and years of service." },
            new BeginnerGuidePensionType { Id = 2, Name = "Defined Contribution Pension Plan (DCPP)", Description = "Contributions from employer and/or employee are invested; retirement income depends on investment performance." },
            new BeginnerGuidePensionType { Id = 3, Name = "Pooled Registered Pension Plan (PRPP)", Description = "Large-scale pension plan for employees and self-employed individuals not covered by workplace plans." },
            new BeginnerGuidePensionType { Id = 4, Name = "Group Registered Retirement Savings Plan (GRSP)", Description = "Employer-sponsored RRSP where contributions are made by both employer and employee." },
            new BeginnerGuidePensionType { Id = 5, Name = "Target Benefit Plan", Description = "Hybrid plan that targets a specific retirement benefit but adjusts contributions or benefits based on fund performance." },
            new BeginnerGuidePensionType { Id = 6, Name = "Deferred Profit Sharing Plan (DPSP)", Description = "Employer shares a portion of profits with employees, held in trust until retirement." }
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
    public DbSet<BeginnerGuideMainResidence> MainResidences { get; set; }
    public DbSet<BeginnerGuideAccountType> AccountTypes { get; set; }
    public DbSet<BeginnerGuideAssetsInvestmentAccount> InvestmentAccounts { get; set; }
    public DbSet<BeginnerGuideAssetsStockData> InvestmentStocks { get; set; }
    public DbSet<BeginnerGuideAssetType> AssetTypes { get; set; }
    public DbSet<BeginnerGuideOtherAsset> OtherAssets { get; set; }
    public DbSet<BeginnerGuideInvestmentProperty> InvestmentProperties { get; set; }
    public DbSet<BeginnerGuideDebt> BeginnerGuideDebts { get; set; }
    public DbSet<BeginnerGuideEmployment> Employments { get; set; }
    public DbSet<BeginnerGuideAdditionalCompensation> AdditionalCompensations { get; set; }
    public DbSet<BeginnerGuideSelfEmployment> SelfEmployments { get; set; }
    public DbSet<BeginnerGuideSelfEmploymentAdditionalCompensation> SelfEmploymentAdditionalCompensations { get; set; }
    public DbSet<BeginnerGuidePensionType> PensionTypes { get; set; }
    public DbSet<BeginnerGuidePension> BeginnerGuidePensions { get; set; }
    public DbSet<BeginnerGuideGovernmentPension> GovernmentPensions { get; set; }
    public DbSet<BeginnerGuideOtherRecurringGain> OtherRecurringGains { get; set; }
}