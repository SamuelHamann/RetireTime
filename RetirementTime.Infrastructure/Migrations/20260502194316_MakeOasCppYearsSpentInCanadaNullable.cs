using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeOasCppYearsSpentInCanadaNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "YearsSpentInCanada",
                table: "dashboard_income_oas_cpp",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "YearsSpentInCanada",
                table: "dashboard_income_oas_cpp",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
