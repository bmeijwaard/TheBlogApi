using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBlogApi.Migrations
{
    public partial class nothumbnail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "Photos",
                newName: "FileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Photos",
                newName: "ThumbnailUrl");
        }
    }
}
