using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGenericDebtAndDebtType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "debt_type",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_debt_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_debt",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    DebtTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Balance = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    InterestRate = table.Column<decimal>(type: "numeric(7,4)", nullable: true),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    TermInYears = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_debt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_debt_common_frequencies_FrequencyId",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_debt_dashboard_scenario_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_dashboard_debt_debt_type_DebtTypeId",
                        column: x => x.DebtTypeId,
                        principalTable: "debt_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "debt_type",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Mortgage" },
                    { 2L, "Home Equity Line of Credit" },
                    { 3L, "Car Loan" },
                    { 4L, "Student Loan" },
                    { 5L, "Credit Card" },
                    { 6L, "Personal Loan" },
                    { 7L, "Line of Credit" },
                    { 8L, "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_debt_DebtTypeId",
                table: "dashboard_debt",
                column: "DebtTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_debt_FrequencyId",
                table: "dashboard_debt",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_debt_ScenarioId",
                table: "dashboard_debt",
                column: "ScenarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dashboard_debt");

            migrationBuilder.DropTable(
                name: "debt_type");
        }
    }
}
