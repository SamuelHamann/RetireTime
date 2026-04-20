using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "beginner_guide_assets_investment_properties");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_main_residence");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_other_assets");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_stock_data");

            migrationBuilder.DropTable(
                name: "beginner_guide_benefits_government_pension");

            migrationBuilder.DropTable(
                name: "beginner_guide_benefits_other_recurring_gains");

            migrationBuilder.DropTable(
                name: "beginner_guide_benefits_pensions");

            migrationBuilder.DropTable(
                name: "beginner_guide_debt_debts");

            migrationBuilder.DropTable(
                name: "beginner_guide_income_additional_compensations");

            migrationBuilder.DropTable(
                name: "beginner_guide_income_self_employment_additional_compensations");

            migrationBuilder.DropTable(
                name: "user_progress");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_asset_type");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_investment_accounts");

            migrationBuilder.DropTable(
                name: "beginner_guide_benefits_pension_types");

            migrationBuilder.DropTable(
                name: "beginner_guide_income_employments");

            migrationBuilder.DropTable(
                name: "beginner_guide_income_self_employments");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_account_types");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "subdivision",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "subdivision",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_account_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    SubdivisionId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_assets_account_types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_account_types_country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_account_types_subdivision_Subdivision~",
                        column: x => x.SubdivisionId,
                        principalTable: "subdivision",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_asset_type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_assets_asset_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_investment_properties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    EstimatedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlyCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyElectricityCosts = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlyMortgagePayments = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyRevenue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MortgageDuration = table.Column<int>(type: "integer", nullable: false),
                    MortgageLeft = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MortgageStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    YearlyInsurance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_assets_investment_properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_investment_properties_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_main_residence",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    EstimatedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    HasMainResidence = table.Column<bool>(type: "boolean", nullable: false),
                    MonthlyElectricityCosts = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlyMortgagePayments = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MortgageDuration = table.Column<int>(type: "integer", nullable: true),
                    MortgageLeft = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MortgageStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    YearlyInsurance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_assets_main_residence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_main_residence_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_benefits_government_pension",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    HasSpecializedPublicSectorPension = table.Column<bool>(type: "boolean", nullable: false),
                    SpecializedPensionName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    YearsWorked = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_benefits_government_pension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_benefits_government_pension_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_benefits_other_recurring_gains",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    SourceName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_benefits_other_recurring_gains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_benefits_other_recurring_gains_common_freque~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_beginner_guide_benefits_other_recurring_gains_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_benefits_pension_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_benefits_pension_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_debt_debts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    InterestRate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    MonthlyPayment = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_debt_debts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_debt_debts_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_income_employments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnnualSalary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AverageAnnualWageIncrease = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    EmployerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_income_employments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_income_employments_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_income_self_employments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnnualSalary = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    AverageAnnualRevenueIncrease = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    BusinessName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    MonthlyDividends = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_income_self_employments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_income_self_employments_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_progress",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    HasFinishedBeginnerGuide = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_progress", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_user_progress_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_investment_accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AccountName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BulkAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    IsBulkAmount = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_assets_investment_accounts", x => x.Id);
                    table.CheckConstraint("CK_InvestmentAccount_BulkAmount_XOR_Stocks", "(\"IsBulkAmount\" = true AND \"BulkAmount\" IS NOT NULL) OR (\"IsBulkAmount\" = false AND \"BulkAmount\" IS NULL)");
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_investment_accounts_beginner_guide_as~",
                        column: x => x.AccountTypeId,
                        principalTable: "beginner_guide_assets_account_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_investment_accounts_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_other_assets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetTypeId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    CurrentValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_assets_other_assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_other_assets_beginner_guide_assets_as~",
                        column: x => x.AssetTypeId,
                        principalTable: "beginner_guide_assets_asset_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_other_assets_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_benefits_pensions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PensionTypeId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    EmployerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_benefits_pensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_benefits_pensions_beginner_guide_benefits_pe~",
                        column: x => x.PensionTypeId,
                        principalTable: "beginner_guide_benefits_pension_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_beginner_guide_benefits_pensions_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_income_additional_compensations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmploymentId = table.Column<long>(type: "bigint", nullable: false),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false, defaultValue: 7),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_income_additional_compensations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_income_additional_compensations_beginner_gui~",
                        column: x => x.EmploymentId,
                        principalTable: "beginner_guide_income_employments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_beginner_guide_income_additional_compensations_common_frequ~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_income_self_employment_additional_compensations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false, defaultValue: 7),
                    SelfEmploymentId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_income_self_employment_additional_compensati~", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_income_self_employment_additional_compensati~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_beginner_guide_income_self_employment_additional_compensat~1",
                        column: x => x.SelfEmploymentId,
                        principalTable: "beginner_guide_income_self_employments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_stock_data",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvestmentAccountId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TickerSymbol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_assets_stock_data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_stock_data_beginner_guide_assets_inve~",
                        column: x => x.InvestmentAccountId,
                        principalTable: "beginner_guide_assets_investment_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "beginner_guide_assets_account_types",
                columns: new[] { "Id", "CountryId", "Name", "SubdivisionId" },
                values: new object[,]
                {
                    { 1, 1, "TFSA (Tax-Free Savings Account)", null },
                    { 2, 1, "RRSP (Registered Retirement Savings Plan)", null },
                    { 3, 1, "FHSA (First Home Savings Account)", null },
                    { 4, 1, "RESP (Registered Education Savings Plan)", null },
                    { 5, 1, "RRIF (Registered Retirement Income Fund)", null },
                    { 6, 1, "LIRA (Locked-In Retirement Account)", null },
                    { 7, 1, "Non-Registered Investment Account", null },
                    { 8, 1, "Cash Account", null },
                    { 9, 1, "Margin Account", null },
                    { 10, 2, "401(k)", null },
                    { 11, 2, "Traditional IRA", null },
                    { 12, 2, "Roth IRA", null },
                    { 13, 2, "SEP IRA", null },
                    { 14, 2, "SIMPLE IRA", null },
                    { 15, 2, "403(b)", null },
                    { 16, 2, "457 Plan", null },
                    { 17, 2, "Thrift Savings Plan (TSP)", null },
                    { 18, 2, "529 Education Savings Plan", null },
                    { 19, 2, "HSA (Health Savings Account)", null },
                    { 20, 2, "Brokerage Account (Taxable)", null },
                    { 21, 2, "Cash Management Account", null }
                });

            migrationBuilder.InsertData(
                table: "beginner_guide_assets_asset_type",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Gold, silver, platinum, and other precious metals", "Precious Metals" },
                    { 2, "Cars, motorcycles, boats, RVs, etc.", "Vehicles" },
                    { 3, "Fine jewelry, watches, and collectible pieces", "Jewelry" },
                    { 4, "Physical cash holdings", "Cash" },
                    { 5, "Bitcoin, Ethereum, and other digital currencies", "Cryptocurrency" },
                    { 6, "Paintings, sculptures, rare collectibles", "Art & Collectibles" },
                    { 7, "Antique furniture, vintage items", "Antiques" },
                    { 8, "Collectible wines and rare spirits", "Wine & Spirits" },
                    { 9, "Professional or recreational equipment", "Equipment & Tools" },
                    { 10, "Other valuable assets not listed above", "Other" }
                });

            migrationBuilder.InsertData(
                table: "beginner_guide_benefits_pension_types",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Employer-funded plan that pays a fixed monthly benefit at retirement based on salary and years of service.", "Defined Benefit Pension Plan (DBPP)" },
                    { 2, "Contributions from employer and/or employee are invested; retirement income depends on investment performance.", "Defined Contribution Pension Plan (DCPP)" },
                    { 3, "Large-scale pension plan for employees and self-employed individuals not covered by workplace plans.", "Pooled Registered Pension Plan (PRPP)" },
                    { 4, "Employer-sponsored RRSP where contributions are made by both employer and employee.", "Group Registered Retirement Savings Plan (GRSP)" },
                    { 5, "Hybrid plan that targets a specific retirement benefit but adjusts contributions or benefits based on fund performance.", "Target Benefit Plan" },
                    { 6, "Employer shares a portion of profits with employees, held in trust until retirement.", "Deferred Profit Sharing Plan (DPSP)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_account_types_CountryId",
                table: "beginner_guide_assets_account_types",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_account_types_SubdivisionId",
                table: "beginner_guide_assets_account_types",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_investment_accounts_AccountTypeId",
                table: "beginner_guide_assets_investment_accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_investment_accounts_UserId",
                table: "beginner_guide_assets_investment_accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_investment_properties_UserId",
                table: "beginner_guide_assets_investment_properties",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_main_residence_UserId",
                table: "beginner_guide_assets_main_residence",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_other_assets_AssetTypeId",
                table: "beginner_guide_assets_other_assets",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_other_assets_UserId",
                table: "beginner_guide_assets_other_assets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_stock_data_InvestmentAccountId",
                table: "beginner_guide_assets_stock_data",
                column: "InvestmentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_benefits_government_pension_UserId",
                table: "beginner_guide_benefits_government_pension",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_benefits_other_recurring_gains_FrequencyId",
                table: "beginner_guide_benefits_other_recurring_gains",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_benefits_other_recurring_gains_UserId",
                table: "beginner_guide_benefits_other_recurring_gains",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_benefits_pensions_PensionTypeId",
                table: "beginner_guide_benefits_pensions",
                column: "PensionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_benefits_pensions_UserId",
                table: "beginner_guide_benefits_pensions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_debt_debts_UserId",
                table: "beginner_guide_debt_debts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_income_additional_compensations_EmploymentId",
                table: "beginner_guide_income_additional_compensations",
                column: "EmploymentId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_income_additional_compensations_FrequencyId",
                table: "beginner_guide_income_additional_compensations",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_income_employments_UserId",
                table: "beginner_guide_income_employments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_income_self_employment_additional_compensat~1",
                table: "beginner_guide_income_self_employment_additional_compensations",
                column: "SelfEmploymentId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_income_self_employment_additional_compensati~",
                table: "beginner_guide_income_self_employment_additional_compensations",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_income_self_employments_UserId",
                table: "beginner_guide_income_self_employments",
                column: "UserId");
        }
    }
}
