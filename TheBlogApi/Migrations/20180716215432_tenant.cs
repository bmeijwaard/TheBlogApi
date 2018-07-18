using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBlogApi.Migrations
{
    public partial class tenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "Users",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantKey = table.Column<string>(nullable: true),
                    IsBlocked = table.Column<bool>(nullable: false),
                    ExpirationDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                schema: "dbo",
                table: "Users",
                column: "TenantId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                schema: "dbo",
                table: "Users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Users_TenantId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "Users");
        }
    }
}
