using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Premium_ServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPremiumPaymentLifecycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstallmentNumber",
                table: "PremiumSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "PremiumSchedules",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId",
                table: "PremiumSchedules",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentId",
                table: "PremiumHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PremiumSchedules_PaymentId",
                table: "PremiumSchedules",
                column: "PaymentId",
                unique: true,
                filter: "[PaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PremiumSchedules_PolicyId_DueDate",
                table: "PremiumSchedules",
                columns: new[] { "PolicyId", "DueDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PremiumHistories_PaymentId",
                table: "PremiumHistories",
                column: "PaymentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PremiumSchedules_PaymentId",
                table: "PremiumSchedules");

            migrationBuilder.DropIndex(
                name: "IX_PremiumSchedules_PolicyId_DueDate",
                table: "PremiumSchedules");

            migrationBuilder.DropIndex(
                name: "IX_PremiumHistories_PaymentId",
                table: "PremiumHistories");

            migrationBuilder.DropColumn(
                name: "InstallmentNumber",
                table: "PremiumSchedules");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "PremiumSchedules");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "PremiumSchedules");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "PremiumHistories");
        }
    }
}
