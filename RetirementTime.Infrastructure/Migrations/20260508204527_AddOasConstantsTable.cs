using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOasConstantsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "oas_constants",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MonthlyRate65To74 = table.Column<decimal>(type: "numeric(10,4)", nullable: false),
                    MonthlyRate75Plus = table.Column<decimal>(type: "numeric(10,4)", nullable: false),
                    ClawbackThreshold = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    EliminationThreshold65To74 = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    EliminationThreshold75Plus = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    DeferralRatePerMonth = table.Column<decimal>(type: "numeric(6,4)", nullable: false),
                    MinResidencyYears = table.Column<int>(type: "integer", nullable: false),
                    FullResidencyYears = table.Column<int>(type: "integer", nullable: false),
                    StandardStartAge = table.Column<int>(type: "integer", nullable: false),
                    MaxDeferralAge = table.Column<int>(type: "integer", nullable: false),
                    PeriodLabel = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oas_constants", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "oas_constants",
                columns: new[] { "Id", "ClawbackThreshold", "CreatedAt", "DeferralRatePerMonth", "EliminationThreshold65To74", "EliminationThreshold75Plus", "FullResidencyYears", "MaxDeferralAge", "MinResidencyYears", "MonthlyRate65To74", "MonthlyRate75Plus", "PeriodLabel", "StandardStartAge", "UpdatedAt" },
                values: new object[] { 1L, 90997m, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0.006m, 152062m, 157490m, 40, 70, 10, 742.31m, 816.54m, "2026-Q1", 65, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "oas_constants");
        }
    }
}
