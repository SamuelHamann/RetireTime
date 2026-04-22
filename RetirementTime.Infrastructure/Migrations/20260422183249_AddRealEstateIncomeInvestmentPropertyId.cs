using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRealEstateIncomeInvestmentPropertyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dashboard_persistent_income_employment");

            migrationBuilder.DropTable(
                name: "dashboard_persistent_income_self_employment");

            migrationBuilder.AddColumn<long>(
                name: "InvestmentPropertyId",
                table: "dashboard_income_real_estate",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_real_estate_InvestmentPropertyId",
                table: "dashboard_income_real_estate",
                column: "InvestmentPropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_real_estate_dashboard_assets_investment_pr~",
                table: "dashboard_income_real_estate",
                column: "InvestmentPropertyId",
                principalTable: "dashboard_assets_investment_property",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_real_estate_dashboard_assets_investment_pr~",
                table: "dashboard_income_real_estate");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_real_estate_InvestmentPropertyId",
                table: "dashboard_income_real_estate");

            migrationBuilder.DropColumn(
                name: "InvestmentPropertyId",
                table: "dashboard_income_real_estate");

            migrationBuilder.CreateTable(
                name: "dashboard_persistent_income_employment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CppDeductionFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    GrossBonusFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    GrossCommissionsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    GrossSalaryFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    NetBonusFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    NetCommissionsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    NetSalaryFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    OtherDeductionFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    PensionContributionFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    TaxDeductionFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CppDeductions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    EmployerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GrossBonus = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrossCommissions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrossSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetBonus = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetCommissions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    OtherDeductions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    PensionContributions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TaxDeductions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
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
                name: "dashboard_persistent_income_self_employment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrossDividendsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    GrossSalaryFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    NetDividendsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    NetSalaryFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    GrossDividends = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrossSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NetDividends = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
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
        }
    }
}
