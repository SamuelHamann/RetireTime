using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Entities.BeginnerGuide.Assets;
using RetirementTime.Domain.Entities.BeginnerGuide.Debt;
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
        
        modelBuilder.Entity<MainResidence>(entity =>
        {
            entity.ToTable("asset_main_residence");
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
        
        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.ToTable("account_type");
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
            entity.ToTable("investment_account", t =>
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
            entity.ToTable("investment_stock");
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
        
        modelBuilder.Entity<AssetType>(entity =>
        {
            entity.ToTable("asset_type");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Description)
                .HasMaxLength(500);
        });
        
        modelBuilder.Entity<OtherAsset>(entity =>
        {
            entity.ToTable("other_asset");
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
        
        modelBuilder.Entity<InvestmentProperty>(entity =>
        {
            entity.ToTable("investment_property");
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
            entity.ToTable("beginner_guide_debt");
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



        SeedRoleData(modelBuilder);
        SeedLocationData(modelBuilder);
        SeedLanguageData(modelBuilder);
        SeedAccountTypeData(modelBuilder);
        SeedAssetTypeData(modelBuilder);

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
        modelBuilder.Entity<AccountType>().HasData(
            new AccountType { Id = 1, Name = "TFSA (Tax-Free Savings Account)", CountryId = 1, SubdivisionId = null },
            new AccountType { Id = 2, Name = "RRSP (Registered Retirement Savings Plan)", CountryId = 1, SubdivisionId = null },
            new AccountType { Id = 3, Name = "FHSA (First Home Savings Account)", CountryId = 1, SubdivisionId = null },
            new AccountType { Id = 4, Name = "RESP (Registered Education Savings Plan)", CountryId = 1, SubdivisionId = null },
            new AccountType { Id = 5, Name = "RRIF (Registered Retirement Income Fund)", CountryId = 1, SubdivisionId = null },
            new AccountType { Id = 6, Name = "LIRA (Locked-In Retirement Account)", CountryId = 1, SubdivisionId = null },
            new AccountType { Id = 7, Name = "Non-Registered Investment Account", CountryId = 1, SubdivisionId = null },
            new AccountType { Id = 8, Name = "Cash Account", CountryId = 1, SubdivisionId = null },
            new AccountType { Id = 9, Name = "Margin Account", CountryId = 1, SubdivisionId = null },
            
            // US account types
            new AccountType { Id = 10, Name = "401(k)", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 11, Name = "Traditional IRA", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 12, Name = "Roth IRA", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 13, Name = "SEP IRA", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 14, Name = "SIMPLE IRA", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 15, Name = "403(b)", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 16, Name = "457 Plan", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 17, Name = "Thrift Savings Plan (TSP)", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 18, Name = "529 Education Savings Plan", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 19, Name = "HSA (Health Savings Account)", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 20, Name = "Brokerage Account (Taxable)", CountryId = 2, SubdivisionId = null },
            new AccountType { Id = 21, Name = "Cash Management Account", CountryId = 2, SubdivisionId = null }
        );
    }
    
    private static void SeedAssetTypeData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AssetType>().HasData(
            new AssetType { Id = 1, Name = "Precious Metals", Description = "Gold, silver, platinum, and other precious metals" },
            new AssetType { Id = 2, Name = "Vehicles", Description = "Cars, motorcycles, boats, RVs, etc." },
            new AssetType { Id = 3, Name = "Jewelry", Description = "Fine jewelry, watches, and collectible pieces" },
            new AssetType { Id = 4, Name = "Cash", Description = "Physical cash holdings" },
            new AssetType { Id = 5, Name = "Cryptocurrency", Description = "Bitcoin, Ethereum, and other digital currencies" },
            new AssetType { Id = 6, Name = "Art & Collectibles", Description = "Paintings, sculptures, rare collectibles" },
            new AssetType { Id = 7, Name = "Antiques", Description = "Antique furniture, vintage items" },
            new AssetType { Id = 8, Name = "Wine & Spirits", Description = "Collectible wines and rare spirits" },
            new AssetType { Id = 9, Name = "Equipment & Tools", Description = "Professional or recreational equipment" },
            new AssetType { Id = 10, Name = "Other", Description = "Other valuable assets not listed above" }
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
    public DbSet<MainResidence> MainResidences { get; set; }
    public DbSet<AccountType> AccountTypes { get; set; }
    public DbSet<BeginnerGuideAssetsInvestmentAccount> InvestmentAccounts { get; set; }
    public DbSet<BeginnerGuideAssetsStockData> InvestmentStocks { get; set; }
    public DbSet<AssetType> AssetTypes { get; set; }
    public DbSet<OtherAsset> OtherAssets { get; set; }
    public DbSet<InvestmentProperty> InvestmentProperties { get; set; }
    public DbSet<BeginnerGuideDebt> BeginnerGuideDebts { get; set; }
}