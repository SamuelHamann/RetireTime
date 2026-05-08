using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyIncome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dashboard_income_property_income",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    RetirementTimelineId = table.Column<long>(type: "bigint", nullable: true),
                    InvestmentPropertyId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_income_property_income", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_income_property_income_common_frequencies_Frequen~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_income_property_income_dashboard_assets_investmen~",
                        column: x => x.InvestmentPropertyId,
                        principalTable: "dashboard_assets_investment_property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_dashboard_income_property_income_dashboard_retirement_timel~",
                        column: x => x.RetirementTimelineId,
                        principalTable: "dashboard_retirement_timeline",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dashboard_income_property_income_dashboard_scenario_Scenari~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_property_income_FrequencyId",
                table: "dashboard_income_property_income",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_property_income_InvestmentPropertyId",
                table: "dashboard_income_property_income",
                column: "InvestmentPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_property_income_RetirementTimelineId",
                table: "dashboard_income_property_income",
                column: "RetirementTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_property_income_ScenarioId",
                table: "dashboard_income_property_income",
                column: "ScenarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dashboard_income_property_income");
        }
    }
}
