using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_LocationInfos_LocationInfoId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "LocationInfos");

            migrationBuilder.RenameColumn(
                name: "LocationInfoId",
                table: "Users",
                newName: "CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_LocationInfoId",
                table: "Users",
                newName: "IX_Users_CountryId");

            migrationBuilder.AddColumn<int>(
                name: "SubdivisionId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Subdivisions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_SubdivisionId",
                table: "Users",
                column: "SubdivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subdivisions_CountryId",
                table: "Subdivisions",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subdivisions_Countries_CountryId",
                table: "Subdivisions",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Countries_CountryId",
                table: "Users",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Subdivisions_SubdivisionId",
                table: "Users",
                column: "SubdivisionId",
                principalTable: "Subdivisions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subdivisions_Countries_CountryId",
                table: "Subdivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Countries_CountryId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Subdivisions_SubdivisionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SubdivisionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Subdivisions_CountryId",
                table: "Subdivisions");

            migrationBuilder.DropColumn(
                name: "SubdivisionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Subdivisions");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Users",
                newName: "LocationInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CountryId",
                table: "Users",
                newName: "IX_Users_LocationInfoId");

            migrationBuilder.CreateTable(
                name: "LocationInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    SubdivisionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationInfos_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationInfos_Subdivisions_SubdivisionId",
                        column: x => x.SubdivisionId,
                        principalTable: "Subdivisions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationInfos_CountryId",
                table: "LocationInfos",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationInfos_SubdivisionId",
                table: "LocationInfos",
                column: "SubdivisionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_LocationInfos_LocationInfoId",
                table: "Users",
                column: "LocationInfoId",
                principalTable: "LocationInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
