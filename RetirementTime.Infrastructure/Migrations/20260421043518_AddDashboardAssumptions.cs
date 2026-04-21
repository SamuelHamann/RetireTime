using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDashboardAssumptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dashboard_assumptions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    YearlyInflationRate = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    YearlyPropertyAppreciation = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    StockAllocation = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    StockYearlyReturn = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    StockYearlyDividend = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    StockCanadianAllocation = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    StockForeignAllocation = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    StockFees = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    BondAllocation = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    BondYearlyReturn = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    BondFees = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    CashAllocation = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    CashYearlyReturn = table.Column<decimal>(type: "numeric(5,4)", precision: 5, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_assumptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_assumptions_dashboard_scenario_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_assumptions_ScenarioId",
                table: "dashboard_assumptions",
                column: "ScenarioId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dashboard_assumptions");
        }
    }
}
