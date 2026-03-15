using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInvestmentPropertyAndRelatedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_beginner_guide_debt_user_UserId",
                table: "beginner_guide_debt");

            migrationBuilder.DropForeignKey(
                name: "FK_investment_account_account_type_AccountTypeId",
                table: "investment_account");

            migrationBuilder.DropForeignKey(
                name: "FK_investment_account_user_UserId",
                table: "investment_account");

            migrationBuilder.DropForeignKey(
                name: "FK_investment_stock_investment_account_InvestmentAccountId",
                table: "investment_stock");

            migrationBuilder.DropTable(
                name: "account_type");

            migrationBuilder.DropTable(
                name: "asset_main_residence");

            migrationBuilder.DropTable(
                name: "investment_property");

            migrationBuilder.DropTable(
                name: "other_asset");

            migrationBuilder.DropTable(
                name: "asset_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_investment_stock",
                table: "investment_stock");

            migrationBuilder.DropPrimaryKey(
                name: "PK_investment_account",
                table: "investment_account");

            migrationBuilder.DropPrimaryKey(
                name: "PK_beginner_guide_debt",
                table: "beginner_guide_debt");

            migrationBuilder.RenameTable(
                name: "investment_stock",
                newName: "beginner_guide_assets_stock_data");

            migrationBuilder.RenameTable(
                name: "investment_account",
                newName: "beginner_guide_assets_investment_accounts");

            migrationBuilder.RenameTable(
                name: "beginner_guide_debt",
                newName: "beginner_guide_debt_debts");

            migrationBuilder.RenameIndex(
                name: "IX_investment_stock_InvestmentAccountId",
                table: "beginner_guide_assets_stock_data",
                newName: "IX_beginner_guide_assets_stock_data_InvestmentAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_investment_account_UserId",
                table: "beginner_guide_assets_investment_accounts",
                newName: "IX_beginner_guide_assets_investment_accounts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_investment_account_AccountTypeId",
                table: "beginner_guide_assets_investment_accounts",
                newName: "IX_beginner_guide_assets_investment_accounts_AccountTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_beginner_guide_debt_UserId",
                table: "beginner_guide_debt_debts",
                newName: "IX_beginner_guide_debt_debts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_beginner_guide_assets_stock_data",
                table: "beginner_guide_assets_stock_data",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_beginner_guide_assets_investment_accounts",
                table: "beginner_guide_assets_investment_accounts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_beginner_guide_debt_debts",
                table: "beginner_guide_debt_debts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_account_types",
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
                    table.PrimaryKey("PK_beginner_guide_assets_account_types", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_account_types_country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_account_types_subdivision_Subdivision~",
                        column: x => x.SubdivisionId,
                        principalTable: "subdivision",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_asset_type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_assets_asset_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_investment_properties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyMortgagePayments = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MortgageLeft = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    YearlyInsurance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyElectricityCosts = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MortgageDuration = table.Column<int>(type: "integer", nullable: false),
                    MortgageStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlyCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyRevenue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_assets_investment_properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_investment_properties_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_main_residence",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    HasMainResidence = table.Column<bool>(type: "boolean", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlyMortgagePayments = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MortgageLeft = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    YearlyInsurance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlyElectricityCosts = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MortgageDuration = table.Column<int>(type: "integer", nullable: true),
                    MortgageStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstimatedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beginner_guide_assets_main_residence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_main_residence_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "beginner_guide_assets_other_assets",
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
                    table.PrimaryKey("PK_beginner_guide_assets_other_assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_other_assets_beginner_guide_assets_as~",
                        column: x => x.AssetTypeId,
                        principalTable: "beginner_guide_assets_asset_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_beginner_guide_assets_other_assets_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "beginner_guide_assets_account_types",
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

            migrationBuilder.InsertData(
                table: "beginner_guide_assets_asset_type",
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
                name: "IX_beginner_guide_assets_account_types_CountryId",
                table: "beginner_guide_assets_account_types",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_account_types_SubdivisionId",
                table: "beginner_guide_assets_account_types",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_investment_properties_UserId",
                table: "beginner_guide_assets_investment_properties",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_main_residence_UserId",
                table: "beginner_guide_assets_main_residence",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_other_assets_AssetTypeId",
                table: "beginner_guide_assets_other_assets",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_beginner_guide_assets_other_assets_UserId",
                table: "beginner_guide_assets_other_assets",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_beginner_guide_assets_investment_accounts_beginner_guide_as~",
                table: "beginner_guide_assets_investment_accounts",
                column: "AccountTypeId",
                principalTable: "beginner_guide_assets_account_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_beginner_guide_assets_investment_accounts_user_UserId",
                table: "beginner_guide_assets_investment_accounts",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_beginner_guide_assets_stock_data_beginner_guide_assets_inve~",
                table: "beginner_guide_assets_stock_data",
                column: "InvestmentAccountId",
                principalTable: "beginner_guide_assets_investment_accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_beginner_guide_debt_debts_user_UserId",
                table: "beginner_guide_debt_debts",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_beginner_guide_assets_investment_accounts_beginner_guide_as~",
                table: "beginner_guide_assets_investment_accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_beginner_guide_assets_investment_accounts_user_UserId",
                table: "beginner_guide_assets_investment_accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_beginner_guide_assets_stock_data_beginner_guide_assets_inve~",
                table: "beginner_guide_assets_stock_data");

            migrationBuilder.DropForeignKey(
                name: "FK_beginner_guide_debt_debts_user_UserId",
                table: "beginner_guide_debt_debts");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_account_types");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_investment_properties");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_main_residence");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_other_assets");

            migrationBuilder.DropTable(
                name: "beginner_guide_assets_asset_type");

            migrationBuilder.DropPrimaryKey(
                name: "PK_beginner_guide_debt_debts",
                table: "beginner_guide_debt_debts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_beginner_guide_assets_stock_data",
                table: "beginner_guide_assets_stock_data");

            migrationBuilder.DropPrimaryKey(
                name: "PK_beginner_guide_assets_investment_accounts",
                table: "beginner_guide_assets_investment_accounts");

            migrationBuilder.RenameTable(
                name: "beginner_guide_debt_debts",
                newName: "beginner_guide_debt");

            migrationBuilder.RenameTable(
                name: "beginner_guide_assets_stock_data",
                newName: "investment_stock");

            migrationBuilder.RenameTable(
                name: "beginner_guide_assets_investment_accounts",
                newName: "investment_account");

            migrationBuilder.RenameIndex(
                name: "IX_beginner_guide_debt_debts_UserId",
                table: "beginner_guide_debt",
                newName: "IX_beginner_guide_debt_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_beginner_guide_assets_stock_data_InvestmentAccountId",
                table: "investment_stock",
                newName: "IX_investment_stock_InvestmentAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_beginner_guide_assets_investment_accounts_UserId",
                table: "investment_account",
                newName: "IX_investment_account_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_beginner_guide_assets_investment_accounts_AccountTypeId",
                table: "investment_account",
                newName: "IX_investment_account_AccountTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_beginner_guide_debt",
                table: "beginner_guide_debt",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_investment_stock",
                table: "investment_stock",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_investment_account",
                table: "investment_account",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "account_type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    SubdivisionId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
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
                name: "asset_main_residence",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    EstimatedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    HasMainResidence = table.Column<bool>(type: "boolean", nullable: false),
                    MonthlyElectricityCosts = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlyMortgagePayments = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MortgageDuration = table.Column<int>(type: "integer", nullable: true),
                    MortgageLeft = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MortgageStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    YearlyInsurance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_main_residence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_asset_main_residence_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "asset_type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asset_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "investment_property",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    EstimatedValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlyCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyElectricityCosts = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    MonthlyMortgagePayments = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MonthlyRevenue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MortgageDuration = table.Column<int>(type: "integer", nullable: false),
                    MortgageLeft = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MortgageStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    YearlyInsurance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_investment_property", x => x.Id);
                    table.ForeignKey(
                        name: "FK_investment_property_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "other_asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AssetTypeId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    CurrentValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
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
                name: "IX_account_type_CountryId",
                table: "account_type",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_account_type_SubdivisionId",
                table: "account_type",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_asset_main_residence_UserId",
                table: "asset_main_residence",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_investment_property_UserId",
                table: "investment_property",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_other_asset_AssetTypeId",
                table: "other_asset",
                column: "AssetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_other_asset_UserId",
                table: "other_asset",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_beginner_guide_debt_user_UserId",
                table: "beginner_guide_debt",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_investment_account_account_type_AccountTypeId",
                table: "investment_account",
                column: "AccountTypeId",
                principalTable: "account_type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_investment_account_user_UserId",
                table: "investment_account",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_investment_stock_investment_account_InvestmentAccountId",
                table: "investment_stock",
                column: "InvestmentAccountId",
                principalTable: "investment_account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
