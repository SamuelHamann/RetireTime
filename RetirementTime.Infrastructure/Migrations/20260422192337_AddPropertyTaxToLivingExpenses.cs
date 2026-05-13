using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyTaxToLivingExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PropertyTax",
                table: "dashboard_spending_living_expenses",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropertyTaxFrequencyId",
                table: "dashboard_spending_living_expenses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_PropertyTaxFrequencyId",
                table: "dashboard_spending_living_expenses",
                column: "PropertyTaxFrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_living_expenses_common_frequencies_Prope~",
                table: "dashboard_spending_living_expenses",
                column: "PropertyTaxFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_living_expenses_common_frequencies_Prope~",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_living_expenses_PropertyTaxFrequencyId",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropColumn(
                name: "PropertyTax",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropColumn(
                name: "PropertyTaxFrequencyId",
                table: "dashboard_spending_living_expenses");
        }
    }
}
