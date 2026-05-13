using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRetirementTimeline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_retirement_spen~",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_retirement_spen~",
                table: "dashboard_spending_debt_repayment");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_dashboard_retirem~",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_living_expenses_dashboard_retirement_spe~",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_other_expense_dashboard_retirement_spend~",
                table: "dashboard_spending_other_expense");

            migrationBuilder.DropTable(
                name: "dashboard_retirement_spending");

            migrationBuilder.RenameColumn(
                name: "RetirementSpendingId",
                table: "dashboard_spending_other_expense",
                newName: "RetirementTimelineId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_other_expense_RetirementSpendingId",
                table: "dashboard_spending_other_expense",
                newName: "IX_dashboard_spending_other_expense_RetirementTimelineId");

            migrationBuilder.RenameColumn(
                name: "RetirementSpendingId",
                table: "dashboard_spending_living_expenses",
                newName: "RetirementTimelineId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_living_expenses_RetirementSpendingId",
                table: "dashboard_spending_living_expenses",
                newName: "IX_dashboard_spending_living_expenses_RetirementTimelineId");

            migrationBuilder.RenameColumn(
                name: "RetirementSpendingId",
                table: "dashboard_spending_discretionary_expenses",
                newName: "RetirementTimelineId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_discretionary_expenses_RetirementSpendin~",
                table: "dashboard_spending_discretionary_expenses",
                newName: "IX_dashboard_spending_discretionary_expenses_RetirementTimelin~");

            migrationBuilder.RenameColumn(
                name: "RetirementSpendingId",
                table: "dashboard_spending_debt_repayment",
                newName: "RetirementTimelineId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_debt_repayment_RetirementSpendingId",
                table: "dashboard_spending_debt_repayment",
                newName: "IX_dashboard_spending_debt_repayment_RetirementTimelineId");

            migrationBuilder.RenameColumn(
                name: "RetirementSpendingId",
                table: "dashboard_spending_assets_expense",
                newName: "RetirementTimelineId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_assets_expense_RetirementSpendingId",
                table: "dashboard_spending_assets_expense",
                newName: "IX_dashboard_spending_assets_expense_RetirementTimelineId");

            migrationBuilder.CreateTable(
                name: "dashboard_retirement_timeline",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AgeFrom = table.Column<int>(type: "integer", nullable: false),
                    AgeTo = table.Column<int>(type: "integer", nullable: false),
                    TimelineType = table.Column<int>(type: "integer", nullable: false, defaultValue: 2),
                    IsFullyCreated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_retirement_timeline", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_retirement_timeline_dashboard_scenario_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_retirement_timeline_ScenarioId",
                table: "dashboard_retirement_timeline",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_retirement_time~",
                table: "dashboard_spending_assets_expense",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_retirement_time~",
                table: "dashboard_spending_debt_repayment",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_dashboard_retirem~",
                table: "dashboard_spending_discretionary_expenses",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_living_expenses_dashboard_retirement_tim~",
                table: "dashboard_spending_living_expenses",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_other_expense_dashboard_retirement_timel~",
                table: "dashboard_spending_other_expense",
                column: "RetirementTimelineId",
                principalTable: "dashboard_retirement_timeline",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_retirement_time~",
                table: "dashboard_spending_assets_expense");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_retirement_time~",
                table: "dashboard_spending_debt_repayment");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_dashboard_retirem~",
                table: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_living_expenses_dashboard_retirement_tim~",
                table: "dashboard_spending_living_expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_dashboard_spending_other_expense_dashboard_retirement_timel~",
                table: "dashboard_spending_other_expense");

            migrationBuilder.DropTable(
                name: "dashboard_retirement_timeline");

            migrationBuilder.RenameColumn(
                name: "RetirementTimelineId",
                table: "dashboard_spending_other_expense",
                newName: "RetirementSpendingId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_other_expense_RetirementTimelineId",
                table: "dashboard_spending_other_expense",
                newName: "IX_dashboard_spending_other_expense_RetirementSpendingId");

            migrationBuilder.RenameColumn(
                name: "RetirementTimelineId",
                table: "dashboard_spending_living_expenses",
                newName: "RetirementSpendingId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_living_expenses_RetirementTimelineId",
                table: "dashboard_spending_living_expenses",
                newName: "IX_dashboard_spending_living_expenses_RetirementSpendingId");

            migrationBuilder.RenameColumn(
                name: "RetirementTimelineId",
                table: "dashboard_spending_discretionary_expenses",
                newName: "RetirementSpendingId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_discretionary_expenses_RetirementTimelin~",
                table: "dashboard_spending_discretionary_expenses",
                newName: "IX_dashboard_spending_discretionary_expenses_RetirementSpendin~");

            migrationBuilder.RenameColumn(
                name: "RetirementTimelineId",
                table: "dashboard_spending_debt_repayment",
                newName: "RetirementSpendingId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_debt_repayment_RetirementTimelineId",
                table: "dashboard_spending_debt_repayment",
                newName: "IX_dashboard_spending_debt_repayment_RetirementSpendingId");

            migrationBuilder.RenameColumn(
                name: "RetirementTimelineId",
                table: "dashboard_spending_assets_expense",
                newName: "RetirementSpendingId");

            migrationBuilder.RenameIndex(
                name: "IX_dashboard_spending_assets_expense_RetirementTimelineId",
                table: "dashboard_spending_assets_expense",
                newName: "IX_dashboard_spending_assets_expense_RetirementSpendingId");

            migrationBuilder.CreateTable(
                name: "dashboard_retirement_spending",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    AgeFrom = table.Column<int>(type: "integer", nullable: false),
                    AgeTo = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    IsFullyCreated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_retirement_spending", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_retirement_spending_dashboard_scenario_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_retirement_spending_ScenarioId",
                table: "dashboard_retirement_spending",
                column: "ScenarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_assets_expense_dashboard_retirement_spen~",
                table: "dashboard_spending_assets_expense",
                column: "RetirementSpendingId",
                principalTable: "dashboard_retirement_spending",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_debt_repayment_dashboard_retirement_spen~",
                table: "dashboard_spending_debt_repayment",
                column: "RetirementSpendingId",
                principalTable: "dashboard_retirement_spending",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_discretionary_expenses_dashboard_retirem~",
                table: "dashboard_spending_discretionary_expenses",
                column: "RetirementSpendingId",
                principalTable: "dashboard_retirement_spending",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_living_expenses_dashboard_retirement_spe~",
                table: "dashboard_spending_living_expenses",
                column: "RetirementSpendingId",
                principalTable: "dashboard_retirement_spending",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_dashboard_spending_other_expense_dashboard_retirement_spend~",
                table: "dashboard_spending_other_expense",
                column: "RetirementSpendingId",
                principalTable: "dashboard_retirement_spending",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
