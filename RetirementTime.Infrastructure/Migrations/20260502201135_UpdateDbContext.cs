using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dashboard_income_real_estate");

            migrationBuilder.DropTable(
                name: "dashboard_persistent_income_other");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dashboard_income_real_estate",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FrequencyId = table.Column<int>(type: "integer", nullable: true),
                    InvestmentPropertyId = table.Column<long>(type: "bigint", nullable: true),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    PropertyName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
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
                        name: "FK_dashboard_income_real_estate_dashboard_assets_investment_pr~",
                        column: x => x.InvestmentPropertyId,
                        principalTable: "dashboard_assets_investment_property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_dashboard_income_real_estate_dashboard_scenario_ScenarioId",
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
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Taxable = table.Column<bool>(type: "boolean", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_real_estate_FrequencyId",
                table: "dashboard_income_real_estate",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_real_estate_InvestmentPropertyId",
                table: "dashboard_income_real_estate",
                column: "InvestmentPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_real_estate_ScenarioId",
                table: "dashboard_income_real_estate",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_other_FrequencyId",
                table: "dashboard_persistent_income_other",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_persistent_income_other_ScenarioId",
                table: "dashboard_persistent_income_other",
                column: "ScenarioId");
        }
    }
}
