using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalTracker.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRecordModelAndContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Accounts_FinancialAccountaccount_id",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_FinancialAccountaccount_id",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "FinancialAccountaccount_id",
                table: "Records");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Records",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Records_AccountId",
                table: "Records",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Accounts_AccountId",
                table: "Records",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "account_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Accounts_AccountId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_AccountId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Records");

            migrationBuilder.AddColumn<Guid>(
                name: "FinancialAccountaccount_id",
                table: "Records",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_FinancialAccountaccount_id",
                table: "Records",
                column: "FinancialAccountaccount_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Accounts_FinancialAccountaccount_id",
                table: "Records",
                column: "FinancialAccountaccount_id",
                principalTable: "Accounts",
                principalColumn: "account_id");
        }
    }
}
