using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBlogApi.Migrations
{
    public partial class correction_tenant_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TenantId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId1",
                schema: "dbo",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Tenants",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId1",
                schema: "dbo",
                table: "Users",
                column: "TenantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId1",
                schema: "dbo",
                table: "Users",
                column: "TenantId1",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId1",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TenantId1",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TenantId1",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tenants");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                schema: "dbo",
                table: "Users",
                column: "TenantId",
                unique: true,
                filter: "[TenantId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                schema: "dbo",
                table: "Users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
