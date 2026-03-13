using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBeginnerGuidePensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "beginner_guide_benefits_pension_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_benefits_pension_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_benefits_pensions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PensionTypeId = table.Column<int>(type: "integer", nullable: false),
                    EmployerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_benefits_pensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_benefits_pensions_beginner_guide_benefits_pe~",
                        column: x => x.PensionTypeId,
                        principalTable: "beginner_guide_benefits_pension_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_beginner_guide_benefits_pensions_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "beginner_guide_benefits_pension_types",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Employer-funded plan that pays a fixed monthly benefit at retirement based on salary and years of service.", "Defined Benefit Pension Plan (DBPP)" },
                    { 2, "Contributions from employer and/or employee are invested; retirement income depends on investment performance.", "Defined Contribution Pension Plan (DCPP)" },
                    { 3, "Large-scale pension plan for employees and self-employed individuals not covered by workplace plans.", "Pooled Registered Pension Plan (PRPP)" },
                    { 4, "Employer-sponsored RRSP where contributions are made by both employer and employee.", "Group Registered Retirement Savings Plan (GRSP)" },
                    { 5, "Hybrid plan that targets a specific retirement benefit but adjusts contributions or benefits based on fund performance.", "Target Benefit Plan" },
                    { 6, "Employer shares a portion of profits with employees, held in trust until retirement.", "Deferred Profit Sharing Plan (DPSP)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_benefits_pensions_PensionTypeId",
                table: "beginner_guide_benefits_pensions",
                column: "PensionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_benefits_pensions_UserId",
                table: "beginner_guide_benefits_pensions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "beginner_guide_benefits_pensions");

            migrationBuilder.DropTable(
                name: "beginner_guide_benefits_pension_types");
        }
    }
}
