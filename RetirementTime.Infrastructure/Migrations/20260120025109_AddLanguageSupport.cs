using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLanguageSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "user",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "en");

            migrationBuilder.CreateTable(
                name: "language",
                columns: table => new
                {
                    LanguageCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    LanguageName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_language", x => x.LanguageCode);
                });

            migrationBuilder.InsertData(
                table: "language",
                columns: new[] { "LanguageCode", "LanguageName" },
                values: new object[,]
                {
                    { "en", "English" },
                    { "fr", "French" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_LanguageCode",
                table: "user",
                column: "LanguageCode");

            migrationBuilder.AddForeignKey(
                name: "FK_user_language_LanguageCode",
                table: "user",
                column: "LanguageCode",
                principalTable: "language",
                principalColumn: "LanguageCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_language_LanguageCode",
                table: "user");

            migrationBuilder.DropTable(
                name: "language");

            migrationBuilder.DropIndex(
                name: "IX_user_LanguageCode",
                table: "user");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "user");
        }
    }
}
