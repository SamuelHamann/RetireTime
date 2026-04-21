using Microsoft.EntityFrameworkCore;
using RetirementTime.Domain.Entities;
using RetirementTime.Domain.Entities.Dashboard;
using RetirementTime.Domain.Entities.Dashboard.Income;
using RetirementTime.Domain.Entities.Dashboard.Debt;
using RetirementTime.Domain.Entities.Location;
using RetirementTime.Domain.Entities.Onboarding;
using RetirementTime.Domain.Entities.RealEstate;
using RetirementTime.Domain.Entities.Common;
using RetirementTime.Domain.Entities.Dashboard.Asset;
using RetirementTime.Domain.Entities.Dashboard.PersistingIncome;
using RetirementTime.Domain.Entities.Dashboard.Spending;

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
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);

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
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
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

        modelBuilder.Entity<PostRetirementSelfEmployment>(entity =>
        {
            entity.ToTable("dashboard_persistent_income_self_employment");
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

        modelBuilder.Entity<PostRetirementEmployment>(entity =>
        {
            entity.ToTable("dashboard_persistent_income_employment");
            entity.HasKey(e => e.Id);

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
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ScenarioId);
        });

        modelBuilder.Entity<OtherPersistingIncome>(entity =>
        {
            entity.ToTable("dashboard_persistent_income_other");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.Amount)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.FrequencyId)
                .IsRequired();
            
            entity.Property(e => e.Taxable)
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
            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<AssetsHome>(entity =>
        {
            entity.ToTable("dashboard_assets_home");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.HomeValue)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.PurchasePrice)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.PurchaseDate);
            
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
        
        modelBuilder.Entity<AssetsInvestmentProperty>(entity =>
        {
            entity.ToTable("dashboard_assets_investment_property");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValue(string.Empty);
            entity.Property(e => e.PropertyValue)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.PurchasePrice)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.PurchaseDate);
            
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
        
        modelBuilder.Entity<AssetsInvestmentAccount>(entity =>
            {
            entity.ToTable("dashboard_assets_investment_account");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.AccountName)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.UseIndividualHoldings)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.AdjustedCostBasis)
                .HasColumnType("numeric(18,2)");
            
            entity.Property(e => e.CurrentTotalValue)
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
            entity.HasOne(e => e.AccountType)
                .WithMany()
                .HasForeignKey(e => e.AccountTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(e => e.Holdings)
                .WithOne()
                .HasForeignKey(h => h.InvestmentAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<AssetsHolding>(entity => 
            {
            entity.ToTable("dashboard_assets_holding");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.AssetName)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.AdjustedCostBasis)
                .HasColumnType("numeric(18,2)");
            
            entity.Property(e => e.CurrentValue)
                .HasColumnType("numeric(18,2)");
            
            entity.Property(e => e.IsPubliclyTraded)
                .IsRequired();
            
            entity.Property(e => e.TickerSymbol)
                .HasMaxLength(20);
            
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            
            entity.HasOne(e => e.InvestmentAccount)
                .WithMany(a => a.Holdings)
                .HasForeignKey(e => e.InvestmentAccountId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<AccountType>(entity => 
        {
            entity.ToTable("account_type");
            entity.HasKey(at => at.Id);
            
            entity.Property(at => at.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
        });
        
        modelBuilder.Entity<AssetsPhysicalAsset>(entity => 
        {
            entity.ToTable("dashboard_assets_physical_asset");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.EstimatedValue)
                .HasColumnType("numeric(18,2)");
            
            entity.Property(e => e.AdjustedCostBasis)
                .HasColumnType("numeric(18,2)");
            
            entity.Property(e => e.IsConsideredAsARetirementAsset)
                .IsRequired();
            
            entity.Property(e => e.IsConsideredPersonalProperty)
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
            entity.HasOne(e => e.PhysicalAssetType)
                .WithMany()
                .HasForeignKey(e => e.AssetTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<PhysicalAssetType>(entity => 
            {
            entity.ToTable("physical_asset_type");
            entity.HasKey(at => at.Id);

            entity.Property(at => at.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
        });

        modelBuilder.Entity<DebtType>(entity =>
        {
            entity.ToTable("debt_type");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
        });

        modelBuilder.Entity<GenericDebt>(entity =>
        {
            entity.ToTable("dashboard_debt");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.IsHomeMortgage)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.DebtAgainstAssetId);

            entity.Property(e => e.Balance)
                .HasColumnType("numeric(18,2)");

            entity.Property(e => e.InterestRate)
                .HasColumnType("numeric(7,4)");

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

            entity.HasOne(e => e.DebtType)
                .WithMany()
                .HasForeignKey(e => e.DebtTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Frequency)
                .WithMany()
                .HasForeignKey(e => e.FrequencyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<NetWorthHistory>(entity =>
        {
            entity.ToTable("dashboard_net_worth_history");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DateOfSnapShot)
                .IsRequired();
            entity.Property(e => e.Debts)
                .IsRequired();
            entity.Property(e => e.Assets)
                .IsRequired();
            entity.Property(e => e.Debt)
                .IsRequired();
            entity.Property(e => e.Asset)
                .IsRequired();

            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<NetWorthHistory>(entity =>
        {
            entity.ToTable("dashboard_net_worth_history");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.DateOfSnapShot)
                .IsRequired();
            entity.Property(e => e.Debt)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.Asset)
                .HasColumnType("numeric(18,2)");
            entity.Property(e => e.Debts)
                .IsRequired()
                .HasColumnType("text");
            entity.Property(e => e.Assets)
                .IsRequired()
                .HasColumnType("text");

            entity.HasOne(e => e.Scenario)
                .WithMany()
                .HasForeignKey(e => e.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ScenarioId);
        });

        modelBuilder.Entity<SpendingLivingExpenses>(entity =>
        {
            entity.ToTable("dashboard_spending_living_expenses");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.RentOrMortgage).HasColumnType("numeric(18,2)");
            entity.Property(e => e.RentOrMortgageFrequencyId).IsRequired();
            entity.Property(e => e.Food).HasColumnType("numeric(18,2)");
            entity.Property(e => e.FoodFrequencyId).IsRequired();
            entity.Property(e => e.Utilities).HasColumnType("numeric(18,2)");
            entity.Property(e => e.UtilitiesFrequencyId).IsRequired();
            entity.Property(e => e.Insurance).HasColumnType("numeric(18,2)");
            entity.Property(e => e.InsuranceFrequencyId).IsRequired();
            entity.Property(e => e.Gas).HasColumnType("numeric(18,2)");
            entity.Property(e => e.GasFrequencyId).IsRequired();
            entity.Property(e => e.HomeMaintenance).HasColumnType("numeric(18,2)");
            entity.Property(e => e.HomeMaintenanceFrequencyId).IsRequired();
            entity.Property(e => e.Cellphone).HasColumnType("numeric(18,2)");
            entity.Property(e => e.CellphoneFrequencyId).IsRequired();
            entity.Property(e => e.HealthSpendings).HasColumnType("numeric(18,2)");
            entity.Property(e => e.HealthSpendingsFrequencyId).IsRequired();
            entity.Property(e => e.OtherLivingExpenses).HasColumnType("numeric(18,2)");
            entity.Property(e => e.OtherLivingExpensesFrequencyId).IsRequired();

            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.Scenario).WithMany().HasForeignKey(e => e.ScenarioId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.RentOrMortgageFrequency).WithMany().HasForeignKey(e => e.RentOrMortgageFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.FoodFrequency).WithMany().HasForeignKey(e => e.FoodFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.UtilitiesFrequency).WithMany().HasForeignKey(e => e.UtilitiesFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.InsuranceFrequency).WithMany().HasForeignKey(e => e.InsuranceFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.GasFrequency).WithMany().HasForeignKey(e => e.GasFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.HomeMaintenanceFrequency).WithMany().HasForeignKey(e => e.HomeMaintenanceFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.CellphoneFrequency).WithMany().HasForeignKey(e => e.CellphoneFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.HealthSpendingsFrequency).WithMany().HasForeignKey(e => e.HealthSpendingsFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.OtherLivingExpensesFrequency).WithMany().HasForeignKey(e => e.OtherLivingExpensesFrequencyId).OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ScenarioId);
        });

        modelBuilder.Entity<SpendingDiscretionaryExpenses>(entity =>
        {
            entity.ToTable("dashboard_spending_discretionary_expenses");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.GymMembership).HasColumnType("numeric(18,2)");
            entity.Property(e => e.GymMembershipFrequencyId).IsRequired();
            entity.Property(e => e.Subscriptions).HasColumnType("numeric(18,2)");
            entity.Property(e => e.SubscriptionsFrequencyId).IsRequired();
            entity.Property(e => e.EatingOut).HasColumnType("numeric(18,2)");
            entity.Property(e => e.EatingOutFrequencyId).IsRequired();
            entity.Property(e => e.Entertainment).HasColumnType("numeric(18,2)");
            entity.Property(e => e.EntertainmentFrequencyId).IsRequired();
            entity.Property(e => e.Travel).HasColumnType("numeric(18,2)");
            entity.Property(e => e.TravelFrequencyId).IsRequired();
            entity.Property(e => e.CharitableDonations).HasColumnType("numeric(18,2)");
            entity.Property(e => e.CharitableDonationsFrequencyId).IsRequired();
            entity.Property(e => e.OtherDiscretionaryExpenses).HasColumnType("numeric(18,2)");
            entity.Property(e => e.OtherDiscretionaryExpensesFrequencyId).IsRequired();

            entity.Property(e => e.UseGroupedEntry).IsRequired().HasDefaultValue(false);
            entity.Property(e => e.GroupedAmount).HasColumnType("numeric(18,2)");
            entity.Property(e => e.GroupedFrequencyId).IsRequired();

            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.Scenario).WithMany().HasForeignKey(e => e.ScenarioId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.GymMembershipFrequency).WithMany().HasForeignKey(e => e.GymMembershipFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.SubscriptionsFrequency).WithMany().HasForeignKey(e => e.SubscriptionsFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.EatingOutFrequency).WithMany().HasForeignKey(e => e.EatingOutFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.EntertainmentFrequency).WithMany().HasForeignKey(e => e.EntertainmentFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.TravelFrequency).WithMany().HasForeignKey(e => e.TravelFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.CharitableDonationsFrequency).WithMany().HasForeignKey(e => e.CharitableDonationsFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.OtherDiscretionaryExpensesFrequency).WithMany().HasForeignKey(e => e.OtherDiscretionaryExpensesFrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.GroupedFrequency).WithMany().HasForeignKey(e => e.GroupedFrequencyId).OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ScenarioId);
        });

        modelBuilder.Entity<SpendingDebtRepayment>(entity =>
        {
            entity.ToTable("dashboard_spending_debt_repayment");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Amount).HasColumnType("numeric(18,2)");
            entity.Property(e => e.FrequencyId).IsRequired();
            entity.Property(e => e.GenericDebtId);

            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.Scenario).WithMany().HasForeignKey(e => e.ScenarioId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Frequency).WithMany().HasForeignKey(e => e.FrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.GenericDebt).WithMany().HasForeignKey(e => e.GenericDebtId).OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.ScenarioId);
            entity.HasIndex(e => e.GenericDebtId);
        });

        modelBuilder.Entity<SpendingAssetsExpense>(entity =>
        {
            entity.ToTable("dashboard_spending_assets_expense");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Expense).HasColumnType("numeric(18,2)");
            entity.Property(e => e.FrequencyId).IsRequired();
            entity.Property(e => e.AssetsHomeId);
            entity.Property(e => e.AssetsInvestmentPropertyId);
            entity.Property(e => e.AssetsInvestmentAccountId);
            entity.Property(e => e.AssetsPhysicalAssetId);

            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.Scenario).WithMany().HasForeignKey(e => e.ScenarioId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Frequency).WithMany().HasForeignKey(e => e.FrequencyId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.AssetsHome).WithMany().HasForeignKey(e => e.AssetsHomeId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.AssetsInvestmentProperty).WithMany().HasForeignKey(e => e.AssetsInvestmentPropertyId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.AssetsInvestmentAccount).WithMany().HasForeignKey(e => e.AssetsInvestmentAccountId).OnDelete(DeleteBehavior.SetNull);
            entity.HasOne(e => e.AssetsPhysicalAsset).WithMany().HasForeignKey(e => e.AssetsPhysicalAssetId).OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.ScenarioId);
            entity.HasIndex(e => e.AssetsHomeId);
            entity.HasIndex(e => e.AssetsInvestmentPropertyId);
            entity.HasIndex(e => e.AssetsInvestmentAccountId);
            entity.HasIndex(e => e.AssetsPhysicalAssetId);
        });

        modelBuilder.Entity<SpendingOtherExpense>(entity =>
        {
            entity.ToTable("dashboard_spending_other_expense");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Amount).HasColumnType("numeric(18,2)");
            entity.Property(e => e.FrequencyId).IsRequired();

            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

            entity.HasOne(e => e.Scenario).WithMany().HasForeignKey(e => e.ScenarioId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Frequency).WithMany().HasForeignKey(e => e.FrequencyId).OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ScenarioId);
        });

        SeedRoleData(modelBuilder);
        SeedLocationData(modelBuilder);
        SeedLanguageData(modelBuilder);
        SeedFrequencyData(modelBuilder);
        SeedPhysicalAssetTypeData(modelBuilder);
        SeedAccountTypeData(modelBuilder);
        SeedDebtTypeData(modelBuilder);
    }

    private static void SeedDebtTypeData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DebtType>().HasData(
            new DebtType { Id = (long)DebtTypeEnum.Mortgage,               Name = "Mortgage" },
            new DebtType { Id = (long)DebtTypeEnum.HomeEquityLineOfCredit, Name = "Home Equity Line of Credit" },
            new DebtType { Id = (long)DebtTypeEnum.CarLoan,                Name = "Car Loan" },
            new DebtType { Id = (long)DebtTypeEnum.StudentLoan,            Name = "Student Loan" },
            new DebtType { Id = (long)DebtTypeEnum.CreditCard,             Name = "Credit Card" },
            new DebtType { Id = (long)DebtTypeEnum.PersonalLoan,           Name = "Personal Loan" },
            new DebtType { Id = (long)DebtTypeEnum.LineOfCredit,           Name = "Line of Credit" },
            new DebtType { Id = (long)DebtTypeEnum.Other,                  Name = "Other" },
            new DebtType { Id = (long)DebtTypeEnum.Medical,                Name = "Medical" }
        );
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

    private static void SeedPhysicalAssetTypeData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PhysicalAssetType>().HasData(
            new PhysicalAssetType { Id = (long)AssetTypeEnum.Vehicle,        Name = "Vehicle" },
            new PhysicalAssetType { Id = (long)AssetTypeEnum.Collectible,    Name = "Collectible" },
            new PhysicalAssetType { Id = (long)AssetTypeEnum.Jewelry,        Name = "Jewelry" },
            new PhysicalAssetType { Id = (long)AssetTypeEnum.PreciousMetals, Name = "Precious Metals" },
            new PhysicalAssetType { Id = (long)AssetTypeEnum.Other,          Name = "Other" }
        );
    }

    private static void SeedAccountTypeData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountType>().HasData(
            new AccountType { Id = (long)InvestmentAccountType.RRSP,          Name = "RRSP" },
            new AccountType { Id = (long)InvestmentAccountType.RRIF,          Name = "RRIF" },
            new AccountType { Id = (long)InvestmentAccountType.TFSA,          Name = "TFSA" },
            new AccountType { Id = (long)InvestmentAccountType.RESP,          Name = "RESP" },
            new AccountType { Id = (long)InvestmentAccountType.RDSP,          Name = "RDSP" },
            new AccountType { Id = (long)InvestmentAccountType.FHSA,          Name = "FHSA" },
            new AccountType { Id = (long)InvestmentAccountType.NonRegistered, Name = "Non-Registered" },
            new AccountType { Id = (long)InvestmentAccountType.LIRA,          Name = "LIRA" },
            new AccountType { Id = (long)InvestmentAccountType.LIF,           Name = "LIF" },
            new AccountType { Id = (long)InvestmentAccountType.PRIF,          Name = "PRIF" },
            new AccountType { Id = (long)InvestmentAccountType.RLSP,          Name = "RLSP" },
            new AccountType { Id = (long)InvestmentAccountType.RLIF,          Name = "RLIF" }
        );
    }

    // Reference data
    public DbSet<Role> Roles { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Frequency> Frequencies { get; set; }
    public DbSet<PhysicalAssetType> PhysicalAssetTypes { get; set; }
    public DbSet<AccountType> AccountTypes { get; set; }

    // Location
    public DbSet<Country> Countries { get; set; }
    public DbSet<Subdivision> Subdivisions { get; set; }

    // Users
    public DbSet<User> Users { get; set; }

    // Real estate (legacy)
    public DbSet<RealEstate> RealEstates { get; set; }
    public DbSet<Mortgage> Mortgages { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<BuyOrRent> BuyOrRents { get; set; }

    // Onboarding
    public DbSet<OnboardingPersonalInfo> OnboardingPersonalInfos { get; set; }
    public DbSet<OnboardingAssets> OnboardingAssets { get; set; }
    public DbSet<OnboardingDebt> OnboardingDebts { get; set; }
    public DbSet<OnboardingEmployment> OnboardingEmployments { get; set; }

    // Dashboard
    public DbSet<DashboardScenario> DashboardScenarios { get; set; }

    // Dashboard — Income
    public DbSet<EmploymentIncome> EmploymentIncomes { get; set; }
    public DbSet<OtherEmploymentIncome> OtherEmploymentIncomes { get; set; }
    public DbSet<SelfEmploymentIncome> SelfEmploymentIncomes { get; set; }
    public DbSet<PensionDefinedBenefits> PensionDefinedBenefits { get; set; }
    public DbSet<PensionDefinedContribution> PensionDefinedContributions { get; set; }
    public DbSet<GroupRrsp> GroupRrsps { get; set; }
    public DbSet<DefinedProfitSharing> DefinedProfitSharings { get; set; }
    public DbSet<SharePurchasePlan> SharePurchasePlans { get; set; }
    public DbSet<OasCppIncome> OasCppIncomes { get; set; }
    public DbSet<OtherIncomeOrBenefits> OtherIncomeOrBenefits { get; set; }
    public DbSet<RealEstateIncome> RealEstateIncomes { get; set; }

    // Dashboard — Persisting Income (post-retirement)
    public DbSet<PostRetirementSelfEmployment> PostRetirementSelfEmployments { get; set; }
    public DbSet<PostRetirementEmployment> PostRetirementEmployments { get; set; }
    public DbSet<OtherPersistingIncome> OtherPersistingIncomes { get; set; }

    // Dashboard — Assets
    public DbSet<AssetsHome> AssetsHomes { get; set; }
    public DbSet<AssetsInvestmentProperty> AssetsInvestmentProperties { get; set; }
    public DbSet<AssetsInvestmentAccount> AssetsInvestmentAccounts { get; set; }
    public DbSet<AssetsHolding> AssetsHoldings { get; set; }
    public DbSet<AssetsPhysicalAsset> AssetsPhysicalAssets { get; set; }

    // Dashboard — Debt
    public DbSet<GenericDebt> GenericDebts { get; set; }
    public DbSet<DebtType> DebtTypes { get; set; }

    // Dashboard — Net Worth
    public DbSet<NetWorthHistory> NetWorthHistories { get; set; }

    // Dashboard — Spending
    public DbSet<SpendingLivingExpenses> SpendingLivingExpenses { get; set; }
    public DbSet<SpendingDiscretionaryExpenses> SpendingDiscretionaryExpenses { get; set; }
    public DbSet<SpendingDebtRepayment> SpendingDebtRepayments { get; set; }
    public DbSet<SpendingAssetsExpense> SpendingAssetsExpenses { get; set; }
    public DbSet<SpendingOtherExpense> SpendingOtherExpenses { get; set; }

}