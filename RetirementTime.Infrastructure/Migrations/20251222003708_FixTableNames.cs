using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subdivisions_Countries_CountryId",
                table: "Subdivisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Countries_CountryId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Subdivisions_SubdivisionId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_SpouseId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subdivisions",
                table: "Subdivisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Countries",
                table: "Countries");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "Subdivisions",
                newName: "subdivision");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "role");

            migrationBuilder.RenameTable(
                name: "Countries",
                newName: "country");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SubdivisionId",
                table: "user",
                newName: "IX_user_SubdivisionId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SpouseId",
                table: "user",
                newName: "IX_user_SpouseId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleId",
                table: "user",
                newName: "IX_user_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_CountryId",
                table: "user",
                newName: "IX_user_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_Subdivisions_CountryId",
                table: "subdivision",
                newName: "IX_subdivision_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_user",
                table: "user",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_subdivision",
                table: "subdivision",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_role",
                table: "role",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_country",
                table: "country",
                column: "Id");

            migrationBuilder.InsertData(
                table: "subdivision",
                columns: new[] { "Id", "Code", "CountryId", "Name" },
                values: new object[,]
                {
                    { 11, "NT", 1, "Northwest Territories" },
                    { 12, "NU", 1, "Nunavut" },
                    { 13, "YT", 1, "Yukon" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_subdivision_country_CountryId",
                table: "subdivision",
                column: "CountryId",
                principalTable: "country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_country_CountryId",
                table: "user",
                column: "CountryId",
                principalTable: "country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_RoleId",
                table: "user",
                column: "RoleId",
                principalTable: "role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_subdivision_SubdivisionId",
                table: "user",
                column: "SubdivisionId",
                principalTable: "subdivision",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_user_SpouseId",
                table: "user",
                column: "SpouseId",
                principalTable: "user",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subdivision_country_CountryId",
                table: "subdivision");

            migrationBuilder.DropForeignKey(
                name: "FK_user_country_CountryId",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_user_role_RoleId",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_user_subdivision_SubdivisionId",
                table: "user");

            migrationBuilder.DropForeignKey(
                name: "FK_user_user_SpouseId",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_subdivision",
                table: "subdivision");

            migrationBuilder.DropPrimaryKey(
                name: "PK_role",
                table: "role");

            migrationBuilder.DropPrimaryKey(
                name: "PK_country",
                table: "country");

            migrationBuilder.DeleteData(
                table: "subdivision",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "subdivision",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "subdivision",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.RenameTable(
                name: "user",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "subdivision",
                newName: "Subdivisions");

            migrationBuilder.RenameTable(
                name: "role",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "country",
                newName: "Countries");

            migrationBuilder.RenameIndex(
                name: "IX_user_SubdivisionId",
                table: "Users",
                newName: "IX_Users_SubdivisionId");

            migrationBuilder.RenameIndex(
                name: "IX_user_SpouseId",
                table: "Users",
                newName: "IX_Users_SpouseId");

            migrationBuilder.RenameIndex(
                name: "IX_user_RoleId",
                table: "Users",
                newName: "IX_Users_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_user_CountryId",
                table: "Users",
                newName: "IX_Users_CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_subdivision_CountryId",
                table: "Subdivisions",
                newName: "IX_Subdivisions_CountryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subdivisions",
                table: "Subdivisions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Countries",
                table: "Countries",
                column: "Id");

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
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Subdivisions_SubdivisionId",
                table: "Users",
                column: "SubdivisionId",
                principalTable: "Subdivisions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_SpouseId",
                table: "Users",
                column: "SpouseId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
