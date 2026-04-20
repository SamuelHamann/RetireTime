using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOnboardingAssetsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "onboarding_step2_assets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    HasSavingsAccount = table.Column<bool>(type: "boolean", nullable: false),
                    HasTFSA = table.Column<bool>(type: "boolean", nullable: false),
                    HasRRSP = table.Column<bool>(type: "boolean", nullable: false),
                    HasRRIF = table.Column<bool>(type: "boolean", nullable: false),
                    HasFHSA = table.Column<bool>(type: "boolean", nullable: false),
                    HasRESP = table.Column<bool>(type: "boolean", nullable: false),
                    HasRDSP = table.Column<bool>(type: "boolean", nullable: false),
                    HasNonRegistered = table.Column<bool>(type: "boolean", nullable: false),
                    HasPension = table.Column<bool>(type: "boolean", nullable: false),
                    HasPrincipalResidence = table.Column<bool>(type: "boolean", nullable: false),
                    HasCar = table.Column<bool>(type: "boolean", nullable: false),
                    HasInvestmentProperty = table.Column<bool>(type: "boolean", nullable: false),
                    HasBusiness = table.Column<bool>(type: "boolean", nullable: false),
                    HasIncorporation = table.Column<bool>(type: "boolean", nullable: false),
                    HasPreciousMetals = table.Column<bool>(type: "boolean", nullable: false),
                    HasOtherHardAssets = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_onboarding_step2_assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_onboarding_step2_assets_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_onboarding_step2_assets_UserId",
                table: "onboarding_step2_assets",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "onboarding_step2_assets");
        }
    }
}
