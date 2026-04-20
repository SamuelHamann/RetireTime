using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpendingExpenseEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dashboard_spending_assets_expense",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Expense = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_spending_assets_expense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_assets_expense_common_frequencies_Freque~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_assets_expense_dashboard_scenario_Scenar~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_spending_debt_repayment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_spending_debt_repayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_debt_repayment_common_frequencies_Freque~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_debt_repayment_dashboard_scenario_Scenar~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_spending_discretionary_expenses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    GymMembership = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GymMembershipFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    Subscriptions = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    SubscriptionsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    EatingOut = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    EatingOutFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    Entertainment = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    EntertainmentFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    Travel = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    TravelFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CharitableDonations = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CharitableDonationsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    OtherDiscretionaryExpenses = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    OtherDiscretionaryExpensesFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_spending_discretionary_expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_discretionary_expenses_common_frequencie~",
                        column: x => x.CharitableDonationsFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~1",
                        column: x => x.EatingOutFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~2",
                        column: x => x.EntertainmentFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~3",
                        column: x => x.GymMembershipFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~4",
                        column: x => x.OtherDiscretionaryExpensesFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~5",
                        column: x => x.SubscriptionsFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_discretionary_expenses_common_frequenci~6",
                        column: x => x.TravelFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_discretionary_expenses_dashboard_scenari~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_spending_living_expenses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    RentOrMortgage = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    RentOrMortgageFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    Food = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FoodFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    Utilities = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    UtilitiesFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    Insurance = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    InsuranceFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    Gas = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    GasFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    HomeMaintenance = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    HomeMaintenanceFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    Cellphone = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    CellphoneFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    HealthSpendings = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    HealthSpendingsFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    OtherLivingExpenses = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    OtherLivingExpensesFrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_spending_living_expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_living_expenses_common_frequencies_Cellp~",
                        column: x => x.CellphoneFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_living_expenses_common_frequencies_FoodF~",
                        column: x => x.FoodFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_living_expenses_common_frequencies_GasFr~",
                        column: x => x.GasFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_living_expenses_common_frequencies_Healt~",
                        column: x => x.HealthSpendingsFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_living_expenses_common_frequencies_HomeM~",
                        column: x => x.HomeMaintenanceFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_living_expenses_common_frequencies_Insur~",
                        column: x => x.InsuranceFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_living_expenses_common_frequencies_Other~",
                        column: x => x.OtherLivingExpensesFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_living_expenses_common_frequencies_RentO~",
                        column: x => x.RentOrMortgageFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_living_expenses_common_frequencies_Utili~",
                        column: x => x.UtilitiesFrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_living_expenses_dashboard_scenario_Scena~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dashboard_spending_other_expense",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    FrequencyId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dashboard_spending_other_expense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_other_expense_common_frequencies_Frequen~",
                        column: x => x.FrequencyId,
                        principalTable: "common_frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_dashboard_spending_other_expense_dashboard_scenario_Scenari~",
                        column: x => x.ScenarioId,
                        principalTable: "dashboard_scenario",
                        principalColumn: "ScenarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_assets_expense_FrequencyId",
                table: "dashboard_spending_assets_expense",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_assets_expense_ScenarioId",
                table: "dashboard_spending_assets_expense",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_debt_repayment_FrequencyId",
                table: "dashboard_spending_debt_repayment",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_debt_repayment_ScenarioId",
                table: "dashboard_spending_debt_repayment",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_discretionary_expenses_CharitableDonatio~",
                table: "dashboard_spending_discretionary_expenses",
                column: "CharitableDonationsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_discretionary_expenses_EatingOutFrequenc~",
                table: "dashboard_spending_discretionary_expenses",
                column: "EatingOutFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_discretionary_expenses_EntertainmentFreq~",
                table: "dashboard_spending_discretionary_expenses",
                column: "EntertainmentFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_discretionary_expenses_GymMembershipFreq~",
                table: "dashboard_spending_discretionary_expenses",
                column: "GymMembershipFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_discretionary_expenses_OtherDiscretionar~",
                table: "dashboard_spending_discretionary_expenses",
                column: "OtherDiscretionaryExpensesFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_discretionary_expenses_ScenarioId",
                table: "dashboard_spending_discretionary_expenses",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_discretionary_expenses_SubscriptionsFreq~",
                table: "dashboard_spending_discretionary_expenses",
                column: "SubscriptionsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_discretionary_expenses_TravelFrequencyId",
                table: "dashboard_spending_discretionary_expenses",
                column: "TravelFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_CellphoneFrequencyId",
                table: "dashboard_spending_living_expenses",
                column: "CellphoneFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_FoodFrequencyId",
                table: "dashboard_spending_living_expenses",
                column: "FoodFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_GasFrequencyId",
                table: "dashboard_spending_living_expenses",
                column: "GasFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_HealthSpendingsFrequency~",
                table: "dashboard_spending_living_expenses",
                column: "HealthSpendingsFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_HomeMaintenanceFrequency~",
                table: "dashboard_spending_living_expenses",
                column: "HomeMaintenanceFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_InsuranceFrequencyId",
                table: "dashboard_spending_living_expenses",
                column: "InsuranceFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_OtherLivingExpensesFrequ~",
                table: "dashboard_spending_living_expenses",
                column: "OtherLivingExpensesFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_RentOrMortgageFrequencyId",
                table: "dashboard_spending_living_expenses",
                column: "RentOrMortgageFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_ScenarioId",
                table: "dashboard_spending_living_expenses",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_living_expenses_UtilitiesFrequencyId",
                table: "dashboard_spending_living_expenses",
                column: "UtilitiesFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_other_expense_FrequencyId",
                table: "dashboard_spending_other_expense",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_dashboard_spending_other_expense_ScenarioId",
                table: "dashboard_spending_other_expense",
                column: "ScenarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dashboard_spending_assets_expense");

            migrationBuilder.DropTable(
                name: "dashboard_spending_debt_repayment");

            migrationBuilder.DropTable(
                name: "dashboard_spending_discretionary_expenses");

            migrationBuilder.DropTable(
                name: "dashboard_spending_living_expenses");

            migrationBuilder.DropTable(
                name: "dashboard_spending_other_expense");
        }
    }
}
