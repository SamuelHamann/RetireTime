using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOtherAssetsAndAssetTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asset_type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "other_asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AssetTypeId = table.Column<int>(type: "integer", nullable: false),
                    CurrentValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_other_asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_other_asset_asset_type_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalTable: "asset_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_other_asset_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "asset_type",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Gold, silver, platinum, and other precious metals", "Precious Metals" },
                    { 2, "Cars, motorcycles, boats, RVs, etc.", "Vehicles" },
                    { 3, "Fine jewelry, watches, and collectible pieces", "Jewelry" },
                    { 4, "Physical cash holdings", "Cash" },
                    { 5, "Bitcoin, Ethereum, and other digital currencies", "Cryptocurrency" },
                    { 6, "Paintings, sculptures, rare collectibles", "Art & Collectibles" },
                    { 7, "Antique furniture, vintage items", "Antiques" },
                    { 8, "Collectible wines and rare spirits", "Wine & Spirits" },
                    { 9, "Professional or recreational equipment", "Equipment & Tools" },
                    { 10, "Other valuable assets not listed above", "Other" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_other_asset_AssetTypeId",
                table: "other_asset",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_other_asset_UserId",
                table: "other_asset",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "other_asset");

            migrationBuilder.DropTable(
                name: "asset_type");
        }
    }
}
