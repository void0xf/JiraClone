using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectService.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTemplateFeaturesToCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TemplateFeatures_DetailsConfigId",
                table: "TemplateFeatures");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateFeatures_DetailsConfigId",
                table: "TemplateFeatures",
                column: "DetailsConfigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TemplateFeatures_DetailsConfigId",
                table: "TemplateFeatures");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateFeatures_DetailsConfigId",
                table: "TemplateFeatures",
                column: "DetailsConfigId",
                unique: true);
        }
    }
}
