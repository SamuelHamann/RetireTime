using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDashboardIncomeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dashboard_employment_income",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    EmployerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GrossSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrossCommissions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetCommissions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GrossBonus = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    NetBonus = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_employment_income", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_other_employment_income",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EmploymentIncomeId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Gross = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    Net = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_other_employment_income", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_other_employment_income_dashboard_employment_inco~",
                        column: x => x.EmploymentIncomeId,
                        principalTable: "dashboard_employment_income",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_employment_income_ScenarioId",
                table: "dashboard_employment_income",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_employment_income_UserId",
                table: "dashboard_employment_income",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_other_employment_income_EmploymentIncomeId",
                table: "dashboard_other_employment_income",
                column: "EmploymentIncomeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dashboard_other_employment_income");

            migrationBuilder.DropTable(
                name: "dashboard_employment_income");
        }
    }
}
