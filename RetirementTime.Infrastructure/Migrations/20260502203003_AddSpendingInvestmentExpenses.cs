using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpendingInvestmentExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dashboard_spending_investment_expenses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    InvestmentAccountId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_spending_investment_expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_investment_expenses_common_frequencies_F~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_investment_expenses_dashboard_assets_inv~",
                        column: x => x.InvestmentAccountId,
                        principalTable: "dashboard_assets_investment_account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_investment_expenses_dashboard_scenario_S~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_investment_expenses_FrequencyId",
                table: "dashboard_spending_investment_expenses",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_investment_expenses_InvestmentAccountId",
                table: "dashboard_spending_investment_expenses",
                column: "InvestmentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_investment_expenses_ScenarioId",
                table: "dashboard_spending_investment_expenses",
                column: "ScenarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dashboard_spending_investment_expenses");
        }
    }
}
