﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SplitRentAndMortgageLivingExpenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clear existing rows so the new MortgageFrequencyId FK (NOT NULL) doesn't violate constraints
            migrationBuilder.Sql("DELETE FROM dashboard_spending_living_expenses;");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_living_expenses_common_frequencies_RentO~",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.RenameColumn(
                name: "RentOrMortgageFrequencyId",
                table: "dashboard_spending_living_expenses",
                newName: "RentFrequencyId");

            migrationBuilder.RenameColumn(
                name: "RentOrMortgage",
                table: "dashboard_spending_living_expenses",
                newName: "Rent");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_living_expenses_RentOrMortgageFrequencyId",
                table: "dashboard_spending_living_expenses",
                newName: "IX_dashboard_spending_living_expenses_RentFrequencyId");

            migrationBuilder.AddColumn<decimal>(
                name: "Mortgage",
                table: "dashboard_spending_living_expenses",
                type: "numeric(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MortgageFrequencyId",
                table: "dashboard_spending_living_expenses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_MortgageFrequencyId",
                table: "dashboard_spending_living_expenses",
                column: "MortgageFrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_living_expenses_common_frequencies_Mortg~",
                table: "dashboard_spending_living_expenses",
                column: "MortgageFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_living_expenses_common_frequencies_RentF~",
                table: "dashboard_spending_living_expenses",
                column: "RentFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_living_expenses_common_frequencies_Mortg~",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_living_expenses_common_frequencies_RentF~",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_spending_living_expenses_MortgageFrequencyId",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropColumn(
                name: "Mortgage",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropColumn(
                name: "MortgageFrequencyId",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.RenameColumn(
                name: "RentFrequencyId",
                table: "dashboard_spending_living_expenses",
                newName: "RentOrMortgageFrequencyId");

            migrationBuilder.RenameColumn(
                name: "Rent",
                table: "dashboard_spending_living_expenses",
                newName: "RentOrMortgage");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_living_expenses_RentFrequencyId",
                table: "dashboard_spending_living_expenses",
                newName: "IX_dashboard_spending_living_expenses_RentOrMortgageFrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_living_expenses_common_frequencies_RentO~",
                table: "dashboard_spending_living_expenses",
                column: "RentOrMortgageFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
