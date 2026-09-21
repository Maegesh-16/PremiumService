using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Premium_ServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class AllowMultiplePremiumPlans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PremiumPlans_PolicyTypeId_Frequency",
                table: "PremiumPlans");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "PremiumPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_PremiumPlans_PolicyTypeId_Frequency_CreatedAtUtc",
                table: "PremiumPlans",
                columns: new[] { "PolicyTypeId", "Frequency", "CreatedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PremiumPlans_PolicyTypeId_Frequency_CreatedAtUtc",
                table: "PremiumPlans");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "PremiumPlans");

            migrationBuilder.CreateIndex(
                name: "IX_PremiumPlans_PolicyTypeId_Frequency",
                table: "PremiumPlans",
                columns: new[] { "PolicyTypeId", "Frequency" },
                unique: true);
        }
    }
}
