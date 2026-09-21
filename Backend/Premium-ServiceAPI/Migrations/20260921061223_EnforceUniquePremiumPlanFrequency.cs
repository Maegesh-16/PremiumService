using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Premium_ServiceAPI.Migrations
{
    /// <inheritdoc />
    public partial class EnforceUniquePremiumPlanFrequency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE [PremiumPlans]
                SET [Frequency] = CASE UPPER(REPLACE(LTRIM(RTRIM([Frequency])), '-', ''))
                    WHEN 'MONTHLY' THEN 'Monthly'
                    WHEN 'QUARTERLY' THEN 'Quarterly'
                    WHEN 'HALFYEARLY' THEN 'HalfYearly'
                    WHEN 'YEARLY' THEN 'Annual'
                    WHEN 'ANNUAL' THEN 'Annual'
                    WHEN 'ANNUALLY' THEN 'Annual'
                    ELSE LTRIM(RTRIM([Frequency]))
                END;
                """);

            migrationBuilder.DropIndex(
                name: "IX_PremiumPlans_PolicyTypeId",
                table: "PremiumPlans");

            migrationBuilder.CreateIndex(
                name: "IX_PremiumPlans_PolicyTypeId_Frequency",
                table: "PremiumPlans",
                columns: new[] { "PolicyTypeId", "Frequency" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PremiumPlans_PolicyTypeId_Frequency",
                table: "PremiumPlans");

            migrationBuilder.CreateIndex(
                name: "IX_PremiumPlans_PolicyTypeId",
                table: "PremiumPlans",
                column: "PolicyTypeId");
        }
    }
}
