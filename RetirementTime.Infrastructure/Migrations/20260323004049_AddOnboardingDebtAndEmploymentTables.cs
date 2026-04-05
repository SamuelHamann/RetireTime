using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOnboardingDebtAndEmploymentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "onboarding_step3_debt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    HasPrimaryMortgage = table.Column<bool>(type: "boolean", nullable: false),
                    HasInvestmentPropertyMortgage = table.Column<bool>(type: "boolean", nullable: false),
                    HasCarPayments = table.Column<bool>(type: "boolean", nullable: false),
                    HasStudentLoans = table.Column<bool>(type: "boolean", nullable: false),
                    HasCreditCardDebt = table.Column<bool>(type: "boolean", nullable: false),
                    HasPersonalLoans = table.Column<bool>(type: "boolean", nullable: false),
                    HasBusinessLoans = table.Column<bool>(type: "boolean", nullable: false),
                    HasTaxDebt = table.Column<bool>(type: "boolean", nullable: false),
                    HasMedicalDebt = table.Column<bool>(type: "boolean", nullable: false),
                    HasInformalDebt = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_onboarding_step3_debt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_onboarding_step3_debt_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "onboarding_step4_employment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsEmployed = table.Column<bool>(type: "boolean", nullable: false),
                    IsSelfEmployed = table.Column<bool>(type: "boolean", nullable: false),
                    PlannedRetirementAge = table.Column<int>(type: "integer", nullable: true),
                    CppContributionYears = table.Column<int>(type: "integer", nullable: true),
                    HasRoyalties = table.Column<bool>(type: "boolean", nullable: false),
                    HasDividends = table.Column<bool>(type: "boolean", nullable: false),
                    HasRentalIncome = table.Column<bool>(type: "boolean", nullable: false),
                    HasOtherIncome = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_onboarding_step4_employment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_onboarding_step4_employment_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_onboarding_step3_debt_UserId",
                table: "onboarding_step3_debt",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_onboarding_step4_employment_UserId",
                table: "onboarding_step4_employment",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "onboarding_step3_debt");

            migrationBuilder.DropTable(
                name: "onboarding_step4_employment");
        }
    }
}
