using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFrequencyAndUpdateIncome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_beginner_guide_income_self_employment_additional_compensati~",
                table: "beginner_guide_income_self_employment_additional_compensations");

            migrationBuilder.RenameColumn(
                name: "AnnualRevenue",
                table: "beginner_guide_income_self_employments",
                newName: "AnnualSalary");

            migrationBuilder.RenameIndex(
                name: "IX_beginner_guide_income_self_employment_additional_compensati~",
                table: "beginner_guide_income_self_employment_additional_compensations",
                newName: "IX_beginner_guide_income_self_employment_additional_compensat~1");

            migrationBuilder.AddColumn<int>(
                name: "FrequencyId",
                table: "beginner_guide_income_self_employment_additional_compensations",
                type: "integer",
                nullable: false,
                defaultValue: 7);

            migrationBuilder.AddColumn<int>(
                name: "FrequencyId",
                table: "beginner_guide_income_additional_compensations",
                type: "integer",
                nullable: false,
                defaultValue: 7);

            migrationBuilder.CreateTable(
                name: "common_frequencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FrequencyPerYear = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_common_frequencies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "common_frequencies",
                columns: new[] { "Id", "FrequencyPerYear", "Name" },
                values: new object[,]
                {
                    { 1, 52, "Weekly" },
                    { 2, 26, "Bi-Weekly" },
                    { 3, 12, "Monthly" },
                    { 4, 6, "Bi-Monthly" },
                    { 5, 4, "Quarterly" },
                    { 6, 2, "Semi-Annually" },
                    { 7, 1, "Annually" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_income_self_employment_additional_compensati~",
                table: "beginner_guide_income_self_employment_additional_compensations",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_income_additional_compensations_FrequencyId",
                table: "beginner_guide_income_additional_compensations",
                column: "FrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_beginner_guide_income_additional_compensations_common_frequ~",
                table: "beginner_guide_income_additional_compensations",
                column: "FrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_beginner_guide_income_self_employment_additional_compensati~",
                table: "beginner_guide_income_self_employment_additional_compensations",
                column: "FrequencyId",
                principalTable: "common_frequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_beginner_guide_income_self_employment_additional_compensat~1",
                table: "beginner_guide_income_self_employment_additional_compensations",
                column: "SelfEmploymentId",
                principalTable: "beginner_guide_income_self_employments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_beginner_guide_income_additional_compensations_common_frequ~",
                table: "beginner_guide_income_additional_compensations");

            migrationBuilder.DropForeignKey(
                name: "FK_beginner_guide_income_self_employment_additional_compensati~",
                table: "beginner_guide_income_self_employment_additional_compensations");

            migrationBuilder.DropForeignKey(
                name: "FK_beginner_guide_income_self_employment_additional_compensat~1",
                table: "beginner_guide_income_self_employment_additional_compensations");

            migrationBuilder.DropTable(
                name: "common_frequencies");

            migrationBuilder.DropIndex(
                name: "IX_beginner_guide_income_self_employment_additional_compensati~",
                table: "beginner_guide_income_self_employment_additional_compensations");

            migrationBuilder.DropIndex(
                name: "IX_beginner_guide_income_additional_compensations_FrequencyId",
                table: "beginner_guide_income_additional_compensations");

            migrationBuilder.DropColumn(
                name: "FrequencyId",
                table: "beginner_guide_income_self_employment_additional_compensations");

            migrationBuilder.DropColumn(
                name: "FrequencyId",
                table: "beginner_guide_income_additional_compensations");

            migrationBuilder.RenameColumn(
                name: "AnnualSalary",
                table: "beginner_guide_income_self_employments",
                newName: "AnnualRevenue");

            migrationBuilder.RenameIndex(
                name: "IX_beginner_guide_income_self_employment_additional_compensat~1",
                table: "beginner_guide_income_self_employment_additional_compensations",
                newName: "IX_beginner_guide_income_self_employment_additional_compensati~");

            migrationBuilder.AddForeignKey(
                name: "FK_beginner_guide_income_self_employment_additional_compensati~",
                table: "beginner_guide_income_self_employment_additional_compensations",
                column: "SelfEmploymentId",
                principalTable: "beginner_guide_income_self_employments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
