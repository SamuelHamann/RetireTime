using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRemainingIncomeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossBonusFr~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossCommiss~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossSalaryF~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetBonusFreq~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetCommissio~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetSalaryFre~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_other_employment_income_common_frequencies_GrossF~",
                table: "dashboard_other_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_other_employment_income_common_frequencies_NetFre~",
                table: "dashboard_other_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_other_employment_income_dashboard_employment_inco~",
                table: "dashboard_other_employment_income");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dashboard_other_employment_income",
                table: "dashboard_other_employment_income");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dashboard_employment_income",
                table: "dashboard_employment_income");

            migrationBuilder.RenameTable(
                name: "dashboard_other_employment_income",
                newName: "dashboard_income_other_employment_income");

            migrationBuilder.RenameTable(
                name: "dashboard_employment_income",
                newName: "dashboard_income_employment_income");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_other_employment_income_NetFrequencyId",
                table: "dashboard_income_other_employment_income",
                newName: "IX_dashboard_income_other_employment_income_NetFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_other_employment_income_GrossFrequencyId",
                table: "dashboard_income_other_employment_income",
                newName: "IX_dashboard_income_other_employment_income_GrossFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_other_employment_income_EmploymentIncomeId",
                table: "dashboard_income_other_employment_income",
                newName: "IX_dashboard_income_other_employment_income_EmploymentIncomeId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_employment_income_UserId",
                table: "dashboard_income_employment_income",
                newName: "IX_dashboard_income_employment_income_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_employment_income_ScenarioId",
                table: "dashboard_income_employment_income",
                newName: "IX_dashboard_income_employment_income_ScenarioId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_employment_income_NetSalaryFrequencyId",
                table: "dashboard_income_employment_income",
                newName: "IX_dashboard_income_employment_income_NetSalaryFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_employment_income_NetCommissionsFrequencyId",
                table: "dashboard_income_employment_income",
                newName: "IX_dashboard_income_employment_income_NetCommissionsFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_employment_income_NetBonusFrequencyId",
                table: "dashboard_income_employment_income",
                newName: "IX_dashboard_income_employment_income_NetBonusFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_employment_income_GrossSalaryFrequencyId",
                table: "dashboard_income_employment_income",
                newName: "IX_dashboard_income_employment_income_GrossSalaryFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_employment_income_GrossCommissionsFrequencyId",
                table: "dashboard_income_employment_income",
                newName: "IX_dashboard_income_employment_income_GrossCommissionsFrequenc~");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_employment_income_GrossBonusFrequencyId",
                table: "dashboard_income_employment_income",
                newName: "IX_dashboard_income_employment_income_GrossBonusFrequencyId");

            migrationBuilder.AddColumn<int>(
                name: "CppDeductionFrequencyId",
                table: "dashboard_income_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CppDeductions",
                table: "dashboard_income_employment_income",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OtherDeductionFrequencyId",
                table: "dashboard_income_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherDeductions",
                table: "dashboard_income_employment_income",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PensionContributionFrequencyId",
                table: "dashboard_income_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "PensionContributions",
                table: "dashboard_income_employment_income",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaxDeductionFrequencyId",
                table: "dashboard_income_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxDeductions",
                table: "dashboard_income_employment_income",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_dashboard_income_other_employment_income",
                table: "dashboard_income_other_employment_income",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dashboard_income_employment_income",
                table: "dashboard_income_employment_income",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "dashboard_income_defined_profit_sharing",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PercentOfSalaryEmployer = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_income_defined_profit_sharing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_income_defined_profit_sharing_dashboard_scenario_~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_income_group_rrsp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PercentOfSalaryEmployee = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    PercentOfSalaryEmployer = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_income_group_rrsp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_income_group_rrsp_dashboard_scenario_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_income_oas_cpp",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    IncomeLastYear = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Income2YearsAgo = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Income3YearsAgo = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Income4YearsAgo = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Income5YearsAgo = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    YearsSpentInCanada = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_income_oas_cpp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_income_oas_cpp_dashboard_scenario_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_income_other_income_or_benefits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_income_other_income_or_benefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_income_other_income_or_benefits_common_frequencie~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_income_other_income_or_benefits_dashboard_scenari~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_income_pension_defined_benefits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    StartAge = table.Column<int>(type: "integer", nullable: false),
                    BenefitsPre65 = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    BenefitsPre65FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    BenefitsPost65 = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    BenefitsPost65FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    PercentIndexedToInflation = table.Column<int>(type: "integer", nullable: false),
                    PercentSurvivorBenefits = table.Column<int>(type: "integer", nullable: false),
                    RrspAdjustment = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    RrspAdjustmentFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_income_pension_defined_benefits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_income_pension_defined_benefits_common_frequencie~",
                        column: x => x.BenefitsPost65FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_income_pension_defined_benefits_common_frequenci~1",
                        column: x => x.BenefitsPre65FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_income_pension_defined_benefits_common_frequenci~2",
                        column: x => x.RrspAdjustmentFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_income_pension_defined_benefits_dashboard_scenari~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_income_pension_defined_contribution",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PercentOfSalaryEmployee = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    PercentOfSalaryEmployer = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_income_pension_defined_contribution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_income_pension_defined_contribution_dashboard_sce~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_income_self_employment_income",
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
                    table.PrimaryKey("PK_dashboard_income_self_employment_income", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_income_self_employment_income_common_frequencies_~",
                        column: x => x.GrossDividendsFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_income_self_employment_income_common_frequencies~1",
                        column: x => x.GrossSalaryFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_income_self_employment_income_common_frequencies~2",
                        column: x => x.NetDividendsFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_income_self_employment_income_common_frequencies~3",
                        column: x => x.NetSalaryFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_income_self_employment_income_dashboard_scenario_~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_income_share_purchase_plan",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PercentOfSalaryEmployee = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    PurchaseFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    PercentOfSalaryEmployer = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    EmployerMatchFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    UseFlatAmountInsteadOfPercent = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_income_share_purchase_plan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_income_share_purchase_plan_common_frequencies_Emp~",
                        column: x => x.EmployerMatchFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dashboard_income_share_purchase_plan_common_frequencies_Pur~",
                        column: x => x.PurchaseFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dashboard_income_share_purchase_plan_dashboard_scenario_Sce~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_employment_income_CppDeductionFrequencyId",
                table: "dashboard_income_employment_income",
                column: "CppDeductionFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_employment_income_OtherDeductionFrequencyId",
                table: "dashboard_income_employment_income",
                column: "OtherDeductionFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_employment_income_PensionContributionFrequ~",
                table: "dashboard_income_employment_income",
                column: "PensionContributionFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_employment_income_TaxDeductionFrequencyId",
                table: "dashboard_income_employment_income",
                column: "TaxDeductionFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_defined_profit_sharing_ScenarioId",
                table: "dashboard_income_defined_profit_sharing",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_group_rrsp_ScenarioId",
                table: "dashboard_income_group_rrsp",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_oas_cpp_ScenarioId",
                table: "dashboard_income_oas_cpp",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_other_income_or_benefits_FrequencyId",
                table: "dashboard_income_other_income_or_benefits",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_other_income_or_benefits_ScenarioId",
                table: "dashboard_income_other_income_or_benefits",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_pension_defined_benefits_BenefitsPost65Fre~",
                table: "dashboard_income_pension_defined_benefits",
                column: "BenefitsPost65FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_pension_defined_benefits_BenefitsPre65Freq~",
                table: "dashboard_income_pension_defined_benefits",
                column: "BenefitsPre65FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_pension_defined_benefits_RrspAdjustmentFre~",
                table: "dashboard_income_pension_defined_benefits",
                column: "RrspAdjustmentFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_pension_defined_benefits_ScenarioId",
                table: "dashboard_income_pension_defined_benefits",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_pension_defined_contribution_ScenarioId",
                table: "dashboard_income_pension_defined_contribution",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_self_employment_income_GrossDividendsFrequ~",
                table: "dashboard_income_self_employment_income",
                column: "GrossDividendsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_self_employment_income_GrossSalaryFrequenc~",
                table: "dashboard_income_self_employment_income",
                column: "GrossSalaryFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_self_employment_income_NetDividendsFrequen~",
                table: "dashboard_income_self_employment_income",
                column: "NetDividendsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_self_employment_income_NetSalaryFrequencyId",
                table: "dashboard_income_self_employment_income",
                column: "NetSalaryFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_self_employment_income_ScenarioId",
                table: "dashboard_income_self_employment_income",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_share_purchase_plan_EmployerMatchFrequency~",
                table: "dashboard_income_share_purchase_plan",
                column: "EmployerMatchFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_share_purchase_plan_PurchaseFrequencyId",
                table: "dashboard_income_share_purchase_plan",
                column: "PurchaseFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_share_purchase_plan_ScenarioId",
                table: "dashboard_income_share_purchase_plan",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_CppDe~",
                table: "dashboard_income_employment_income",
                column: "CppDeductionFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_Gross~",
                table: "dashboard_income_employment_income",
                column: "GrossBonusFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_Gros~1",
                table: "dashboard_income_employment_income",
                column: "GrossCommissionsFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_Gros~2",
                table: "dashboard_income_employment_income",
                column: "GrossSalaryFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_NetBo~",
                table: "dashboard_income_employment_income",
                column: "NetBonusFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_NetCo~",
                table: "dashboard_income_employment_income",
                column: "NetCommissionsFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_NetSa~",
                table: "dashboard_income_employment_income",
                column: "NetSalaryFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_Other~",
                table: "dashboard_income_employment_income",
                column: "OtherDeductionFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_Pensi~",
                table: "dashboard_income_employment_income",
                column: "PensionContributionFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_TaxDe~",
                table: "dashboard_income_employment_income",
                column: "TaxDeductionFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_other_employment_income_common_frequencies~",
                table: "dashboard_income_other_employment_income",
                column: "GrossFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_other_employment_income_common_frequencie~1",
                table: "dashboard_income_other_employment_income",
                column: "NetFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_other_employment_income_dashboard_income_e~",
                table: "dashboard_income_other_employment_income",
                column: "EmploymentIncomeId",
                principalTable: "dashboard_income_employment_income",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_CppDe~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_Gross~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_Gros~1",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_Gros~2",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_NetBo~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_NetCo~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_NetSa~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_Other~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_Pensi~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_common_frequencies_TaxDe~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_other_employment_income_common_frequencies~",
                table: "dashboard_income_other_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_other_employment_income_common_frequencie~1",
                table: "dashboard_income_other_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_other_employment_income_dashboard_income_e~",
                table: "dashboard_income_other_employment_income");

            migrationBuilder.DropTable(
                name: "dashboard_income_defined_profit_sharing");

            migrationBuilder.DropTable(
                name: "dashboard_income_group_rrsp");

            migrationBuilder.DropTable(
                name: "dashboard_income_oas_cpp");

            migrationBuilder.DropTable(
                name: "dashboard_income_other_income_or_benefits");

            migrationBuilder.DropTable(
                name: "dashboard_income_pension_defined_benefits");

            migrationBuilder.DropTable(
                name: "dashboard_income_pension_defined_contribution");

            migrationBuilder.DropTable(
                name: "dashboard_income_self_employment_income");

            migrationBuilder.DropTable(
                name: "dashboard_income_share_purchase_plan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dashboard_income_other_employment_income",
                table: "dashboard_income_other_employment_income");

            migrationBuilder.DropPrimaryKey(
                name: "PK_dashboard_income_employment_income",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_employment_income_CppDeductionFrequencyId",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_employment_income_OtherDeductionFrequencyId",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_employment_income_PensionContributionFrequ~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_employment_income_TaxDeductionFrequencyId",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropColumn(
                name: "CppDeductionFrequencyId",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropColumn(
                name: "CppDeductions",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropColumn(
                name: "OtherDeductionFrequencyId",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropColumn(
                name: "OtherDeductions",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropColumn(
                name: "PensionContributionFrequencyId",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropColumn(
                name: "PensionContributions",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropColumn(
                name: "TaxDeductionFrequencyId",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropColumn(
                name: "TaxDeductions",
                table: "dashboard_income_employment_income");

            migrationBuilder.RenameTable(
                name: "dashboard_income_other_employment_income",
                newName: "dashboard_other_employment_income");

            migrationBuilder.RenameTable(
                name: "dashboard_income_employment_income",
                newName: "dashboard_employment_income");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_other_employment_income_NetFrequencyId",
                table: "dashboard_other_employment_income",
                newName: "IX_dashboard_other_employment_income_NetFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_other_employment_income_GrossFrequencyId",
                table: "dashboard_other_employment_income",
                newName: "IX_dashboard_other_employment_income_GrossFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_other_employment_income_EmploymentIncomeId",
                table: "dashboard_other_employment_income",
                newName: "IX_dashboard_other_employment_income_EmploymentIncomeId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_employment_income_UserId",
                table: "dashboard_employment_income",
                newName: "IX_dashboard_employment_income_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_employment_income_ScenarioId",
                table: "dashboard_employment_income",
                newName: "IX_dashboard_employment_income_ScenarioId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_employment_income_NetSalaryFrequencyId",
                table: "dashboard_employment_income",
                newName: "IX_dashboard_employment_income_NetSalaryFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_employment_income_NetCommissionsFrequencyId",
                table: "dashboard_employment_income",
                newName: "IX_dashboard_employment_income_NetCommissionsFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_employment_income_NetBonusFrequencyId",
                table: "dashboard_employment_income",
                newName: "IX_dashboard_employment_income_NetBonusFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_employment_income_GrossSalaryFrequencyId",
                table: "dashboard_employment_income",
                newName: "IX_dashboard_employment_income_GrossSalaryFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_employment_income_GrossCommissionsFrequenc~",
                table: "dashboard_employment_income",
                newName: "IX_dashboard_employment_income_GrossCommissionsFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_income_employment_income_GrossBonusFrequencyId",
                table: "dashboard_employment_income",
                newName: "IX_dashboard_employment_income_GrossBonusFrequencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dashboard_other_employment_income",
                table: "dashboard_other_employment_income",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_dashboard_employment_income",
                table: "dashboard_employment_income",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossBonusFr~",
                table: "dashboard_employment_income",
                column: "GrossBonusFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossCommiss~",
                table: "dashboard_employment_income",
                column: "GrossCommissionsFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossSalaryF~",
                table: "dashboard_employment_income",
                column: "GrossSalaryFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetBonusFreq~",
                table: "dashboard_employment_income",
                column: "NetBonusFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetCommissio~",
                table: "dashboard_employment_income",
                column: "NetCommissionsFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetSalaryFre~",
                table: "dashboard_employment_income",
                column: "NetSalaryFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_other_employment_income_common_frequencies_GrossF~",
                table: "dashboard_other_employment_income",
                column: "GrossFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_other_employment_income_common_frequencies_NetFre~",
                table: "dashboard_other_employment_income",
                column: "NetFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_other_employment_income_dashboard_employment_inco~",
                table: "dashboard_other_employment_income",
                column: "EmploymentIncomeId",
                principalTable: "dashboard_employment_income",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
