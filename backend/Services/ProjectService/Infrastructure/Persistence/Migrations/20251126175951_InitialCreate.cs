using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectService.API.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryDescription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DetailsConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RecommendedFor = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IconKey = table.Column<string>(type: "text", nullable: false),
                    DetailsConfigId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateFeatures_DetailsConfigs_DetailsConfigId",
                        column: x => x.DetailsConfigId,
                        principalTable: "DetailsConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    ProjectCreationWizardTemplateCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CardConfig_Description = table.Column<string>(type: "text", nullable: false),
                    CardConfig_IconName = table.Column<string>(type: "text", nullable: false),
                    CardConfig_IconKey = table.Column<string>(type: "text", nullable: false),
                    DetailsConfigId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Categories_ProjectCreationWizardTemplateCategoryId",
                        column: x => x.ProjectCreationWizardTemplateCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Templates_DetailsConfigs_DetailsConfigId",
                        column: x => x.DetailsConfigId,
                        principalTable: "DetailsConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    DetailsConfigId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTypes_DetailsConfigs_DetailsConfigId",
                        column: x => x.DetailsConfigId,
                        principalTable: "DetailsConfigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateFeatures_DetailsConfigId",
                table: "TemplateFeatures",
                column: "DetailsConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Templates_DetailsConfigId",
                table: "Templates",
                column: "DetailsConfigId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Templates_ProjectCreationWizardTemplateCategoryId",
                table: "Templates",
                column: "ProjectCreationWizardTemplateCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTypes_DetailsConfigId",
                table: "WorkTypes",
                column: "DetailsConfigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateFeatures");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "WorkTypes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "DetailsConfigs");
        }
    }
}
