﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetirementTime.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixInvestmentAccountBulkAmountConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, fix existing data to comply with the constraint
            // Set BulkAmount to NULL for accounts where IsBulkAmount is false
            migrationBuilder.Sql(@"
                UPDATE investment_account
                SET ""BulkAmount"" = NULL
                WHERE ""IsBulkAmount"" = false AND ""BulkAmount"" IS NOT NULL;
            ");
            
            // For bulk amount accounts without a value, set a default value
            // (This shouldn't happen in normal operation, but just in case)
            migrationBuilder.Sql(@"
                UPDATE investment_account
                SET ""BulkAmount"" = 0
                WHERE ""IsBulkAmount"" = true AND ""BulkAmount"" IS NULL;
            ");
            
            // Delete any stocks for bulk amount accounts (shouldn't exist, but clean up if they do)
            migrationBuilder.Sql(@"
                DELETE FROM investment_stock
                WHERE ""InvestmentAccountId"" IN (
                    SELECT ""Id"" FROM investment_account WHERE ""IsBulkAmount"" = true
                );
            ");
            
            // Now add the constraint
            migrationBuilder.Sql(@"
                ALTER TABLE investment_account 
                ADD CONSTRAINT ""CK_InvestmentAccount_BulkAmount_XOR_Stocks"" 
                CHECK ((""IsBulkAmount"" = true AND ""BulkAmount"" IS NOT NULL) OR (""IsBulkAmount"" = false AND ""BulkAmount"" IS NULL));
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE investment_account 
                DROP CONSTRAINT IF EXISTS ""CK_InvestmentAccount_BulkAmount_XOR_Stocks"";
            ");
        }
    }
}

