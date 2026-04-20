using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpendingDebtRepaymentGenericDebtFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GenericDebtId",
                table: "dashboard_spending_debt_repayment",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_debt_repayment_GenericDebtId",
                table: "dashboard_spending_debt_repayment",
                column: "GenericDebtId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_debt_GenericDeb~",
                table: "dashboard_spending_debt_repayment",
                column: "GenericDebtId",
                principalTable: "dashboard_debt",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_debt_GenericDeb~",
                table: "dashboard_spending_debt_repayment");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_debt_repayment_GenericDebtId",
                table: "dashboard_spending_debt_repayment");

            migrationBuilder.DropColumn(
                name: "GenericDebtId",
                table: "dashboard_spending_debt_repayment");
        }
    }
}
