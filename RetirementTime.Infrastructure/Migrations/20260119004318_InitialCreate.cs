using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "real_estate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    MonthlyElectricityCosts = table.Column<double>(type: "double precision", nullable: false),
                    MonthlyInsuranceCosts = table.Column<double>(type: "double precision", nullable: false),
                    PercentYearlyExpenses = table.Column<double>(type: "double precision", nullable: false),
                    YearlyTaxesPercent = table.Column<double>(type: "double precision", nullable: false),
                    YearlyAppreciation = table.Column<double>(type: "double precision", nullable: false),
                    YearlyHoaCosts = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_real_estate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MonthlyRent = table.Column<double>(type: "double precision", nullable: false),
                    YearlyRentIncrease = table.Column<double>(type: "double precision", nullable: false),
                    MonthlyElectricityCosts = table.Column<double>(type: "double precision", nullable: false),
                    MonthlyInsuranceCosts = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "subdivision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subdivision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_subdivision_country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mortgage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    InterestRate = table.Column<double>(type: "double precision", nullable: false),
                    TermInYears = table.Column<int>(type: "integer", nullable: false),
                    DownPayment = table.Column<double>(type: "double precision", nullable: false),
                    MonthlyMortgageInsuranceCosts = table.Column<double>(type: "double precision", nullable: false),
                    ClosingCosts = table.Column<double>(type: "double precision", nullable: false),
                    AdditionalCosts = table.Column<double>(type: "double precision", nullable: false),
                    AdditionalMonthlyCosts = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    RealEstateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mortgage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_mortgage_real_estate_RealEstateId",
                        column: x => x.RealEstateId,
                        principalTable: "real_estate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    SubdivisionId = table.Column<int>(type: "integer", nullable: true),
                    SpouseId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_subdivision_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "subdivision",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_user_user_SpouseId",
                        column: x => x.SpouseId,
                        principalTable: "user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "buy_or_rent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PercentDifferenceReinvested = table.Column<double>(type: "double precision", nullable: false),
                    PercentDifferenceReinvestedGrowthRate = table.Column<double>(type: "double precision", nullable: false),
                    ComparisonTimeframeInYears = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    RentId = table.Column<int>(type: "integer", nullable: false),
                    MortgageId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_buy_or_rent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_buy_or_rent_mortgage_MortgageId",
                        column: x => x.MortgageId,
                        principalTable: "mortgage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_buy_or_rent_rent_RentId",
                        column: x => x.RentId,
                        principalTable: "rent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    ValidUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC' + INTERVAL '30 minutes'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_session_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "country",
                columns: new[] { "Id", "CountryCode", "Name" },
                values: new object[,]
                {
                    { 1, "CA", "Canada" },
                    { 2, "US", "United States" }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "subdivision",
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
                    { 10, "SK", 1, "Saskatchewan" },
                    { 11, "NT", 1, "Northwest Territories" },
                    { 12, "NU", 1, "Nunavut" },
                    { 13, "YT", 1, "Yukon" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_buy_or_rent_MortgageId",
                table: "buy_or_rent",
                column: "MortgageId");

            migrationBuilder.CreateIndex(
                name: "IX_buy_or_rent_RentId",
                table: "buy_or_rent",
                column: "RentId");

            migrationBuilder.CreateIndex(
                name: "IX_mortgage_RealEstateId",
                table: "mortgage",
                column: "RealEstateId");

            migrationBuilder.CreateIndex(
                name: "IX_session_UserId",
                table: "session",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_subdivision_CountryId",
                table: "subdivision",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_user_CountryId",
                table: "user",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_user_RoleId",
                table: "user",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_user_SpouseId",
                table: "user",
                column: "SpouseId");

            migrationBuilder.CreateIndex(
                name: "IX_user_SubdivisionId",
                table: "user",
                column: "SubdivisionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "buy_or_rent");

            migrationBuilder.DropTable(
                name: "session");

            migrationBuilder.DropTable(
                name: "mortgage");

            migrationBuilder.DropTable(
                name: "rent");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "real_estate");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "subdivision");

            migrationBuilder.DropTable(
                name: "country");
        }
    }
}
