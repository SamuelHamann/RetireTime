using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPhysicalAssetAndAccountTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_type",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_assets_home",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HomeValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_assets_home", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_assets_home_dashboard_scenario_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_assets_investment_property",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PropertyValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_assets_investment_property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_assets_investment_property_dashboard_scenario_Sce~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_income_real_estate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    PropertyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    FrequencyId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_income_real_estate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_income_real_estate_common_frequencies_FrequencyId",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_income_real_estate_dashboard_scenario_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_persistent_income_employment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    EmployerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GrossSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrossSalaryFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    NetSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetSalaryFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    GrossCommissions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrossCommissionsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    NetCommissions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetCommissionsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    GrossBonus = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrossBonusFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    NetBonus = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetBonusFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    PensionContributions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    PensionContributionFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    TaxDeductions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TaxDeductionFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CppDeductions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CppDeductionFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    OtherDeductions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    OtherDeductionFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_persistent_income_employment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_common_frequencies_C~",
                        column: x => x.CppDeductionFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_common_frequencies_G~",
                        column: x => x.GrossBonusFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_common_frequencies_N~",
                        column: x => x.NetBonusFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_common_frequencies_O~",
                        column: x => x.OtherDeductionFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_common_frequencies_P~",
                        column: x => x.PensionContributionFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_common_frequencies_T~",
                        column: x => x.TaxDeductionFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_common_frequencies_~1",
                        column: x => x.GrossCommissionsFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_common_frequencies_~2",
                        column: x => x.GrossSalaryFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_common_frequencies_~3",
                        column: x => x.NetCommissionsFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_common_frequencies_~4",
                        column: x => x.NetSalaryFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_employment_dashboard_scenario_S~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_persistent_income_other",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    Taxable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_persistent_income_other", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_other_common_frequencies_Freque~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_other_dashboard_scenario_Scenar~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_persistent_income_self_employment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NetSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetSalaryFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    GrossSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrossSalaryFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    GrossDividends = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrossDividendsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    NetDividends = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetDividendsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_persistent_income_self_employment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_self_employment_common_frequenc~",
                        column: x => x.GrossDividendsFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_self_employment_common_frequen~1",
                        column: x => x.GrossSalaryFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_self_employment_common_frequen~2",
                        column: x => x.NetDividendsFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_self_employment_common_frequen~3",
                        column: x => x.NetSalaryFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_persistent_income_self_employment_dashboard_scena~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "physical_asset_type",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_physical_asset_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_assets_investment_account",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    AccountName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AccountTypeId = table.Column<long>(type: "bigint", nullable: false),
                    AdjustedCostBasis = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CurrentTotalValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_assets_investment_account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_assets_investment_account_account_type_AccountTyp~",
                        column: x => x.AccountTypeId,
                        principalTable: "account_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_assets_investment_account_dashboard_scenario_Scen~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_assets_physical_asset",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    AssetTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    AdjustedCostBasis = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    IsConsideredPersonalProperty = table.Column<bool>(type: "boolean", nullable: false),
                    IsConsideredAsARetirementAsset = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_assets_physical_asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_assets_physical_asset_dashboard_scenario_Scenario~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dashboard_assets_physical_asset_physical_asset_type_AssetTy~",
                        column: x => x.AssetTypeId,
                        principalTable: "physical_asset_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_assets_holding",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvestmentAccountId = table.Column<long>(type: "bigint", nullable: false),
                    AssetName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsPubliclyTraded = table.Column<bool>(type: "boolean", nullable: false),
                    CurrentValue = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TickerSymbol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AdjustedCostBasis = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_assets_holding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_assets_holding_dashboard_assets_investment_accoun~",
                        column: x => x.InvestmentAccountId,
                        principalTable: "dashboard_assets_investment_account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "account_type",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "RRSP" },
                    { 2L, "RRIF" },
                    { 3L, "TFSA" },
                    { 4L, "RESP" },
                    { 5L, "RDSP" },
                    { 6L, "FHSA" },
                    { 7L, "Non-Registered" },
                    { 8L, "LIRA" },
                    { 9L, "LIF" },
                    { 10L, "PRIF" },
                    { 11L, "RLSP" },
                    { 12L, "RLIF" }
                });

            migrationBuilder.InsertData(
                table: "physical_asset_type",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Vehicle" },
                    { 2L, "Collectible" },
                    { 3L, "Jewelry" },
                    { 4L, "Precious Metals" },
                    { 5L, "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_assets_holding_InvestmentAccountId",
                table: "dashboard_assets_holding",
                column: "InvestmentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_assets_home_ScenarioId",
                table: "dashboard_assets_home",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_assets_investment_account_AccountTypeId",
                table: "dashboard_assets_investment_account",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_assets_investment_account_ScenarioId",
                table: "dashboard_assets_investment_account",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_assets_investment_property_ScenarioId",
                table: "dashboard_assets_investment_property",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_assets_physical_asset_AssetTypeId",
                table: "dashboard_assets_physical_asset",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_assets_physical_asset_ScenarioId",
                table: "dashboard_assets_physical_asset",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_real_estate_FrequencyId",
                table: "dashboard_income_real_estate",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_real_estate_ScenarioId",
                table: "dashboard_income_real_estate",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_CppDeductionFrequenc~",
                table: "dashboard_persistent_income_employment",
                column: "CppDeductionFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_GrossBonusFrequencyId",
                table: "dashboard_persistent_income_employment",
                column: "GrossBonusFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_GrossCommissionsFreq~",
                table: "dashboard_persistent_income_employment",
                column: "GrossCommissionsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_GrossSalaryFrequency~",
                table: "dashboard_persistent_income_employment",
                column: "GrossSalaryFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_NetBonusFrequencyId",
                table: "dashboard_persistent_income_employment",
                column: "NetBonusFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_NetCommissionsFreque~",
                table: "dashboard_persistent_income_employment",
                column: "NetCommissionsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_NetSalaryFrequencyId",
                table: "dashboard_persistent_income_employment",
                column: "NetSalaryFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_OtherDeductionFreque~",
                table: "dashboard_persistent_income_employment",
                column: "OtherDeductionFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_PensionContributionF~",
                table: "dashboard_persistent_income_employment",
                column: "PensionContributionFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_ScenarioId",
                table: "dashboard_persistent_income_employment",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_employment_TaxDeductionFrequenc~",
                table: "dashboard_persistent_income_employment",
                column: "TaxDeductionFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_other_FrequencyId",
                table: "dashboard_persistent_income_other",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_other_ScenarioId",
                table: "dashboard_persistent_income_other",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_self_employment_GrossDividendsF~",
                table: "dashboard_persistent_income_self_employment",
                column: "GrossDividendsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_self_employment_GrossSalaryFreq~",
                table: "dashboard_persistent_income_self_employment",
                column: "GrossSalaryFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_self_employment_NetDividendsFre~",
                table: "dashboard_persistent_income_self_employment",
                column: "NetDividendsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_self_employment_NetSalaryFreque~",
                table: "dashboard_persistent_income_self_employment",
                column: "NetSalaryFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_self_employment_ScenarioId",
                table: "dashboard_persistent_income_self_employment",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_dashboard_scenario_Scena~",
                table: "dashboard_income_employment_income",
                column: "ScenarioId",
                principalTable: "dashboard_scenario",
                principalColumn: "ScenarioId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_dashboard_scenario_Scena~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropTable(
                name: "dashboard_assets_holding");

            migrationBuilder.DropTable(
                name: "dashboard_assets_home");

            migrationBuilder.DropTable(
                name: "dashboard_assets_investment_property");

            migrationBuilder.DropTable(
                name: "dashboard_assets_physical_asset");

            migrationBuilder.DropTable(
                name: "dashboard_income_real_estate");

            migrationBuilder.DropTable(
                name: "dashboard_persistent_income_employment");

            migrationBuilder.DropTable(
                name: "dashboard_persistent_income_other");

            migrationBuilder.DropTable(
                name: "dashboard_persistent_income_self_employment");

            migrationBuilder.DropTable(
                name: "dashboard_assets_investment_account");

            migrationBuilder.DropTable(
                name: "physical_asset_type");

            migrationBuilder.DropTable(
                name: "account_type");
        }
    }
}
