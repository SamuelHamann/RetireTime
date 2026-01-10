using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedLocationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "CountryCode", "Name" },
                values: new object[,]
                {
                    { 1, "CA", "Canada" },
                    { 2, "US", "United States" }
                });

            migrationBuilder.InsertData(
                table: "Subdivisions",
                columns: new[] { "Id", "Code", "CountryId", "Name" },
                values: new object[,]
                {
                    { 1, "AB", 1, "Alberta" },
                    { 2, "BC", 1, "British Columbia" },
                    { 3, "MB", 1, "Manitoba" },
                    { 4, "NB", 1, "New Brunswick" },
                    { 5, "NL", 1, "Newfoundland and Labrador" },
                    { 6, "NS", 1, "Nova Scotia" },
                    { 7, "ON", 1, "Ontario" },
                    { 8, "PE", 1, "Prince Edward Island" },
                    { 9, "QC", 1, "Quebec" },
                    { 10, "SK", 1, "Saskatchewan" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subdivisions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subdivisions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subdivisions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Subdivisions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Subdivisions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Subdivisions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Subdivisions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Subdivisions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Subdivisions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Subdivisions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
