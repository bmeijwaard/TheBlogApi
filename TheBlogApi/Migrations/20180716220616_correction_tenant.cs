using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBlogApi.Migrations
{
    public partial class correction_tenant : Migration
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

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "Users",
                nullable: true,
                oldClrType: typeof(Guid));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TenantId",
                schema: "dbo",
                table: "Users");

            migrationBuilder.AlterColumn<Guid>(
                name: "TenantId",
                schema: "dbo",
                table: "Users",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

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
    }
}
