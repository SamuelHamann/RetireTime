using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeRentMortgageFrequencyNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove all existing rows to avoid FK constraint violations during column alteration
            migrationBuilder.Sql("DELETE FROM dashboard_spending_living_expenses;");

            migrationBuilder.AlterColumn<int>(
                name: "RentFrequencyId",
                table: "dashboard_spending_living_expenses",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "MortgageFrequencyId",
                table: "dashboard_spending_living_expenses",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE dashboard_spending_living_expenses SET \"RentFrequencyId\" = 2 WHERE \"RentFrequencyId\" IS NULL;");
            migrationBuilder.Sql(
                "UPDATE dashboard_spending_living_expenses SET \"MortgageFrequencyId\" = 2 WHERE \"MortgageFrequencyId\" IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "RentFrequencyId",
                table: "dashboard_spending_living_expenses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MortgageFrequencyId",
                table: "dashboard_spending_living_expenses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
