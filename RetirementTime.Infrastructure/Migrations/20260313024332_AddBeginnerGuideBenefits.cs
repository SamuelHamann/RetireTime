using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBeginnerGuideBenefits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "beginner_guide_benefits_government_pension",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    YearsWorked = table.Column<int>(type: "integer", nullable: false),
                    HasSpecializedPublicSectorPension = table.Column<bool>(type: "boolean", nullable: false),
                    SpecializedPensionName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_benefits_government_pension", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_benefits_government_pension_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_benefits_other_recurring_gains",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    SourceName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_benefits_other_recurring_gains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_benefits_other_recurring_gains_common_freque~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_beginner_guide_benefits_other_recurring_gains_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_benefits_government_pension_UserId",
                table: "beginner_guide_benefits_government_pension",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_benefits_other_recurring_gains_FrequencyId",
                table: "beginner_guide_benefits_other_recurring_gains",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_benefits_other_recurring_gains_UserId",
                table: "beginner_guide_benefits_other_recurring_gains",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "beginner_guide_benefits_government_pension");

            migrationBuilder.DropTable(
                name: "beginner_guide_benefits_other_recurring_gains");
        }
    }
}
