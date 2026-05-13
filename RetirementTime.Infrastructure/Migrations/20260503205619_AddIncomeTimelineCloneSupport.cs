using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIncomeTimelineCloneSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RetirementTimelineId",
                table: "dashboard_income_share_purchase_plan",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementTimelineId",
                table: "dashboard_income_self_employment_income",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementTimelineId",
                table: "dashboard_income_pension_defined_contribution",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementTimelineId",
                table: "dashboard_income_pension_defined_benefits",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementTimelineId",
                table: "dashboard_income_other_income_or_benefits",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementTimelineId",
                table: "dashboard_income_oas_cpp",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementTimelineId",
                table: "dashboard_income_group_rrsp",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementTimelineId",
                table: "dashboard_income_employment_income",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RetirementTimelineId",
                table: "dashboard_income_defined_profit_sharing",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_share_purchase_plan_RetirementTimelineId",
                table: "dashboard_income_share_purchase_plan",
                column: "RetirementTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_self_employment_income_RetirementTimelineId",
                table: "dashboard_income_self_employment_income",
                column: "RetirementTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_pension_defined_contribution_RetirementTim~",
                table: "dashboard_income_pension_defined_contribution",
                column: "RetirementTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_pension_defined_benefits_RetirementTimelin~",
                table: "dashboard_income_pension_defined_benefits",
                column: "RetirementTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_other_income_or_benefits_RetirementTimelin~",
                table: "dashboard_income_other_income_or_benefits",
                column: "RetirementTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_oas_cpp_RetirementTimelineId",
                table: "dashboard_income_oas_cpp",
                column: "RetirementTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_group_rrsp_RetirementTimelineId",
                table: "dashboard_income_group_rrsp",
                column: "RetirementTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_employment_income_RetirementTimelineId",
                table: "dashboard_income_employment_income",
                column: "RetirementTimelineId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_income_defined_profit_sharing_RetirementTimelineId",
                table: "dashboard_income_defined_profit_sharing",
                column: "RetirementTimelineId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_defined_profit_sharing_dashboard_retiremen~",
                table: "dashboard_income_defined_profit_sharing",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_employment_income_dashboard_retirement_tim~",
                table: "dashboard_income_employment_income",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_group_rrsp_dashboard_retirement_timeline_R~",
                table: "dashboard_income_group_rrsp",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_oas_cpp_dashboard_retirement_timeline_Reti~",
                table: "dashboard_income_oas_cpp",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_other_income_or_benefits_dashboard_retirem~",
                table: "dashboard_income_other_income_or_benefits",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_pension_defined_benefits_dashboard_retirem~",
                table: "dashboard_income_pension_defined_benefits",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_pension_defined_contribution_dashboard_ret~",
                table: "dashboard_income_pension_defined_contribution",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_self_employment_income_dashboard_retiremen~",
                table: "dashboard_income_self_employment_income",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_income_share_purchase_plan_dashboard_retirement_t~",
                table: "dashboard_income_share_purchase_plan",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_defined_profit_sharing_dashboard_retiremen~",
                table: "dashboard_income_defined_profit_sharing");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_employment_income_dashboard_retirement_tim~",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_group_rrsp_dashboard_retirement_timeline_R~",
                table: "dashboard_income_group_rrsp");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_oas_cpp_dashboard_retirement_timeline_Reti~",
                table: "dashboard_income_oas_cpp");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_other_income_or_benefits_dashboard_retirem~",
                table: "dashboard_income_other_income_or_benefits");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_pension_defined_benefits_dashboard_retirem~",
                table: "dashboard_income_pension_defined_benefits");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_pension_defined_contribution_dashboard_ret~",
                table: "dashboard_income_pension_defined_contribution");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_self_employment_income_dashboard_retiremen~",
                table: "dashboard_income_self_employment_income");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_income_share_purchase_plan_dashboard_retirement_t~",
                table: "dashboard_income_share_purchase_plan");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_share_purchase_plan_RetirementTimelineId",
                table: "dashboard_income_share_purchase_plan");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_self_employment_income_RetirementTimelineId",
                table: "dashboard_income_self_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_pension_defined_contribution_RetirementTim~",
                table: "dashboard_income_pension_defined_contribution");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_pension_defined_benefits_RetirementTimelin~",
                table: "dashboard_income_pension_defined_benefits");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_other_income_or_benefits_RetirementTimelin~",
                table: "dashboard_income_other_income_or_benefits");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_oas_cpp_RetirementTimelineId",
                table: "dashboard_income_oas_cpp");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_group_rrsp_RetirementTimelineId",
                table: "dashboard_income_group_rrsp");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_employment_income_RetirementTimelineId",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropIndex(
                name: "IX_dashboard_income_defined_profit_sharing_RetirementTimelineId",
                table: "dashboard_income_defined_profit_sharing");

            migrationBuilder.DropColumn(
                name: "RetirementTimelineId",
                table: "dashboard_income_share_purchase_plan");

            migrationBuilder.DropColumn(
                name: "RetirementTimelineId",
                table: "dashboard_income_self_employment_income");

            migrationBuilder.DropColumn(
                name: "RetirementTimelineId",
                table: "dashboard_income_pension_defined_contribution");

            migrationBuilder.DropColumn(
                name: "RetirementTimelineId",
                table: "dashboard_income_pension_defined_benefits");

            migrationBuilder.DropColumn(
                name: "RetirementTimelineId",
                table: "dashboard_income_other_income_or_benefits");

            migrationBuilder.DropColumn(
                name: "RetirementTimelineId",
                table: "dashboard_income_oas_cpp");

            migrationBuilder.DropColumn(
                name: "RetirementTimelineId",
                table: "dashboard_income_group_rrsp");

            migrationBuilder.DropColumn(
                name: "RetirementTimelineId",
                table: "dashboard_income_employment_income");

            migrationBuilder.DropColumn(
                name: "RetirementTimelineId",
                table: "dashboard_income_defined_profit_sharing");
        }
    }
}
