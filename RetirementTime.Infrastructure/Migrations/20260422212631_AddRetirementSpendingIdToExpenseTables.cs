using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRetirementSpendingIdToExpenseTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RetirementSpendingId",
                table: "dashboard_spending_other_expense",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementSpendingId",
                table: "dashboard_spending_living_expenses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementSpendingId",
                table: "dashboard_spending_discretionary_expenses",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementSpendingId",
                table: "dashboard_spending_debt_repayment",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementSpendingId",
                table: "dashboard_spending_assets_expense",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_other_expense_RetirementSpendingId",
                table: "dashboard_spending_other_expense",
                column: "RetirementSpendingId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_RetirementSpendingId",
                table: "dashboard_spending_living_expenses",
                column: "RetirementSpendingId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_discretionary_expenses_RetirementSpendin~",
                table: "dashboard_spending_discretionary_expenses",
                column: "RetirementSpendingId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_debt_repayment_RetirementSpendingId",
                table: "dashboard_spending_debt_repayment",
                column: "RetirementSpendingId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_assets_expense_RetirementSpendingId",
                table: "dashboard_spending_assets_expense",
                column: "RetirementSpendingId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_retirement_spen~",
                table: "dashboard_spending_assets_expense",
                column: "RetirementSpendingId",
                principalTable: "dashboard_retirement_spending",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_retirement_spen~",
                table: "dashboard_spending_debt_repayment",
                column: "RetirementSpendingId",
                principalTable: "dashboard_retirement_spending",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_dashboard_retirem~",
                table: "dashboard_spending_discretionary_expenses",
                column: "RetirementSpendingId",
                principalTable: "dashboard_retirement_spending",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_living_expenses_dashboard_retirement_spe~",
                table: "dashboard_spending_living_expenses",
                column: "RetirementSpendingId",
                principalTable: "dashboard_retirement_spending",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_other_expense_dashboard_retirement_spend~",
                table: "dashboard_spending_other_expense",
                column: "RetirementSpendingId",
                principalTable: "dashboard_retirement_spending",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_retirement_spen~",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_retirement_spen~",
                table: "dashboard_spending_debt_repayment");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_dashboard_retirem~",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_living_expenses_dashboard_retirement_spe~",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_other_expense_dashboard_retirement_spend~",
                table: "dashboard_spending_other_expense");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_other_expense_RetirementSpendingId",
                table: "dashboard_spending_other_expense");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_living_expenses_RetirementSpendingId",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_discretionary_expenses_RetirementSpendin~",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_debt_repayment_RetirementSpendingId",
                table: "dashboard_spending_debt_repayment");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_assets_expense_RetirementSpendingId",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropColumn(
                name: "RetirementSpendingId",
                table: "dashboard_spending_other_expense");

            migrationBuilder.DropColumn(
                name: "RetirementSpendingId",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropColumn(
                name: "RetirementSpendingId",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropColumn(
                name: "RetirementSpendingId",
                table: "dashboard_spending_debt_repayment");

            migrationBuilder.DropColumn(
                name: "RetirementSpendingId",
                table: "dashboard_spending_assets_expense");
        }
    }
}
