using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRetirementTimelineIdToInvestmentExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RetirementTimelineId",
                table: "dashboard_spending_investment_expenses",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_investment_expenses_RetirementTimelineId",
                table: "dashboard_spending_investment_expenses",
                column: "RetirementTimelineId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_investment_expenses_dashboard_retirement~",
                table: "dashboard_spending_investment_expenses",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_investment_expenses_dashboard_retirement~",
                table: "dashboard_spending_investment_expenses");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_investment_expenses_RetirementTimelineId",
                table: "dashboard_spending_investment_expenses");

            migrationBuilder.DropColumn(
                name: "RetirementTimelineId",
                table: "dashboard_spending_investment_expenses");
        }
    }
}
