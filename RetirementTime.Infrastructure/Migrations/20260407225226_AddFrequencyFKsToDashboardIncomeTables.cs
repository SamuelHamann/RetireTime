using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFrequencyFKsToDashboardIncomeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GrossFrequencyId",
                table: "dashboard_other_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NetFrequencyId",
                table: "dashboard_other_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GrossBonusFrequencyId",
                table: "dashboard_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GrossCommissionsFrequencyId",
                table: "dashboard_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GrossSalaryFrequencyId",
                table: "dashboard_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NetBonusFrequencyId",
                table: "dashboard_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NetCommissionsFrequencyId",
                table: "dashboard_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NetSalaryFrequencyId",
                table: "dashboard_employment_income",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_other_employment_income_GrossFrequencyId",
                table: "dashboard_other_employment_income",
                column: "GrossFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_other_employment_income_NetFrequencyId",
                table: "dashboard_other_employment_income",
                column: "NetFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_employment_income_GrossBonusFrequencyId",
                table: "dashboard_employment_income",
                column: "GrossBonusFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_employment_income_GrossCommissionsFrequencyId",
                table: "dashboard_employment_income",
                column: "GrossCommissionsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_employment_income_GrossSalaryFrequencyId",
                table: "dashboard_employment_income",
                column: "GrossSalaryFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_employment_income_NetBonusFrequencyId",
                table: "dashboard_employment_income",
                column: "NetBonusFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_employment_income_NetCommissionsFrequencyId",
                table: "dashboard_employment_income",
                column: "NetCommissionsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_employment_income_NetSalaryFrequencyId",
                table: "dashboard_employment_income",
                column: "NetSalaryFrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossBonusFr~",
                table: "dashboard_employment_income",
                column: "GrossBonusFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossCommiss~",
                table: "dashboard_employment_income",
                column: "GrossCommissionsFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossSalaryF~",
                table: "dashboard_employment_income",
                column: "GrossSalaryFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetBonusFreq~",
                table: "dashboard_employment_income",
                column: "NetBonusFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetCommissio~",
                table: "dashboard_employment_income",
                column: "NetCommissionsFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetSalaryFre~",
                table: "dashboard_employment_income",
                column: "NetSalaryFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_other_employment_income_common_frequencies_GrossF~",
                table: "dashboard_other_employment_income",
                column: "GrossFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_other_employment_income_common_frequencies_NetFre~",
                table: "dashboard_other_employment_income",
                column: "NetFrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossBonusFr~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossCommiss~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_GrossSalaryF~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetBonusFreq~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetCommissio~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_employment_income_common_frequencies_NetSalaryFre~",
                table: "dashboard_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_other_employment_income_common_frequencies_GrossF~",
                table: "dashboard_other_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_other_employment_income_common_frequencies_NetFre~",
                table: "dashboard_other_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_other_employment_income_GrossFrequencyId",
                table: "dashboard_other_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_other_employment_income_NetFrequencyId",
                table: "dashboard_other_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_employment_income_GrossBonusFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_employment_income_GrossCommissionsFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_employment_income_GrossSalaryFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_employment_income_NetBonusFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_employment_income_NetCommissionsFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_employment_income_NetSalaryFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropColumn(
                name: "GrossFrequencyId",
                table: "dashboard_other_employment_income");

            migrationBuilder.DropColumn(
                name: "NetFrequencyId",
                table: "dashboard_other_employment_income");

            migrationBuilder.DropColumn(
                name: "GrossBonusFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropColumn(
                name: "GrossCommissionsFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropColumn(
                name: "GrossSalaryFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropColumn(
                name: "NetBonusFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropColumn(
                name: "NetCommissionsFrequencyId",
                table: "dashboard_employment_income");

            migrationBuilder.DropColumn(
                name: "NetSalaryFrequencyId",
                table: "dashboard_employment_income");
        }
    }
}
