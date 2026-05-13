using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TimelineExpensesCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_retirement_time~",
                table: "dashboard_spending_debt_repayment");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_dashboard_retirem~",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_investment_expenses_dashboard_retirement~",
                table: "dashboard_spending_investment_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_living_expenses_dashboard_retirement_tim~",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_other_expense_dashboard_retirement_timel~",
                table: "dashboard_spending_other_expense");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_retirement_time~",
                table: "dashboard_spending_debt_repayment",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_dashboard_retirem~",
                table: "dashboard_spending_discretionary_expenses",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_investment_expenses_dashboard_retirement~",
                table: "dashboard_spending_investment_expenses",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_living_expenses_dashboard_retirement_tim~",
                table: "dashboard_spending_living_expenses",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_other_expense_dashboard_retirement_timel~",
                table: "dashboard_spending_other_expense",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_retirement_time~",
                table: "dashboard_spending_debt_repayment");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_dashboard_retirem~",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_investment_expenses_dashboard_retirement~",
                table: "dashboard_spending_investment_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_living_expenses_dashboard_retirement_tim~",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_other_expense_dashboard_retirement_timel~",
                table: "dashboard_spending_other_expense");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_retirement_time~",
                table: "dashboard_spending_debt_repayment",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_dashboard_retirem~",
                table: "dashboard_spending_discretionary_expenses",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_investment_expenses_dashboard_retirement~",
                table: "dashboard_spending_investment_expenses",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_living_expenses_dashboard_retirement_tim~",
                table: "dashboard_spending_living_expenses",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_other_expense_dashboard_retirement_timel~",
                table: "dashboard_spending_other_expense",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
