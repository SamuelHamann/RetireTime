using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedAccountTypesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    SubdivisionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_type", x => x.Id);
                    table.ForeignKey(
                        name: "FK_account_type_country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_account_type_subdivision_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "subdivision",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "investment_account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsBulkAmount = table.Column<bool>(type: "boolean", nullable: false),
                    BulkAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_investment_account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_investment_account_account_type_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "account_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_investment_account_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "investment_stock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvestmentAccountId = table.Column<int>(type: "integer", nullable: false),
                    TickerSymbol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_investment_stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_investment_stock_investment_account_InvestmentAccountId",
                        column: x => x.InvestmentAccountId,
                        principalTable: "investment_account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "account_type",
                columns: new[] { "Id", "CountryId", "Name", "SubdivisionId" },
                values: new object[,]
                {
                    { 1, 1, "TFSA (Tax-Free Savings Account)", null },
                    { 2, 1, "RRSP (Registered Retirement Savings Plan)", null },
                    { 3, 1, "FHSA (First Home Savings Account)", null },
                    { 4, 1, "RESP (Registered Education Savings Plan)", null },
                    { 5, 1, "RRIF (Registered Retirement Income Fund)", null },
                    { 6, 1, "LIRA (Locked-In Retirement Account)", null },
                    { 7, 1, "Non-Registered Investment Account", null },
                    { 8, 1, "Cash Account", null },
                    { 9, 1, "Margin Account", null },
                    { 10, 2, "401(k)", null },
                    { 11, 2, "Traditional IRA", null },
                    { 12, 2, "Roth IRA", null },
                    { 13, 2, "SEP IRA", null },
                    { 14, 2, "SIMPLE IRA", null },
                    { 15, 2, "403(b)", null },
                    { 16, 2, "457 Plan", null },
                    { 17, 2, "Thrift Savings Plan (TSP)", null },
                    { 18, 2, "529 Education Savings Plan", null },
                    { 19, 2, "HSA (Health Savings Account)", null },
                    { 20, 2, "Brokerage Account (Taxable)", null },
                    { 21, 2, "Cash Management Account", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_type_CountryId",
                table: "account_type",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_account_type_SubdivisionId",
                table: "account_type",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_investment_account_AccountTypeId",
                table: "investment_account",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_investment_account_UserId",
                table: "investment_account",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_investment_stock_InvestmentAccountId",
                table: "investment_stock",
                column: "InvestmentAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "investment_stock");

            migrationBuilder.DropTable(
                name: "investment_account");

            migrationBuilder.DropTable(
                name: "account_type");
        }
    }
}
