using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscretionaryGroupedEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~3",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~4",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~5",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~6",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.AddColumn<decimal>(
                name: "GroupedAmount",
                table: "dashboard_spending_discretionary_expenses",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupedFrequencyId",
                table: "dashboard_spending_discretionary_expenses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "UseGroupedEntry",
                table: "dashboard_spending_discretionary_expenses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_discretionary_expenses_GroupedFrequencyId",
                table: "dashboard_spending_discretionary_expenses",
                column: "GroupedFrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~3",
                table: "dashboard_spending_discretionary_expenses",
                column: "GroupedFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~4",
                table: "dashboard_spending_discretionary_expenses",
                column: "GymMembershipFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~5",
                table: "dashboard_spending_discretionary_expenses",
                column: "OtherDiscretionaryExpensesFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~6",
                table: "dashboard_spending_discretionary_expenses",
                column: "SubscriptionsFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~7",
                table: "dashboard_spending_discretionary_expenses",
                column: "TravelFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~3",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~4",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~5",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~6",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~7",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_discretionary_expenses_GroupedFrequencyId",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropColumn(
                name: "GroupedAmount",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropColumn(
                name: "GroupedFrequencyId",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropColumn(
                name: "UseGroupedEntry",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~3",
                table: "dashboard_spending_discretionary_expenses",
                column: "GymMembershipFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~4",
                table: "dashboard_spending_discretionary_expenses",
                column: "OtherDiscretionaryExpensesFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~5",
                table: "dashboard_spending_discretionary_expenses",
                column: "SubscriptionsFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~6",
                table: "dashboard_spending_discretionary_expenses",
                column: "TravelFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
