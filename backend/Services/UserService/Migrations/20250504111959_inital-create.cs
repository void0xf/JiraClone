using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class initalcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    KeycloakUserId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FullNameValue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FullNamePrivacy = table.Column<int>(type: "integer", nullable: true),
                    PublicNameValue = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PublicNamePrivacy = table.Column<int>(type: "integer", nullable: true),
                    HeaderImageUrl = table.Column<string>(type: "text", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "text", nullable: false),
                    WorkedOn = table.Column<List<Guid>>(type: "uuid[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_KeycloakUserId",
                table: "Users",
                column: "KeycloakUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
