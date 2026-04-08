using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Domain.Entities.Onboarding;
using RetirementTime.Domain.Entities.RealEstate;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.PersistingIncome;

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
            entity.Property(e => e.HasCompletedIntro)
                .IsRequired()
                .HasDefaultValue(false);
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

        modelBuilder.Entity<OnboardingPersonalInfo>(entity =>
        {
            entity.ToTable("onboarding_step1_personal_info");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DateOfBirth)
                .IsRequired();

            entity.Property(e => e.CitizenshipStatus)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.MaritalStatus)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.HasCurrentChildren)
                .IsRequired();

            entity.Property(e => e.PlansFutureChildren)
                .IsRequired();

            entity.Property(e => e.IncludePartner)
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

            entity.HasIndex(e => e.UserId)
                .IsUnique();
        });

        modelBuilder.Entity<OnboardingAssets>(entity =>
        {
            entity.ToTable("onboarding_step2_assets");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.HasSavingsAccount)
                .IsRequired();

            entity.Property(e => e.HasTFSA)
                .IsRequired();

            entity.Property(e => e.HasRRSP)
                .IsRequired();

            entity.Property(e => e.HasRRIF)
                .IsRequired();

            entity.Property(e => e.HasFHSA)
                .IsRequired();

            entity.Property(e => e.HasRESP)
                .IsRequired();

            entity.Property(e => e.HasRDSP)
                .IsRequired();

            entity.Property(e => e.HasNonRegistered)
                .IsRequired();

            entity.Property(e => e.HasPension)
                .IsRequired();

            entity.Property(e => e.HasPrincipalResidence)
                .IsRequired();

            entity.Property(e => e.HasCar)
                .IsRequired();

            entity.Property(e => e.HasInvestmentProperty)
                .IsRequired();

            entity.Property(e => e.HasBusiness)
                .IsRequired();

            entity.Property(e => e.HasIncorporation)
                .IsRequired();

            entity.Property(e => e.HasPreciousMetals)
                .IsRequired();

            entity.Property(e => e.HasOtherHardAssets)
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

            entity.HasIndex(e => e.UserId)
                .IsUnique();
        });

        modelBuilder.Entity<OnboardingDebt>(entity =>
        {
            entity.ToTable("onboarding_step3_debt");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.HasPrimaryMortgage)
                .IsRequired();

            entity.Property(e => e.HasInvestmentPropertyMortgage)
                .IsRequired();

            entity.Property(e => e.HasCarPayments)
                .IsRequired();

            entity.Property(e => e.HasStudentLoans)
                .IsRequired();

            entity.Property(e => e.HasCreditCardDebt)
                .IsRequired();

            entity.Property(e => e.HasPersonalLoans)
                .IsRequired();

            entity.Property(e => e.HasBusinessLoans)
                .IsRequired();

            entity.Property(e => e.HasTaxDebt)
                .IsRequired();

            entity.Property(e => e.HasMedicalDebt)
                .IsRequired();

            entity.Property(e => e.HasInformalDebt)
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

            entity.HasIndex(e => e.UserId)
                .IsUnique();
        });

        modelBuilder.Entity<OnboardingEmployment>(entity =>
        {
            entity.ToTable("onboarding_step4_employment");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IsEmployed)
                .IsRequired();

            entity.Property(e => e.IsSelfEmployed)
                .IsRequired();

            entity.Property(e => e.PlannedRetirementAge);

            entity.Property(e => e.CppContributionYears);

            entity.Property(e => e.HasRoyalties)
                .IsRequired();

            entity.Property(e => e.HasDividends)
                .IsRequired();

            entity.Property(e => e.HasRentalIncome)
                .IsRequired();

            entity.Property(e => e.HasOtherIncome)
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

            entity.HasIndex(e => e.UserId)
                .IsUnique();
        });

        modelBuilder.Entity<DashboardScenario>(entity =>
        {
            entity.ToTable("dashboard_scenario");
            entity.HasKey(e => e.ScenarioId);

            entity.Property(e => e.ScenarioName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.ScenarioFullyCreated)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasIndex(e => e.UserId);
        });

        modelBuilder.Entity<EmploymentIncome>(entity =>
        {
            entity.ToTable("dashboard_income_employment_income");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.UserId)
                .IsRequired();

            entity.Property(e => e.EmployerName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.GrossSalary)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.GrossSalaryFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.NetSalary)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.NetSalaryFrequencyId)
                .IsRequired();

            entity.Property(e => e.GrossCommissions)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.GrossCommissionsFrequencyId)
                .IsRequired();

            entity.Property(e => e.NetCommissions)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.NetCommissionsFrequencyId)
                .IsRequired();

            entity.Property(e => e.GrossBonus)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.GrossBonusFrequencyId)
                .IsRequired();

            entity.Property(e => e.NetBonus)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.NetBonusFrequencyId)
                .IsRequired();

            entity.Property(e => e.PensionContributions)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.PensionContributionFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.TaxDeductions)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.TaxDeductionFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.CppDeductions)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.CppDeductionFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.OtherDeductions)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.OtherDeductionFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasMany(e => e.OtherIncomes)
                .WithOne(o => o.EmploymentIncome)
                .HasForeignKey(o => o.EmploymentIncomeId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.GrossSalaryFrequency)
                .WithMany()
                .HasForeignKey(e => e.GrossSalaryFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.NetSalaryFrequency)
                .WithMany()
                .HasForeignKey(e => e.NetSalaryFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.GrossCommissionsFrequency)
                .WithMany()
                .HasForeignKey(e => e.GrossCommissionsFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.NetCommissionsFrequency)
                .WithMany()
                .HasForeignKey(e => e.NetCommissionsFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.GrossBonusFrequency)
                .WithMany()
                .HasForeignKey(e => e.GrossBonusFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.NetBonusFrequency)
                .WithMany()
                .HasForeignKey(e => e.NetBonusFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.PensionContributionFrequency)
                .WithMany()
                .HasForeignKey(e => e.PensionContributionFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.TaxDeductionFrequency)
                .WithMany()
                .HasForeignKey(e => e.TaxDeductionFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.CppDeductionFrequency)
                .WithMany()
                .HasForeignKey(e => e.CppDeductionFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.OtherDeductionFrequency)
                .WithMany()
                .HasForeignKey(e => e.OtherDeductionFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ScenarioId);
            entity.HasIndex(e => e.UserId);
        });

        modelBuilder.Entity<OtherEmploymentIncome>(entity =>
        {
            entity.ToTable("dashboard_income_other_employment_income");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Gross)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.GrossFrequencyId)
                .IsRequired();

            entity.Property(e => e.Net)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.NetFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.EmploymentIncome)
                .WithMany(ei => ei.OtherIncomes)
                .HasForeignKey(e => e.EmploymentIncomeId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.GrossFrequency)
                .WithMany()
                .HasForeignKey(e => e.GrossFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.NetFrequency)
                .WithMany()
                .HasForeignKey(e => e.NetFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SelfEmploymentIncome>(entity =>
        {
            entity.ToTable("dashboard_income_self_employment_income");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.NetSalary)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.NetSalaryFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.GrossSalary)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.GrossSalaryFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.GrossDividends)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.GrossDividendsFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.NetDividends)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.NetDividendsFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.GrossSalaryFrequency)
                .WithMany()
                .HasForeignKey(e => e.GrossSalaryFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.NetSalaryFrequency)
                .WithMany()
                .HasForeignKey(e => e.NetSalaryFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.GrossDividendsFrequency)
                .WithMany()
                .HasForeignKey(e => e.GrossDividendsFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.NetDividendsFrequency)
                .WithMany()
                .HasForeignKey(e => e.NetDividendsFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PensionDefinedBenefits>(entity =>
        {
            entity.ToTable("dashboard_income_pension_defined_benefits");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.StartAge)
                .IsRequired();
            
            entity.Property(e => e.BenefitsPre65)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.BenefitsPre65FrequencyId)
                .IsRequired();
            
            entity.Property(e => e.BenefitsPost65)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.BenefitsPost65FrequencyId)
                .IsRequired();
            
            entity.Property(e => e.PercentIndexedToInflation)
                .IsRequired();
            
            entity.Property(e => e.PercentSurvivorBenefits)
                .IsRequired();
            
            entity.Property(e => e.RrspAdjustment)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.RrspAdjustmentFrequencyId)
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.BenefitsPre65Frequency)
                .WithMany()
                .HasForeignKey(e => e.BenefitsPre65FrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.BenefitsPost65Frequency)
                .WithMany()
                .HasForeignKey(e => e.BenefitsPost65FrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.RrspAdjustmentFrequency)
                .WithMany()
                .HasForeignKey(e => e.RrspAdjustmentFrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PensionDefinedContribution>(entity =>
        {
            entity.ToTable("dashboard_income_pension_defined_contribution");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.PercentOfSalaryEmployee)
                .HasColumnType("numeric(5,2)");
            
            entity.Property(e => e.PercentOfSalaryEmployer)
                .HasColumnType("numeric(5,2)");
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<GroupRrsp>(entity =>
        {
            entity.ToTable("dashboard_income_group_rrsp");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.PercentOfSalaryEmployee)
                .HasColumnType("numeric(5,2)");
            
            entity.Property(e => e.PercentOfSalaryEmployer)
                .HasColumnType("numeric(5,2)");
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<DefinedProfitSharing>(entity =>
        {
            entity.ToTable("dashboard_income_defined_profit_sharing");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.PercentOfSalaryEmployer)
                .HasColumnType("numeric(5,2)");
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<SharePurchasePlan>(entity =>
        {
            entity.ToTable("dashboard_income_share_purchase_plan");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.PercentOfSalaryEmployee)
                .IsRequired()
                .HasColumnType("numeric(5,2)");
            
            entity.Property(e => e.PercentOfSalaryEmployer)
                .IsRequired()
                .HasColumnType("numeric(5,2)");
            
            entity.Property(e => e.UseFlatAmountInsteadOfPercent)
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.PurchaseFrequency)
                .WithMany()
                .HasForeignKey(e => e.PurchaseFrequencyId);
            entity.HasOne(e => e.EmployerMatchFrequency)
                .WithMany()
                .HasForeignKey(e => e.EmployerMatchFrequencyId);
        });

        modelBuilder.Entity<OasCppIncome>(entity =>
        {
            entity.ToTable("dashboard_income_oas_cpp");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.IncomeLastYear)
                .IsRequired()
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.Income2YearsAgo)
                .IsRequired()
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.Income3YearsAgo)
                .IsRequired()
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.Income4YearsAgo)
                .IsRequired()
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.Income5YearsAgo)
                .IsRequired()
                .HasColumnType("numeric(18,2)");
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<OtherIncomeOrBenefits>(entity =>
        {
            entity.ToTable("dashboard_income_other_income_or_benefits");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Amount)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.FrequencyId)
                .IsRequired();
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            

            entity.HasOne(e => e.Frequency)
                .WithMany()
                .HasForeignKey(e => e.FrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RealEstateIncome>(entity =>
        {
            entity.ToTable("dashboard_income_real_estate");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.PropertyName)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasColumnType("numeric(18,2)");
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Frequency)
                .WithMany()
                .HasForeignKey(e => e.FrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
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
            new Frequency { Id = (int)FrequencyEnum.Weekly,       Name = FrequencyEnum.Weekly.GetDescription(),       FrequencyPerYear = 52 },
            new Frequency { Id = (int)FrequencyEnum.BiWeekly,     Name = FrequencyEnum.BiWeekly.GetDescription(),     FrequencyPerYear = 26 },
            new Frequency { Id = (int)FrequencyEnum.Monthly,      Name = FrequencyEnum.Monthly.GetDescription(),      FrequencyPerYear = 12 },
            new Frequency { Id = (int)FrequencyEnum.BiMonthly,    Name = FrequencyEnum.BiMonthly.GetDescription(),    FrequencyPerYear = 6  },
            new Frequency { Id = (int)FrequencyEnum.Quarterly,    Name = FrequencyEnum.Quarterly.GetDescription(),    FrequencyPerYear = 4  },
            new Frequency { Id = (int)FrequencyEnum.SemiAnnually, Name = FrequencyEnum.SemiAnnually.GetDescription(), FrequencyPerYear = 2  },
            new Frequency { Id = (int)FrequencyEnum.Annually,     Name = FrequencyEnum.Annually.GetDescription(),     FrequencyPerYear = 1  }
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
    public DbSet<OnboardingPersonalInfo> OnboardingPersonalInfos { get; set; }
    public DbSet<OnboardingAssets> OnboardingAssets { get; set; }
    public DbSet<OnboardingDebt> OnboardingDebts { get; set; }
    public DbSet<OnboardingEmployment> OnboardingEmployments { get; set; }
    public DbSet<DashboardScenario> DashboardScenarios { get; set; }
    public DbSet<EmploymentIncome> EmploymentIncomes { get; set; }
    public DbSet<OtherEmploymentIncome> OtherEmploymentIncomes { get; set; }

}