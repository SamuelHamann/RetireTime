using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpendingAssetsExpenseAssetFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AssetsHomeId",
                table: "dashboard_spending_assets_expense",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AssetsInvestmentAccountId",
                table: "dashboard_spending_assets_expense",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AssetsInvestmentPropertyId",
                table: "dashboard_spending_assets_expense",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AssetsPhysicalAssetId",
                table: "dashboard_spending_assets_expense",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_assets_expense_AssetsHomeId",
                table: "dashboard_spending_assets_expense",
                column: "AssetsHomeId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_assets_expense_AssetsInvestmentAccountId",
                table: "dashboard_spending_assets_expense",
                column: "AssetsInvestmentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_assets_expense_AssetsInvestmentPropertyId",
                table: "dashboard_spending_assets_expense",
                column: "AssetsInvestmentPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_assets_expense_AssetsPhysicalAssetId",
                table: "dashboard_spending_assets_expense",
                column: "AssetsPhysicalAssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_assets_home_Ass~",
                table: "dashboard_spending_assets_expense",
                column: "AssetsHomeId",
                principalTable: "dashboard_assets_home",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_assets_investme~",
                table: "dashboard_spending_assets_expense",
                column: "AssetsInvestmentAccountId",
                principalTable: "dashboard_assets_investment_account",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_assets_investm~1",
                table: "dashboard_spending_assets_expense",
                column: "AssetsInvestmentPropertyId",
                principalTable: "dashboard_assets_investment_property",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_assets_physical~",
                table: "dashboard_spending_assets_expense",
                column: "AssetsPhysicalAssetId",
                principalTable: "dashboard_assets_physical_asset",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_assets_home_Ass~",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_assets_investme~",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_assets_investm~1",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_assets_physical~",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_assets_expense_AssetsHomeId",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_assets_expense_AssetsInvestmentAccountId",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_assets_expense_AssetsInvestmentPropertyId",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_assets_expense_AssetsPhysicalAssetId",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropColumn(
                name: "AssetsHomeId",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropColumn(
                name: "AssetsInvestmentAccountId",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropColumn(
                name: "AssetsInvestmentPropertyId",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropColumn(
                name: "AssetsPhysicalAssetId",
                table: "dashboard_spending_assets_expense");
        }
    }
}
