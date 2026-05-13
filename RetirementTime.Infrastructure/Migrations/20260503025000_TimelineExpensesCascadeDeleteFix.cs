using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TimelineExpensesCascadeDeleteFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_retirement_time~",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_retirement_time~",
                table: "dashboard_spending_assets_expense",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_retirement_time~",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_retirement_time~",
                table: "dashboard_spending_assets_expense",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id");
        }
    }
}
