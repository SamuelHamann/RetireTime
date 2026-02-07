using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameMainResidenceTableToAssetMainResidence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_main_residence_user_UserId",
                table: "main_residence");

            migrationBuilder.DropPrimaryKey(
                name: "PK_main_residence",
                table: "main_residence");

            migrationBuilder.RenameTable(
                name: "main_residence",
                newName: "asset_main_residence");

            migrationBuilder.RenameIndex(
                name: "IX_main_residence_UserId",
                table: "asset_main_residence",
                newName: "IX_asset_main_residence_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_asset_main_residence",
                table: "asset_main_residence",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_asset_main_residence_user_UserId",
                table: "asset_main_residence",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_asset_main_residence_user_UserId",
                table: "asset_main_residence");

            migrationBuilder.DropPrimaryKey(
                name: "PK_asset_main_residence",
                table: "asset_main_residence");

            migrationBuilder.RenameTable(
                name: "asset_main_residence",
                newName: "main_residence");

            migrationBuilder.RenameIndex(
                name: "IX_asset_main_residence_UserId",
                table: "main_residence",
                newName: "IX_main_residence_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_main_residence",
                table: "main_residence",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_main_residence_user_UserId",
                table: "main_residence",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
